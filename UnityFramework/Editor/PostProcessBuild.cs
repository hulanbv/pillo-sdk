#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;
#if UNITY_IOS || UNITY_TVOS
using UnityEditor.iOS.Xcode;
#endif

namespace Hulan.PilloSDK.Framework.Editor {
  /// <summary>
  /// Contains the methods to hook into the Unity Editor build process.
  /// </summary>
  static class PostProcessBuild {
    /// <summary>
    /// When the Unity build is succesfull, this hook will be invoked.
    /// </summary>
    [PostProcessBuild]
    static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject) {
#if UNITY_IOS || UNITY_TVOS
      // This adds the required usage descriptions to the Info Property List to 
      // the  Xcode project. This is required in order to run the Application on 
      // the target hardware.
      var plistPath = pathToBuiltProject + "/Info.plist";
      var plist = new PlistDocument();
      plist.ReadFromString(File.ReadAllText(plistPath));
      // Adds the required usage descriptions to the Info Property List.
      plist.root.SetString("NSBluetoothPeripheralUsageDescription", "Allow Pillo Play to connect to the Pillos.");
      plist.root.SetString("NSBluetoothAlwaysUsageDescription", "Allow Pillo Play to connect to the Pillos.");
      // Writes the Info Property List back to the Xcode project.
      File.WriteAllText(plistPath, plist.WriteToString());
#endif
    }
  }
}
#endif
