#if UNITY_EDITOR
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using System.IO;
using Hulan.PilloSDK.Core.Framework;

namespace Hulan.PilloSDK.Editor {
  public static class PilloEditorMenuItems {

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidConnect (1)")]
    private static void SimulateOnPeripheralDidConnect1 () {
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidConnect", "faux-pillo-1");
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidConnect (2)")]
    private static void SimulateOnPeripheralDidConnect2 () {
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidConnect", "faux-pillo-2");
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidDisconnect (1)")]
    private static void SimulateOnPeripheralDidDisconnect1 () {
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidDisconnect", "faux-pillo-1");
    }

    [MenuItem ("Pillo SDK/Input System/Simulate OnPeripheralDidDisconnect (2)")]
    private static void SimulateOnPeripheralDidDisconnect2 () {
      GameObject.Find ("~PilloFrameworkCallbackListener").SendMessage ("OnPeripheralDidDisconnect", "faux-pillo-2");
    }
  }
}
#endif
