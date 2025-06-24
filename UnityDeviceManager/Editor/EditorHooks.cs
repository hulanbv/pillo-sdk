#if UNITY_EDITOR
using Hulan.PilloSDK.DeviceManager.Core;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;
using System;
#if UNITY_IOS || UNITY_TVOS
using UnityEditor.iOS.Xcode;
#endif

namespace Hulan.PilloSDK.DeviceManager.Editor {
  /// <summary>
  /// The Editor Hooks class is used to manage the Device Manager Native Plugin
  /// in the Unity Editor.
  /// </summary>
  [InitializeOnLoad]
  internal static class EditorHooks {
    /// <summary>
    /// Registers a callback for the Unity Editor's play mode state changes.
    /// </summary>
    static EditorHooks() {
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    /// <summary>
    /// Stops the Device Manager service when the Unity Editor exits play mode.
    /// This is necessary to ensure that the service does not continue running
    /// in the background after exiting play mode.
    /// </summary>
    private static void OnPlayModeStateChanged(PlayModeStateChange state) {
      if (state == PlayModeStateChange.ExitingPlayMode) {
        PluginBridge.StopService();
      }
    }

    /// <summary>
    /// When the Unity build is succesfull, this hook will be invoked. This adds
    /// the required usage descriptions to the Info Property List in the Xcode project.
    /// This is required in order to run the Application on the target hardware.
    /// The usage descriptions are required for Bluetooth connectivity.
    /// </summary>
    [PostProcessBuild]
    internal static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject) {
#if UNITY_IOS || UNITY_TVOS
      try {
        var plistPath = pathToBuiltProject + "/Info.plist";
        var plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        plist.root.SetString("NSBluetoothPeripheralUsageDescription", "Allow this app to connect to a Pillo.");
        plist.root.SetString("NSBluetoothAlwaysUsageDescription", "Allow this app to connect to a Pillo.");
        File.WriteAllText(plistPath, plist.WriteToString());
        Debug.Log("Pillo SDK added usage descriptions to Info.plist");
      }
      catch (Exception exception) {
        Debug.LogError($"Pillo SDK failed to add usage descriptions to Info.plist: {exception.Message}");
      }
#endif
    }
  }
}
#endif