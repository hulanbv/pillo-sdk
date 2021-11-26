#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using System.IO;
using Hulan.PilloSDK.Framework;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

namespace Hulan.PilloSDK.Editor {
  internal static class PilloEditorMenuItems {
    private static AddRequest packageAddRequest;

    /// <summary>
    /// Updates the Pillo SDK package using the Unity Package manager.
    /// </summary>
    [MenuItem ("Pillo SDK/Update Package", false, 0)]
    private static void UpdatePackage () {
      PilloEditorMenuItems.packageAddRequest = Client.Add ("git+https://github.com/hulanbv/pillo-sdk-package");
      EditorApplication.update += PilloEditorMenuItems.OnEditorApplicationDidUpdate;
    }

    /// <summary>
    /// Event invoked everytime the Unity Editor updates.
    /// </summary>
    private static void OnEditorApplicationDidUpdate () {
      // We'll wait until the package is installed.
      if (PilloEditorMenuItems.packageAddRequest.IsCompleted == true) {
        if (PilloEditorMenuItems.packageAddRequest.Status == StatusCode.Success)
          Debug.Log ($"Pillo SDK Package Updated to version {PilloEditorMenuItems.packageAddRequest.Result.version}");
        else if (PilloEditorMenuItems.packageAddRequest.Status >= StatusCode.Failure)
          Debug.LogError ($"Something went wrong while updating {PilloEditorMenuItems.packageAddRequest.Error.message}");
        // If the package is installed, we can stop listening for updates.
        EditorApplication.update -= PilloEditorMenuItems.OnEditorApplicationDidUpdate;
      }
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidConnect (1)", false, 100)]
    private static void SimulateOnPeripheralDidConnect1 () {
      // Simlates a SendMessage to the Pillo Framework's Callback Listener
      // which tells the Pillo Framework that a peripheral has been connected.
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidConnect", "faux-pillo-1");
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidConnect (2)", false, 100)]
    private static void SimulateOnPeripheralDidConnect2 () {
      // Simlates a SendMessage to the Pillo Framework's Callback Listener
      // which tells the Pillo Framework that a peripheral has been connected.
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidConnect", "faux-pillo-2");
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidDisconnect (1)", false, 100)]
    private static void SimulateOnPeripheralDidDisconnect1 () {
      // Simlates a SendMessage to the Pillo Framework's Callback Listener
      // which tells the Pillo Framework that a peripheral has been disconnected.
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidDisconnect", "faux-pillo-1");
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidDisconnect (2)", false, 100)]
    private static void SimulateOnPeripheralDidDisconnect2 () {
      // Simlates a SendMessage to the Pillo Framework's Callback Listener
      // which tells the Pillo Framework that a peripheral has been disconnected.
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidDisconnect", "faux-pillo-2");
    }
  }
}
#endif
