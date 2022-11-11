#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Editor {
  /// <summary>
  /// Contains the methods to hook into the Unity Editor.
  /// </summary>
  internal static class PilloEditorMenuItems {
    /// <summary>
    /// Opens the releases GitHub page.
    /// </summary>
    [MenuItem ("Pillo SDK/Open GitHub Releases", false, 0)]
    private static void UpdatePackage () {
      Application.OpenURL ("https://github.com/hulanbv/pillo-sdk-package/releases");
    }
  }
}
#endif
