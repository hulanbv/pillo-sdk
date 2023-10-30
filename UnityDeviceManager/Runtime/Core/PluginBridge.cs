using UnityEngine;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
using System.Runtime.InteropServices;
#endif

namespace Hulan.PilloSDK.DeviceManager.Core {
  /// <summary>
  /// The Plugin Bridge manages the Device Manager Native Plugin.
  /// </summary>
  public static class PluginBridge {
    /// <summary>
    /// Exposed Device Manager Native Plugin method to set the delegates.
    /// </summary>
#if UNITY_EDITOR
    internal static void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
      Debug.LogWarning("Setting the Device Manager delegates is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
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
#if UNITY_EDITOR
    internal static void CancelPeripheralConnection(string identifier) {
      Debug.LogWarning("Cancelling a Peripheral connection is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
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
#if UNITY_EDITOR
    internal static void PowerOffPeripheral(string identifier) {
      Debug.LogWarning("Powering off a Peripheral is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
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
#if UNITY_EDITOR
    internal static void ForcePeripheralLedOff(string identifier, bool enabled) {
      Debug.LogWarning("Forcing the LED state of a Peripheral is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    internal static extern void ForcePeripheralLedOff(string identifier, bool enabled);
#else
    internal static void ForcePeripheralLedOff(string identifier) {
      Debug.LogWarning("Forcing the LED state of a Peripheral is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to start a Peripheral
    /// calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    internal static void StartPeripheralCalibration(string identifier) {
      Debug.LogWarning("Starting a Peripheral calibration is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    internal static extern void StartPeripheralCalibration(string identifier);
#else
    internal static void StartPeripheralCalibration(string identifier) {
      Debug.LogWarning("Starting a Peripheral calibration is not supported on the current platform.");
    }
#endif
  }
}