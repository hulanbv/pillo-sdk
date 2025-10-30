namespace Hulan.PilloSDK.DeviceManager.Core {
  /// <summary>
  /// Platform-agnostic bridge interface for Device Manager plugin.
  /// </summary>
  public interface IPluginBridge {
    void StartService();
    void StopService();
    void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange);
    void CancelPeripheralConnection(string identifier);
    void PowerOffPeripheral(string identifier);
    void ForcePeripheralLedOff(string identifier, bool enabled);
    void StartPeripheralCalibration(string identifier);

    // Android-only. Non-Android platforms may implement as no-ops.
    void OnPermissionResult(int requestCode, string[] permissions, int[] grantResults);
    void OnBluetoothEnableResult(bool enabled);
  }
}