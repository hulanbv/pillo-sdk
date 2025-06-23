using Hulan.PilloSDK.DeviceManager.Core;

namespace Hulan.PilloSDK.DeviceManager.Editor {
  /// <summary>
  /// The Editor Hooks class is used to manage the Device Manager Native Plugin
  /// in the Unity Editor.
  /// </summary>
  public static class EditorHooks {
    /// <summary>
    /// Registers a callback for the Unity Editor's play mode state changes.
    /// </summary>
    [UnityEditor.InitializeOnLoadMethod]
    private static void RegisterPlayModeStateChangedCallback() {
      UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    /// <summary>
    /// Stops the Device Manager service when the Unity Editor exits play mode.
    /// This is necessary to ensure that the service does not continue running
    /// in the background after exiting play mode.
    /// </summary>
    private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state) {
      if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode) {
        PluginBridge.StopService();
      }
    }
  }
}