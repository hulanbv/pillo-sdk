using System.Runtime.InteropServices;
using UnityEngine;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan

namespace Hulan.PilloSDK.Framework {

  /// <summary>
  /// The Pillo Framework is responsible binding the events sent by the Pillo
  /// Frmaework iOS Plugin to the Unity Engine. It also exposes a set of methods
  /// and delegates to interact with the Pillo Framework. The data coming from
  /// and going to the iOS Plugin can be matched one to one.
  /// </summary>
  internal class PilloFramework {

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