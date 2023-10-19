using AOT;
using Hulan.PilloSDK.Framework.Core;
using UnityEngine;

namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// The Pillo Framework class manages the Pillo Framework Native Plugin.
  /// </summary>
  public class PilloFramework {
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidInitialize))]
    static void OnCentralDidInitialize() => onCentralDidInitialize?.Invoke();

    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidFailToInitialize))]
    static void OnCentralDidFailToInitialize(string message) => onCentralDidFailToInitialize?.Invoke(message);

    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidStartScanning))]
    static void OnCentralDidStartScanning() => onCentralDidStartScanning?.Invoke();

    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidStopScanning))]
    static void OnCentralDidStopScanning() => onCentralDidStopScanning?.Invoke();

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidConnect))]
    static void OnPeripheralDidConnect(string identifier) => onPeripheralDidConnect?.Invoke(identifier);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidDisconnect))]
    static void OnPeripheralDidDisconnect(string identifier) => onPeripheralDidDisconnect?.Invoke(identifier);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidFailToConnect))]
    static void OnPeripheralDidFailToConnect(string identifier) => onPeripheralDidFailToConnect?.Invoke(identifier);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralBatteryLevelDidChange))]
    static void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) => onPeripheralBatteryLevelDidChange?.Invoke(identifier, batteryLevel);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralPressureDidChange))]
    static void OnPeripheralPressureDidChange(string identifier, int pressure) => onPeripheralPressureDidChange?.Invoke(identifier, pressure);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralChargingStateDidChange))]
    static void OnPeripheralChargingStateDidChange(string identifier, ChargingState chargingState) => onPeripheralChargingStateDidChange?.Invoke(identifier, chargingState);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralFirmwareVersionDidChange))]
    static void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) => onPeripheralFirmwareVersionDidChange?.Invoke(identifier, firmwareVersion);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralHardwareVersionDidChange))]
    static void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) => onPeripheralHardwareVersionDidChange?.Invoke(identifier, hardwareVersion);

    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralModelNumberDidChange))]
    static void OnPeripheralModelNumberDidChange(string identifier, string modelNumber) => onPeripheralModelNumberDidChange?.Invoke(identifier, modelNumber);

    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    public static Delegates.OnCentralDidInitialize onCentralDidInitialize;

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    public static Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize;

    /// <summary>
    /// Delegate will be invoked when the Central has started scanning.
    /// </summary>
    public static Delegates.OnCentralDidStartScanning onCentralDidStartScanning;

    /// <summary>
    /// Delegate will be invoked when the Central has stopped scanning.
    /// </summary>
    public static Delegates.OnCentralDidStopScanning onCentralDidStopScanning;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    public static Delegates.OnPeripheralDidConnect onPeripheralDidConnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did disconnect.
    /// </summary>
    public static Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// </summary>
    public static Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did 
    /// </summary>
    public static Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    public static Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    public static Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's firmware version did 
    /// change.
    /// </summary>
    public static Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's hardware version did
    /// change.
    /// </summary>
    public static Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's model number did change.
    /// </summary>
    public static Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange;

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// invokes the Device Manager Native Plugin's Initialization Method.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    static void RuntimeInitializeOnLoad() {
      DeviceManager.Instantiate(OnCentralDidInitialize, OnCentralDidFailToInitialize, OnCentralDidStartScanning, OnCentralDidStopScanning, OnPeripheralDidConnect, OnPeripheralDidDisconnect, OnPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange, OnPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange);
    }

    /// <summary>
    /// Cancels a Peripheral connection.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void CancelPeripheralConnection(string identifier) {
      DeviceManager.CancelPeripheralConnection(identifier);
    }

    /// <summary>
    /// Powers off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void PowerOffPeripheral(string identifier) {
      DeviceManager.PowerOffPeripheral(identifier);
    }

    /// <summary>
    /// Forces the LED of a Peripheral to be turned off.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="enabled">Defines whether the LED should be forced off.</param>
    public static void ForceLedOff(string identifier, bool enabled) {
      DeviceManager.ForceLedOff(identifier, enabled);
    }

    /// <summary>
    /// Starts a Peripheral calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void StartPeripheralCalibration(string identifier) {
      DeviceManager.StartPeripheralCalibration(identifier);
    }
  }
}