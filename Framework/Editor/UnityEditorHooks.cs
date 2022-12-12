#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
#if UNITY_IOS || UNITY_TVOS
using UnityEditor.iOS.Xcode;
#endif

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// Contains the methods to hook into the Unity Editor.
  /// </summary>
  internal static class UnityEditorHooks {
    /// <summary>
    /// When the Unity build is succesfull, this hook will be invoked.
    /// </summary>
    [PostProcessBuild]
    private static void OnPostProcessBuild (BuildTarget buildTarget, string pathToBuiltProject) {
#if UNITY_IOS || UNITY_TVOS
      // This adds the required usage descriptions to the Info Property List to 
      // the  Xcode project. This is required in order to run the Application on 
      // the target hardware.
      var plistPath = pathToBuiltProject + "/Info.plist";
      var plist = new PlistDocument ();
      plist.ReadFromString (File.ReadAllText (plistPath));
      var rootDict = plist.root;
      // Adds the required usage descriptions to the Info Property List.
      rootDict.SetString ("NSBluetoothPeripheralUsageDescription", "Uses BLE to communicate with devices.");
      rootDict.SetString ("NSBluetoothAlwaysUsageDescription", "Uses BLE to communicate with devices.");
      // Writes the Info Property List back to the Xcode project.
      File.WriteAllText (plistPath, plist.WriteToString ());
#endif
    }
  }
}
#endif
