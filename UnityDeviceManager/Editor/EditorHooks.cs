using System.Diagnostics;
using Hulan.PilloSDK.DeviceManager.Core;
using UnityEditor;
using UnityEditor.Compilation;

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
      UnityEngine.Debug.Log("EditorHooks: Initializing Device Manager service...");
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
      AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
      // EditorApplication.quitting += OnEditorQuitting;
    }

    /// <summary>
    /// Stops the Device Manager service when the Unity Editor exits play mode.
    /// This is necessary to ensure that the service does not continue running
    /// in the background after exiting play mode.
    /// </summary>
    private static void OnPlayModeStateChanged(PlayModeStateChange state) {
      if (state == PlayModeStateChange.ExitingPlayMode) {
        PluginBridge.StopService();
        UnityEngine.Debug.Log("EditorHooks: Stopping Device Manager service on exiting play mode.");
      }
    }

    private static void OnBeforeAssemblyReload() {
      PluginBridge.StopService();
      UnityEngine.Debug.Log("EditorHooks: Stopping Device Manager service before assembly reload.");
    }

    private static void OnEditorQuitting() {
      PluginBridge.StopService();
      UnityEngine.Debug.Log("EditorHooks: Stopping Device Manager service on editor quitting.");
    }
  }
}