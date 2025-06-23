#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_IOS || UNITY_TVOS
using System.Runtime.InteropServices;
#else
using UnityEngine;
#endif

// TODO: clean-up when unity editor stops running
// TODO: Check if iOS can also use the PilloDeviceManager plugin so the source code cna be moved away
// TODO: And/otherwise also check if the changes to the m, mm and h file didnt break the iOS build

namespace Hulan.PilloSDK.DeviceManager.Core {
  /// <summary>
  /// The Plugin Bridge manages the Device Manager Native Plugin.
  /// </summary>
  public static class PluginBridge {
    /// <summary>
    /// Exposed Device Manager Native Plugin method to start the Device Manager
    /// service.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerStartService")]
    internal static extern void StartService();
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerStartService")]
    internal static extern void StartService();
#else
    internal static void StartService() {
      Debug.LogWarning("Starting the Device Manager service is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to stop the Device Manager
    /// service.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerStopService")]
    internal static extern void StopService();
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerStopService")]
    internal static extern void StopService();
#else
    internal static void StopService() {
      Debug.LogWarning("Stoping the Device Manager service is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to set the delegates.
    /// </summary>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerSetDelegates")]
    internal static extern void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerSetDelegates")]
    internal static extern void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange);
#else
    internal static void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
      Debug.LogWarning("Setting the Device Manager delegates is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to cancel a Peripheral 
    /// connection.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerCancelPeripheralConnection")]
    internal static extern void CancelPeripheralConnection(string identifier);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerCancelPeripheralConnection")]
    internal static extern void CancelPeripheralConnection(string identifier);
#else
    internal static void CancelPeripheralConnection(string identifier) {
      Debug.LogWarning("Cancelling a Peripheral connection is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to power off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerPowerOffPeripheral")]
    internal static extern void PowerOffPeripheral(string identifier);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerPowerOffPeripheral")]
    internal static extern void PowerOffPeripheral(string identifier);
#else
    internal static void PowerOffPeripheral(string identifier) {
      Debug.LogWarning("Powering off a Peripheral is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Forces the LED of a Peripheral to be turned off.
    /// Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="enabled">Defines whether the LED should be forced off.</param>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerForcePeripheralLedOff")]
    internal static extern void ForcePeripheralLedOff(string identifier, bool enabled);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerForcePeripheralLedOff")]
    internal static extern void ForcePeripheralLedOff(string identifier, bool enabled);
#else
    internal static void ForcePeripheralLedOff(string identifier, bool enabled) {
      Debug.LogWarning("Forcing the LED state of a Peripheral is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to start a Peripheral
    /// calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    [DllImport("PilloDeviceManager", EntryPoint = "PilloDeviceManagerStartPeripheralCalibration")]
    internal static extern void StartPeripheralCalibration(string identifier);
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal", EntryPoint = "PilloDeviceManagerStartPeripheralCalibration")]
    internal static extern void PeripheralCalibration(string identifier);
#else
    internal static void StartPeripheralCalibration(string identifier) {
      Debug.LogWarning("Starting a Peripheral calibration is not supported on the current platform.");
    }
#endif
  }
}