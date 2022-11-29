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
    /// Delegate invoked when the Central has been initialized.
    /// </summary>
    internal static PilloFrameworkDelegate.OnCentralDidInitialize onCentralDidInitialize;

    /// <summary>
    /// Delegate invoked when the Framework has failed to initialize.
    /// </summary>
    internal static PilloFrameworkDelegate.OnCentralDidFailToInitialize onCentralDidFailToInitialize;

    /// <summary>
    /// Delegate invoked when a Peripheral has been connected.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralDidConnect onPeripheralDidConnect;

    /// <summary>
    /// Delegate invoked when a Peripheral has been disconnected.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralDidDisconnect onPeripheralDidDisconnect;

    /// <summary>
    /// Delegate invoked when a Peripheral has failed to connect.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;

    /// <summary>
    /// Delegate invoked when the Peripheral's battery level did change.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;

    /// <summary>
    /// Delegate invoked when the Peripheral's pressure did change.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralPressureDidChange onPeripheralPressureDidChange;

    /// <summary>
    /// Delegate invoked when the Peripheral's charge state did change.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralChargeStateDidChange onPeripheralChargeStateDidChange;

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// invokes the Native Pillo Framework Initialization Method.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
      // Even though the Pillo Framework Runtime Initialization Method is
      // available, it should only be invoked when the Pillo Framework is
      // running in a non Editor environment.
#if UNITY_EDITOR == false
      PilloFrameworkInitialize ();
#endif
    }
  }
}