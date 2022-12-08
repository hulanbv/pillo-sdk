using Hulan.PilloSDK.Framework.Core;
using UnityEngine;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// The Pillo Framework is responsible for binding the events sent by the 
  /// Pillo Framework's Device Manager Native iOS Plugin to the Unity Engine.
  /// </summary>
  public class PilloFramework {
    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    public static PilloFrameworkDelegate.OnCentralDidInitialize onCentralDidInitialize;

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    public static PilloFrameworkDelegate.OnCentralDidFailToInitialize onCentralDidFailToInitialize;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralDidConnect onPeripheralDidConnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did disconnect.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralDidDisconnect onPeripheralDidDisconnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did 
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralPressureDidChange onPeripheralPressureDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    public static PilloFrameworkDelegate.OnPeripheralChargeStateDidChange onPeripheralChargeStateDidChange;

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// invokes the Device Manager Native Plugin's Initialization Method.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
#if UNITY_EDITOR == false
      DeviceManager.Instantiate ();
      DeviceManagerCallbackListener.Instantiate ();
#endif
    }

    /// <summary>
    /// Cancels a Peripheral connection.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void CancelPeripheralConnection (string identifier) {
#if UNITY_EDITOR == false
      DeviceManager.CancelPeripheralConnection (identifier);
#endif
    }

    /// <summary>
    /// Powers off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void PowerOffPeripheral (string identifier) {
#if UNITY_EDITOR == false
      DeviceManager.PowerOffPeripheral (identifier);
#endif
    }

    /// <summary>
    /// Starts a Peripheral calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void StartPeripheralCalibration (string identifier) {
#if UNITY_EDITOR == false
      DeviceManager.StartPeripheralCalibration (identifier);
#endif
    }
  }
}