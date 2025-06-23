using Hulan.PilloSDK.DeviceManager.Core;

namespace Hulan.PilloSDK.DeviceManager.Editor {
  /// <summary>
  /// The Editor Hooks class is used to manage the Device Manager Native Plugin
  /// in the Unity Editor.
  /// </summary>
  public static class EditorHooks {
    [UnityEditor.InitializeOnLoadMethod]
    private static void RegisterPlayModeStateChangedCallback() {
      UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state) {
      if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode) {
        PluginBridge.StopService();
      }
    }
  }
}