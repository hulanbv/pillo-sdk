#if UNITY_EDITOR && (UNITY_TVOS || UNITY_IOS)
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Editor {
  /// <summary>
  /// Contains the methods to hook into the Unity Editor.
  /// </summary>
  internal static class UnityEditorHooks {
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
  }
}
#endif
