using AOT;
using Hulan.PilloSDK.DeviceManager.Core;
using UnityEngine;

namespace Hulan.PilloSDK.DeviceManager {
  /// <summary>
  /// The Pillo Device Manager manages the Native Plugin.
  /// </summary>
  public class PilloDeviceManager {
    /// <summary>
    /// Mono Callback for the Central Did Initialize event.
    /// </summary>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidInitialize))]
    static void OnCentralDidInitialize() => onCentralDidInitialize?.Invoke();

    /// <summary>
    /// Mono Callback for the Central Did Fail To Initialize event.
    /// </summary>
    /// <param name="message">The error message.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidFailToInitialize))]
    static void OnCentralDidFailToInitialize(string message) => onCentralDidFailToInitialize?.Invoke(message);

    /// <summary>
    /// Mono Callback for the Central Did Start Scanning event.
    /// </summary>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidStartScanning))]
    static void OnCentralDidStartScanning() => onCentralDidStartScanning?.Invoke();

    /// <summary>
    /// Mono Callback for the Central Did Stop Scanning event.
    /// </summary>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidStopScanning))]
    static void OnCentralDidStopScanning() => onCentralDidStopScanning?.Invoke();

    /// <summary>
    /// Mono Callback for the Peripheral Did Connect event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidConnect))]
    static void OnPeripheralDidConnect(string identifier) => onPeripheralDidConnect?.Invoke(identifier);

    /// <summary>
    /// Mono Callback for the Peripheral Did Disconnect event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidDisconnect))]
    static void OnPeripheralDidDisconnect(string identifier) => onPeripheralDidDisconnect?.Invoke(identifier);

    /// <summary>
    /// Mono Callback for the Peripheral Did Fail To Connect event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidFailToConnect))]
    static void OnPeripheralDidFailToConnect(string identifier) => onPeripheralDidFailToConnect?.Invoke(identifier);

    /// <summary>
    /// Mono Callback for the Peripheral Battery Level Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="batteryLevel">The battery level of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralBatteryLevelDidChange))]
    static void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) => onPeripheralBatteryLevelDidChange?.Invoke(identifier, batteryLevel);

    /// <summary>
    /// Mono Callback for the Peripheral Pressure Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="pressure">The pressure of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralPressureDidChange))]
    static void OnPeripheralPressureDidChange(string identifier, int pressure) => onPeripheralPressureDidChange?.Invoke(identifier, pressure);

    /// <summary>
    /// Mono Callback for the Peripheral Charging State Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="chargingState">The charging state of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralChargingStateDidChange))]
    static void OnPeripheralChargingStateDidChange(string identifier, ChargingState chargingState) => onPeripheralChargingStateDidChange?.Invoke(identifier, chargingState);

    /// <summary>
    /// Mono Callback for the Peripheral Firmware Version Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="firmwareVersion">The firmware version of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralFirmwareVersionDidChange))]
    static void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) => onPeripheralFirmwareVersionDidChange?.Invoke(identifier, firmwareVersion);

    /// <summary>
    /// Mono Callback for the Peripheral Hardware Version Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="hardwareVersion">The hardware version of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralHardwareVersionDidChange))]
    static void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) => onPeripheralHardwareVersionDidChange?.Invoke(identifier, hardwareVersion);

    /// <summary>
    /// Mono Callback for the Peripheral Model Number Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="modelNumber">The model number of the peripheral.</param>
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
    /// Invoked when the Runtime Application initializes and is loaded.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializeOnLoad() {
      // Set the device manager delegates.
      PluginBridge.SetDelegates(OnCentralDidInitialize, OnCentralDidFailToInitialize, OnCentralDidStartScanning, OnCentralDidStopScanning, OnPeripheralDidConnect, OnPeripheralDidDisconnect, OnPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange, OnPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange);
    }

    /// <summary>
    /// Cancels a Peripheral connection.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void CancelPeripheralConnection(string identifier) {
      PluginBridge.CancelPeripheralConnection(identifier);
    }

    /// <summary>
    /// Powers off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void PowerOffPeripheral(string identifier) {
      PluginBridge.PowerOffPeripheral(identifier);
    }

    /// <summary>
    /// Forces the LED of a Peripheral to be turned off.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="enabled">Defines whether the LED should be forced off.</param>
    public static void ForcePeripheralLedOff(string identifier, bool enabled) {
      PluginBridge.ForcePeripheralLedOff(identifier, enabled);
    }

    /// <summary>
    /// Starts a Peripheral calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void StartPeripheralCalibration(string identifier) {
      PluginBridge.StartPeripheralCalibration(identifier);
    }
  }
}