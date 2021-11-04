#if UNITY_EDITOR && UNITY_TVOS
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;

public class BluetoothPostProcessBuild {
  [PostProcessBuild]
  public static void ChangeXcodePlist (BuildTarget buildTarget, string pathToBuiltProject) {
    if (buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.tvOS) {
      var plistPath = pathToBuiltProject + "/Info.plist";
      var plist = new PlistDocument ();
      plist.ReadFromString (File.ReadAllText (plistPath));
      var rootDict = plist.root;
      rootDict.SetString ("NSBluetoothPeripheralUsageDescription", "Uses BLE to communicate with devices.");
      rootDict.SetString ("NSBluetoothAlwaysUsageDescription", "Uses BLE to communicate with devices.");
      File.WriteAllText (plistPath, plist.WriteToString ());
    }
  }
}
#endif
