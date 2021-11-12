using System.Runtime.InteropServices;
using UnityEngine;

// Pillo Framework Unity SDK
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK.Core {

  /// <summary>
  /// Containing the delegate definitions for the Pillo SDK.
  /// </summary>
  public static class PilloFramework {

    /// <summary>
    /// Exposed method which invoked a native method from within the Pillo
    /// Framework. This initializes the Pillo Framework and sets up the
    /// connection to the Pillo Peripherals.
    /// </summary>
    [DllImport ("__Internal")]
    private static extern void PilloFrameworkInitialize ();

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// invokes the Native Pillo Framework Initialization Method.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
#if UNITY_EDITOR == false && UNITY_TVOS == true
      // Initialize the native Pillo Framework when we're on the right platform
      // while not being in the Unity Editor.
      PilloFrameworkInitialize ();
#endif
    }
  }
}