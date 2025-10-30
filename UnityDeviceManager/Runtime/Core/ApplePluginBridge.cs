using System.Runtime.InteropServices;

namespace Hulan.PilloSDK.DeviceManager.Core {
  /// <summary>
  /// Apple platforms bridge (macOS, iOS, tvOS) for the Device Manager native plugin.
  /// </summary>
  public class ApplePluginBridge : IPluginBridge {
    /// <summary>
    /// Start the Device Manager service.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerStartService")]
    static extern void NativeStartService();
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerStartService")]
    static extern void NativeStartService();
#else
    static void NativeStartService() {}
#endif
    public void StartService() { NativeStartService(); }

    /// <summary>
    /// Stop the Device Manager service.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerStopService")]
    static extern void NativeStopService();
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerStopService")]
    static extern void NativeStopService();
#else
    static void NativeStopService() {}
#endif
    public void StopService() { NativeStopService(); }

    /// <summary>
    /// Set native delegates for callbacks.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerSetDelegates")]
    static extern void NativeSetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerSetDelegates")]
    static extern void NativeSetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange);
#else
    static void NativeSetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {}
#endif
    public void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) { NativeSetDelegates(onCentralDidInitialize, onCentralDidFailToInitialize, onCentralDidStartScanning, onCentralDidStopScanning, onPeripheralDidConnect, onPeripheralDidDisconnect, onPeripheralDidFailToConnect, onPeripheralBatteryLevelDidChange, onPeripheralPressureDidChange, onPeripheralChargingStateDidChange, onPeripheralFirmwareVersionDidChange, onPeripheralHardwareVersionDidChange, onPeripheralModelNumberDidChange); }

    /// <summary>
    /// Cancel a Peripheral connection.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerCancelPeripheralConnection")]
    static extern void NativeCancelPeripheralConnection(string identifier);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerCancelPeripheralConnection")]
    static extern void NativeCancelPeripheralConnection(string identifier);
#else
    static void NativeCancelPeripheralConnection(string identifier) {}
#endif
    public void CancelPeripheralConnection(string identifier) { NativeCancelPeripheralConnection(identifier); }

    /// <summary>
    /// Power off a Peripheral.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerPowerOffPeripheral")]
    static extern void NativePowerOffPeripheral(string identifier);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerPowerOffPeripheral")]
    static extern void NativePowerOffPeripheral(string identifier);
#else
    static void NativePowerOffPeripheral(string identifier) {}
#endif
    public void PowerOffPeripheral(string identifier) { NativePowerOffPeripheral(identifier); }

    /// <summary>
    /// Force Peripheral LED off.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerForcePeripheralLedOff")]
    static extern void NativeForcePeripheralLedOff(string identifier, bool enabled);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerForcePeripheralLedOff")]
    static extern void NativeForcePeripheralLedOff(string identifier, bool enabled);
#else
    static void NativeForcePeripheralLedOff(string identifier, bool enabled) {}
#endif
    public void ForcePeripheralLedOff(string identifier, bool enabled) { NativeForcePeripheralLedOff(identifier, enabled); }

    /// <summary>
    /// Start Peripheral calibration.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerStartPeripheralCalibration")]
    static extern void NativeStartPeripheralCalibration(string identifier);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerStartPeripheralCalibration")]
    static extern void NativeStartPeripheralCalibration(string identifier);
#else
    static void NativeStartPeripheralCalibration(string identifier) {}
#endif
    public void StartPeripheralCalibration(string identifier) { NativeStartPeripheralCalibration(identifier); }

    // Android-only no-ops
    public void OnPermissionResult(int requestCode, string[] permissions, int[] grantResults) {}
    public void OnBluetoothEnableResult(bool enabled) {}
  }
}


