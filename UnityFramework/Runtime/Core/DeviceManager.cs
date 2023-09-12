using UnityEngine;
#if UNITY_EDITOR
using Hulan.PilloSDK.Framework.Editor;
#elif UNITY_IOS || UNITY_TVOS
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
    private static void _DeviceManagerInstantiate() {
      Debug.LogWarning("Instantiating the Device Manager is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport ("__Internal")]
    private static extern void _DeviceManagerInstantiate ();
#else
    private static void _DeviceManagerInstantiate () {
      Debug.LogWarning ("Instantiating the Device Manager is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to cancel a Peripheral 
    /// connection.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    private static void _DeviceManagerCancelPeripheralConnection(string identifier) {
      PilloFrameworkSimulator.RemovePeripheral(identifier);
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport ("__Internal")]
    private static extern void _DeviceManagerCancelPeripheralConnection (string identifier);
#else
    private static void _DeviceManagerCancelPeripheralConnection (string identifier) {
      Debug.LogWarning ("Cancelling a Peripheral connection is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to power off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    private static void _DeviceManagerPowerOffPeripheral(string identifier) {
      PilloFrameworkSimulator.RemovePeripheral(identifier);
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport ("__Internal")]
    private static extern void _DeviceManagerPowerOffPeripheral (string identifier);
#else
    private static void _DeviceManagerPowerOffPeripheral (string identifier) {
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
    private static void _DeviceManagerForceLedOff(string identifier, bool enabled) {
      Debug.LogWarning("Forcing the LED state of a Peripheral is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport ("__Internal")]
    private static extern void _DeviceManagerForceLedOff (string identifier, bool enabled);
#else
    private static void _DeviceManagerForceLedOff (string identifier) {
      Debug.LogWarning ("Forcing the LED state of a Peripheral is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Exposed Device Manager Native Plugin method to start a Peripheral
    /// calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
#if UNITY_EDITOR
    private static void _DeviceManagerStartPeripheralCalibration(string identifier) {
      Debug.LogWarning("Starting a Peripheral calibration is not supported in the Unity Editor.");
    }
#elif UNITY_IOS || UNITY_TVOS
    [DllImport ("__Internal")]
    private static extern void _DeviceManagerStartPeripheralCalibration (string identifier);
#else
    private static void _DeviceManagerStartPeripheralCalibration (string identifier) {
      Debug.LogWarning ("Starting a Peripheral calibration is not supported on the current platform.");
    }
#endif

    /// <summary>
    /// Initializes the Device Manager.
    /// </summary>
    internal static void Instantiate() => _DeviceManagerInstantiate();

    /// <summary>
    /// Cancels a Peripheral connection.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal static void CancelPeripheralConnection(string identifier) => _DeviceManagerCancelPeripheralConnection(identifier);

    /// <summary>
    /// Powers off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal static void PowerOffPeripheral(string identifier) => _DeviceManagerPowerOffPeripheral(identifier);

    /// <summary>
    /// Forces the LEDs of a Peripheral to be turned off.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="enabled">The state of the LED.</param>
    internal static void ForceLedOff(string identifier, bool enabled) => _DeviceManagerForceLedOff(identifier, enabled);

    /// <summary>
    /// Starts a Peripheral calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal static void StartPeripheralCalibration(string identifier) => _DeviceManagerStartPeripheralCalibration(identifier);
  }
}