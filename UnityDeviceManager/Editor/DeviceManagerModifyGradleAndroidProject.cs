#if UNITY_ANDROID && UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

using UnityEditor.Android;

public class DeviceManagerModifyGradleAndroidProject : IPostGenerateGradleAndroidProject {
  public int callbackOrder => 100;

  public void OnPostGenerateGradleAndroidProject(string path) {
    PatchBuildGradle(Path.Combine(path, "build.gradle"));
    PatchManifest(Path.Combine(path, "src", "main", "AndroidManifest.xml"));
  }

  void PatchBuildGradle(string gradlePath) {
    if (!File.Exists(gradlePath)) {
      Log($"build.gradle not found: {gradlePath}", true);
      return;
    }

    string[] wantedDeps = {
      // Android BLE support
      "androidx.bluetooth:bluetooth:1.0.0-alpha02",
      
      // Kotlin core libraries
      "org.jetbrains.kotlin:kotlin-stdlib:2.1.0",
      "org.jetbrains.kotlin:kotlin-stdlib-jdk8:2.1.0",
    };

    var lines = File.ReadAllLines(gradlePath).ToList();
    var inDepsBlock = false;
    var wroteResolutionBlock = lines.Any(l => l.Contains("resolutionStrategy.eachDependency"));
    var dependenciesWereInjected = false;

    var output = new List<string>();

    foreach (var line in lines) {
      var trimmed = line.Trim();

      // Detect start of dependencies block
      if (trimmed.StartsWith("dependencies {")) {
        inDepsBlock = true;
        output.Add(line);
        continue;
      }

      // Inside dependencies block
      if (inDepsBlock && trimmed == "}") {
        // Inject any missing dependencies before the closing brace
        if (!dependenciesWereInjected) {
          foreach (var dep in wantedDeps) {
            if (!lines.Any(l => l.Contains(dep)))
              output.Add($"    implementation '{dep}'");
          }
          dependenciesWereInjected = true;
        }

        inDepsBlock = false;
        output.Add(line);

        // Inject resolution strategy block AFTER dependencies block
        if (!wroteResolutionBlock) {
          output.Add("");
          output.Add("configurations.all {");
          output.Add("    resolutionStrategy.eachDependency { details ->");
          output.Add("        if (details.requested.group == \"org.jetbrains.kotlin\") {");
          output.Add("            details.useVersion(\"1.8.22\")");
          output.Add("        }");
          output.Add("    }");
          output.Add("}");
          wroteResolutionBlock = true;
        }

        continue;
      }

      // Normal line passthrough
      output.Add(line);
    }

    var utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    File.WriteAllLines(gradlePath, output, utf8NoBom);
  }

  void PatchManifest(string manifestPath) {
    if (!File.Exists(manifestPath)) {
      Log($"Manifest not found: {manifestPath}", true);
      return;
    }

    var doc = new XmlDocument();
    doc.Load(manifestPath);

    var nsMgr = new XmlNamespaceManager(doc.NameTable);
    nsMgr.AddNamespace("android", "http://schemas.android.com/apk/res/android");

    // Ensure BLUETOOTH permissions
    var bluetoothPermissions = new[] {
      "android.permission.BLUETOOTH",
      "android.permission.BLUETOOTH_ADMIN",
      "android.permission.BLUETOOTH_CONNECT"
    };

    foreach (var permission in bluetoothPermissions) {
      var usesPerm = doc.SelectSingleNode($"/manifest/uses-permission[@android:name='{permission}']", nsMgr);
      if (usesPerm == null) {
        var perm = doc.CreateElement("uses-permission");
        perm.SetAttribute("name", "http://schemas.android.com/apk/res/android", permission);
        doc.DocumentElement?.InsertBefore(perm, doc.DocumentElement.FirstChild);
      }
    }
    
    // For Android 12+ (API 31+), add BLUETOOTH_SCAN with neverForLocation flag to avoid requiring location permission
    // First, remove ALL existing BLUETOOTH_SCAN permissions to avoid duplicates
    var existingScanPerms = doc.SelectNodes("/manifest/uses-permission[@android:name='android.permission.BLUETOOTH_SCAN']", nsMgr);
    if (existingScanPerms != null) {
      foreach (XmlNode perm in existingScanPerms) {
        perm.ParentNode?.RemoveChild(perm);
      }
    }
    
    // Now add BLUETOOTH_SCAN with neverForLocation flag
    var flagsPerm = doc.CreateElement("uses-permission");
    flagsPerm.SetAttribute("name", "http://schemas.android.com/apk/res/android", "android.permission.BLUETOOTH_SCAN");
    flagsPerm.SetAttribute("usesPermissionFlags", "http://schemas.android.com/apk/res/android", "neverForLocation");
    doc.DocumentElement?.InsertBefore(flagsPerm, doc.DocumentElement.FirstChild);

    // Ensure PilloDeviceManager activity
    const string actFqn = "com.hulan.devicemanager.PilloDeviceManager";
    var actNode = doc.SelectSingleNode($"/manifest/application/activity[@android:name='{actFqn}']", nsMgr);
    if (actNode == null) {
      var appNode = doc.SelectSingleNode("/manifest/application");
      if (appNode == null) {
        appNode = doc.CreateElement("application");
        doc.DocumentElement?.AppendChild(appNode);
      }

      var act = doc.CreateElement("activity");
      act.SetAttribute("name", "http://schemas.android.com/apk/res/android", actFqn);
      act.SetAttribute("exported", "http://schemas.android.com/apk/res/android", "false");
      act.SetAttribute("theme", "http://schemas.android.com/apk/res/android", "@android:style/Theme.Material.Light.NoActionBar");
      appNode.AppendChild(act);
    }

    // Ensure Unity activity has hardware acceleration enabled
    PatchHardwareAcceleration(doc, nsMgr);

    doc.Save(manifestPath);
  }
  
  // Adds android:hardwareAccelerated="true" to the Unity player activity or the default Unity activity
  void PatchHardwareAcceleration(XmlDocument doc, XmlNamespaceManager nsMgr)
  {
    // Both activity names Unity may generate
    var unityActivities = new[]
    {
      "com.unity3d.player.UnityPlayerGameActivity",
      "com.unity3d.player.UnityPlayerActivity"
    };

    var patched = false;

    foreach (var actName in unityActivities)
    {
      var node = doc.SelectSingleNode($"/manifest/application/activity[@android:name='{actName}']", nsMgr);

      if (node == null) continue;

      // Ensure android:hardwareAccelerated="true"
      var accelAttr = node.Attributes?["android:hardwareAccelerated", "http://schemas.android.com/apk/res/android"];

      if (accelAttr == null)
      {
        accelAttr = doc.CreateAttribute(
          "android", "hardwareAccelerated",
          "http://schemas.android.com/apk/res/android");
        node.Attributes?.Append(accelAttr);
      }
      accelAttr.Value = "true";
      patched = true;
    }

    if (!patched)
      Debug.Log("No Unity player activity found; hardware-acceleration patch skipped.");
  }

  static void Log(string msg, bool warn = false) {
    if (warn) Debug.LogWarning("[DeviceManager] " + msg);
    else Debug.Log("[DeviceManager] " + msg);
  }
}
#endif
