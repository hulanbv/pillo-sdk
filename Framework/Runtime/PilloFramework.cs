using Hulan.PilloSDK.Framework.Core;
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
  public class PilloFramework {
    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    public static PilloFrameworkDelegate.OnCentralDidInitialize onCentralDidInitialize = delegate { };

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    public static PilloFrameworkDelegate.OnCentralDidFailToInitialize onCentralDidFailToInitialize = delegate { };

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralDidConnect onPeripheralDidConnect = delegate { };

    /// <summary>
    /// Delegate will be invoked when a Peripheral did disconnect.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralDidDisconnect onPeripheralDidDisconnect = delegate { };

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect = delegate { };

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did 
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange = delegate { };

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralPressureDidChange onPeripheralPressureDidChange = delegate { };

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralChargeStateDidChange onPeripheralChargeStateDidChange = delegate { };

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// invokes the Device Manager Native Plugin's Initialization Method.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
      // Even though the Device Manager Initialization Method is available, it
      // should only be invoked when the Pillo Framework is running in a non
      // Editor environment.
#if UNITY_EDITOR == false
      DeviceManager.Instantiate ();
      DeviceManagerCallbackListener.Instantiate ();
#endif
    }

    public static void CancelPeripheralConnection (string identifier) {
#if UNITY_EDITOR == false
      DeviceManager.CancelPeripheralConnection (identifier);
#endif
    }
  }
}