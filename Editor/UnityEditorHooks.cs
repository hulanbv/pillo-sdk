#if UNITY_EDITOR && (UNITY_TVOS || UNITY_IOS)
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using System.IO;
using Hulan.Pillo.SDK.Core.Framework;

namespace Hulan.Pillo.SDK.Editor {
  public static class UnityEditorHooks {

    /// <summary>
    /// When the Unity build is succesfull, this hook will be invoked. This adds
    /// the required usage descriptions to the Info PList to the Xcode project.
    /// This is required in order to run the Application on the target hardware.
    /// </summary>
    [PostProcessBuild]
    private static void OnPostProcessBuild (BuildTarget buildTarget, string pathToBuiltProject) {
      // Fetches the Info PList and parses it.
      var plistPath = pathToBuiltProject + "/Info.plist";
      var plist = new PlistDocument ();
      plist.ReadFromString (File.ReadAllText (plistPath));
      var rootDict = plist.root;
      // Adds the required usage descriptions to the Info PList.
      rootDict.SetString ("NSBluetoothPeripheralUsageDescription", "Uses BLE to communicate with devices.");
      rootDict.SetString ("NSBluetoothAlwaysUsageDescription", "Uses BLE to communicate with devices.");
      // Writes the Info PList back to the Xcode project.
      File.WriteAllText (plistPath, plist.WriteToString ());
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnDeviceDidConnect (1)")]
    private static void SimulateOnDeviceDidConnect1 () {
      if (Application.isPlaying == true) {
        var pilloFrameworkCallbackListener = GameObject.FindObjectOfType<PilloFrameworkCallbackListener> ();
        pilloFrameworkCallbackListener.OnDeviceDidConnect ("faux-pillo-1");
      }
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnDeviceDidConnect (2)")]
    private static void SimulateOnDeviceDidConnect2 () {
      if (Application.isPlaying == true) {
        var pilloFrameworkCallbackListener = GameObject.FindObjectOfType<PilloFrameworkCallbackListener> ();
        pilloFrameworkCallbackListener.OnDeviceDidConnect ("faux-pillo-2");
      }
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnDeviceDidDisconnect (1)")]
    private static void SimulateOnDeviceDidDisconnect1 () {
      if (Application.isPlaying == true) {
        var pilloFrameworkCallbackListener = GameObject.FindObjectOfType<PilloFrameworkCallbackListener> ();
        pilloFrameworkCallbackListener.OnDeviceDidDisconnect ("faux-pillo-1");
      }
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnDeviceDidDisconnect (2)")]
    private static void SimulateOnDeviceDidDisconnect2 () {
      if (Application.isPlaying == true) {
        var pilloFrameworkCallbackListener = GameObject.FindObjectOfType<PilloFrameworkCallbackListener> ();
        pilloFrameworkCallbackListener.OnDeviceDidDisconnect ("faux-pillo-2");
      }
    }
  }
}
#endif
