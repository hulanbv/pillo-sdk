using System.Runtime.InteropServices;
using UnityEngine;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Core {
  /// <summary>
  /// The Device Manager class manages the Device Manager Native Plugin.
  /// </summary>
  internal static class DeviceManager {
    /// <summary>
    /// Exposed Device Manager Native Plugin method to instantiate itself.
    /// </summary>
    [DllImport ("__Internal")]
    private static extern void InstantiateDeviceManager ();

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// invokes the Device Manager Native Plugin's Initialization Method.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
      // Even though the Pillo Framework Runtime Initialization Method is
      // available, it should only be invoked when the Pillo Framework is
      // running in a non Editor environment.
#if UNITY_EDITOR == false
      InstantiateDeviceManager ();
#endif
    }
  }
}