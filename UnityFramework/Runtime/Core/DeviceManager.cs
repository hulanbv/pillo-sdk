using UnityEngine;

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_TVOS)
using System.Runtime.InteropServices;
#endif

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Core {
  /// <summary>
  /// The Device Manager class manages the Device Manager Native Plugin.
  /// </summary>
  internal static class DeviceManager {
    /// <summary>
    /// Exposed Device Manager Native Plugin method to instantiate itself.
    /// </summary>
#if UNITY_EDITOR
    static void DeviceManagerInstantiate() {
      Debug.LogWarning("Instantiating the Device Manager is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    static extern void DeviceManagerInstantiate();
#else
    static void DeviceManagerInstantiate () {
      Debug.LogWarning ("Instantiating the Device Manager is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to cancel a Peripheral 
    /// connection.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    static void DeviceManagerCancelPeripheralConnection(string identifier) {
      Debug.LogWarning("Cancelling a Peripheral connection is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    static extern void DeviceManagerCancelPeripheralConnection(string identifier);
#else
    static void DeviceManagerCancelPeripheralConnection (string identifier) {
      Debug.LogWarning ("Cancelling a Peripheral connection is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to power off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    static void DeviceManagerPowerOffPeripheral(string identifier) {
      Debug.LogWarning("Powering off a Peripheral is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    static extern void DeviceManagerPowerOffPeripheral(string identifier);
#else
    static void DeviceManagerPowerOffPeripheral (string identifier) {
      Debug.LogWarning ("Powering off a Peripheral is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Forces the LED of a Peripheral to be turned off.
    /// Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="enabled">Defines whether the LED should be forced off.</param>
#if UNITY_EDITOR
    static void DeviceManagerForceLedOff(string identifier, bool enabled) {
      Debug.LogWarning("Forcing the LED state of a Peripheral is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    static extern void DeviceManagerForceLedOff(string identifier, bool enabled);
#else
    static void DeviceManagerForceLedOff (string identifier) {
      Debug.LogWarning ("Forcing the LED state of a Peripheral is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to start a Peripheral
    /// calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    static void DeviceManagerStartPeripheralCalibration(string identifier) {
      Debug.LogWarning("Starting a Peripheral calibration is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport("__Internal")]
    static extern void DeviceManagerStartPeripheralCalibration(string identifier);
#else
    static void DeviceManagerStartPeripheralCalibration (string identifier) {
      Debug.LogWarning ("Starting a Peripheral calibration is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Initializes the Device Manager.
    /// </summary>
    internal static void Instantiate() => DeviceManagerInstantiate();

    /// <summary>
    /// Cancels a Peripheral connection.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal static void CancelPeripheralConnection(string identifier) => DeviceManagerCancelPeripheralConnection(identifier);

    /// <summary>
    /// Powers off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal static void PowerOffPeripheral(string identifier) => DeviceManagerPowerOffPeripheral(identifier);

    /// <summary>
    /// Forces the LEDs of a Peripheral to be turned off.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="enabled">The state of the LED.</param>
    internal static void ForceLedOff(string identifier, bool enabled) => DeviceManagerForceLedOff(identifier, enabled);

    /// <summary>
    /// Starts a Peripheral calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal static void StartPeripheralCalibration(string identifier) => DeviceManagerStartPeripheralCalibration(identifier);
  }
}