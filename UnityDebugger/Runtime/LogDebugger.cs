using UnityEngine;
using Hulan.PilloSDK.DeviceManager;

namespace Hulan.PilloSDK.Debugger {
  /// <summary>
  /// The Pillo Test MonoBehaviour will be debug log all the incoming Pillo
  /// Device Manager events.
  /// </summary>
  [AddComponentMenu("Pillo SDK/Debugger/Log Debugger")]
  class LogDebugger : MonoBehaviour {
    /// <summary>
    /// Binds the Pillo Device Manager events.
    /// </summary>
    void Awake() {
      PilloDeviceManager.onCentralDidInitialize += OnCentralDidInitialize;
      PilloDeviceManager.onCentralDidFailToInitialize += OnCentralDidFailToInitialize;
      PilloDeviceManager.onCentralDidStartScanning += OnCentralDidStartScanning;
      PilloDeviceManager.onCentralDidStopScanning += OnCentralDidStopScanning;
      PilloDeviceManager.onPeripheralDidConnect += OnPeripheralDidConnect;
      PilloDeviceManager.onPeripheralDidDisconnect += OnPeripheralDidDisconnect;
      PilloDeviceManager.onPeripheralDidFailToConnect += OnPeripheralDidFailToConnect;
      PilloDeviceManager.onPeripheralBatteryLevelDidChange += OnPeripheralBatteryLevelDidChange;
      PilloDeviceManager.onPeripheralPressureDidChange += OnPeripheralPressureDidChange;
      PilloDeviceManager.onPeripheralChargingStateDidChange += OnPeripheralChargingStateDidChange;
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange += OnPeripheralFirmwareVersionDidChange;
      PilloDeviceManager.onPeripheralHardwareVersionDidChange += OnPeripheralHardwareVersionDidChange;
      PilloDeviceManager.onPeripheralModelNumberDidChange += OnPeripheralModelNumberDidChange;
    }

    /// <summary>
    /// Unbinds the Pillo Device Manager events.
    /// </summary>
    void Destroy() {
      PilloDeviceManager.onCentralDidInitialize -= OnCentralDidInitialize;
      PilloDeviceManager.onCentralDidFailToInitialize -= OnCentralDidFailToInitialize;
      PilloDeviceManager.onCentralDidStartScanning -= OnCentralDidStartScanning;
      PilloDeviceManager.onCentralDidStopScanning -= OnCentralDidStopScanning;
      PilloDeviceManager.onPeripheralDidConnect -= OnPeripheralDidConnect;
      PilloDeviceManager.onPeripheralDidDisconnect -= OnPeripheralDidDisconnect;
      PilloDeviceManager.onPeripheralDidFailToConnect -= OnPeripheralDidFailToConnect;
      PilloDeviceManager.onPeripheralBatteryLevelDidChange -= OnPeripheralBatteryLevelDidChange;
      PilloDeviceManager.onPeripheralPressureDidChange -= OnPeripheralPressureDidChange;
      PilloDeviceManager.onPeripheralChargingStateDidChange -= OnPeripheralChargingStateDidChange;
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange -= OnPeripheralFirmwareVersionDidChange;
      PilloDeviceManager.onPeripheralHardwareVersionDidChange -= OnPeripheralHardwareVersionDidChange;
      PilloDeviceManager.onPeripheralModelNumberDidChange -= OnPeripheralModelNumberDidChange;
    }

    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    void OnCentralDidInitialize() {
      Debug.Log("Pillo Device Manager Central Did Initialize");
    }

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    void OnCentralDidFailToInitialize(string message) {
      Debug.Log($"Pillo Device Manager Central Did Fail To Initialize with message {message}");
    }

    /// <summary>
    /// Delegate will be invoked when the Central has started scanning.
    /// </summary>
    void OnCentralDidStartScanning() {
      Debug.Log("Pillo Device Manager Central Did Start Scanning");
    }

    /// <summary>
    /// Delegate will be invoked when the Central has stopped scanning.
    /// </summary>
    void OnCentralDidStopScanning() {
      Debug.Log("Pillo Device Manager Central Did Stop Scanning");
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidConnect(string identifier) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Did Connect");
    }

    /// <summary>
    /// Delegate should be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidDisconnect(string identifier) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Did Disconnect");
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidFailToConnect(string identifier) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Did Fail To Connect");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The battery level of the Peripheral.</param>
    void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Battery Level Did Change to {batteryLevel}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel"> The pressure of the Peripheral.</param>
    void OnPeripheralPressureDidChange(string identifier, int pressure) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Pressure Did Change to {pressure}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargingState">The charge state of the Peripheral.</param>
    void OnPeripheralChargingStateDidChange(string identifier, ChargingState chargingState) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Charge State Did Change to {chargingState}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's model number did change.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="modelNumber"></param>
    void OnPeripheralModelNumberDidChange(string identifier, string modelNumber) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Model Number Did Change to {modelNumber}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's hardware version did 
    /// change.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="hardwareVersion"></param>
    void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Hardware Version Did Change to {hardwareVersion}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's firmware version did
    /// change.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="firmwareVersion"></param>
    void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) {
      Debug.Log($"Pillo Device Manager Peripheral with identifier {identifier} Firmware Version Did Change to {firmwareVersion}");
    }
  }
}
