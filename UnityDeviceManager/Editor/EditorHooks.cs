#if UNITY_EDITOR
using Hulan.PilloSDK.DeviceManager.Core;
using UnityEditor;

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
  }
}
#endif