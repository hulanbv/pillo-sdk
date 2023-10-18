using UnityEngine;
using Hulan.PilloSDK.Framework.Payloads;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Core {
  /// <summary>
  /// The Callback Listener MonoBehaviour will be assigned to a specifically
  /// named GameObject in the scene. This GameObject will be created automatic-
  /// ally and is used receive callbacks sent by the Device Manager Native Plug-
  /// in.
  /// </summary>
  [AddComponentMenu("")]
  internal class DeviceManagerCallbackListener : MonoBehaviour {
    /// <summary>
    /// Method invoked by the Device Manager when the Central has been init-
    /// ialized.
    /// </summary>
    internal void OnCentralDidInitialize() {
      PilloFramework.onCentralDidInitialize?.Invoke();
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Central has failed to ini-
    /// tialize.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnCentralDidFailToInitialize(string payloadJson) {
      if (PilloFramework.onCentralDidFailToInitialize == null) {
        return;
      }
      var payload = JsonUtility.FromJson<CentralDidFailToInitializePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onCentralDidFailToInitialize(payload.message);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Central has been started
    /// scanning.
    /// </summary>
    internal void OnCentralDidStartScanning() {
      PilloFramework.onCentralDidStartScanning?.Invoke();
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Central has been stopped
    /// scanning.
    /// </summary>
    internal void OnCentralDidStopScanning() {
      PilloFramework.onCentralDidStopScanning?.Invoke();
    }

    /// <summary>
    /// Method invoked by the Device Manager when a Peripheral did connect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidConnect(string payloadJson) {
      if (PilloFramework.onPeripheralDidConnect == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralDidConnectPayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralDidConnect(payload.identifier);
    }

    /// <summary>
    /// Method invoked by the Device Manager when a Peripheral did disconnect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidDisconnect(string payloadJson) {
      if (PilloFramework.onPeripheralDidDisconnect == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralDidDisconnectPayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralDidDisconnect(payload.identifier);
    }

    /// <summary>
    /// Method invoked by the Device Manager when a Peripheral did fail to con-
    /// nect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidFailToConnect(string payloadJson) {
      if (PilloFramework.onPeripheralDidFailToConnect == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralDidFailToConnectPayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralDidFailToConnect(payload.identifier);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's battery level
    /// did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralBatteryLevelDidChange(string payloadJson) {
      if (PilloFramework.onPeripheralBatteryLevelDidChange == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralBatteryLevelDidChangePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralBatteryLevelDidChange(payload.identifier, payload.batteryLevel);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's pressure did
    /// change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralPressureDidChange(string payloadJson) {
      if (PilloFramework.onPeripheralPressureDidChange == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralPressureDidChangePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralPressureDidChange(payload.identifier, payload.pressure);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's charge state
    /// did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralChargeStateDidChange(string payloadJson) {
      if (PilloFramework.onPeripheralChargeStateDidChange == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralChargeStateDidChangePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralChargeStateDidChange(payload.identifier, payload.chargeState);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's firware
    /// version did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralFirmwareVersionDidChange(string payloadJson) {
      if (PilloFramework.onPeripheralFirmwareVersionDidChange == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralFirmwareVersionDidChangePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralFirmwareVersionDidChange(payload.identifier, payload.firmwareVersion);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's hardware
    /// version did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralHardwareVersionDidChange(string payloadJson) {
      if (PilloFramework.onPeripheralHardwareVersionDidChange == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralHardwareVersionDidChangePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralHardwareVersionDidChange(payload.identifier, payload.hardwareVersion);
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's model number
    /// did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralModelNumberDidChange(string payloadJson) {
      if (PilloFramework.onPeripheralModelNumberDidChange == null) {
        return;
      }
      var payload = JsonUtility.FromJson<PeripheralModelNumberDidChangePayload>(payloadJson);
      if (payload == null) {
        return;
      }
      PilloFramework.onPeripheralModelNumberDidChange(payload.identifier, payload.modelNumber);
    }

    /// <summary>
    /// This instantiates a new GameObject and adds this class as its component
    /// in order to receive callbacks from the native plugin.
    /// </summary>
    internal static void Instantiate() {
      // TODO -- Replace the need of this class with actual callbacks.
      var gameObject = new GameObject("~DeviceManagerCallbackListener");
      gameObject.AddComponent<DeviceManagerCallbackListener>();
      gameObject.hideFlags = HideFlags.HideInHierarchy;
      DontDestroyOnLoad(gameObject);
    }
  }
}