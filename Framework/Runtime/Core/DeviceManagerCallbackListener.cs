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
  [AddComponentMenu ("")]
  internal class DeviceManagerCallbackListener : MonoBehaviour {
    /// <summary>
    /// Method invoked by the Device Manager when the Central has been init-
    /// ialized.
    /// </summary>
    internal void OnCentralDidInitialize () {
      PilloFramework.onCentralDidInitialize ();
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Central has failed to ini-
    /// tialize.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnCentralDidFailToInitialize (string payloadJson) {
      if (PilloFramework.onCentralDidFailToInitialize != null) {
        var payload = JsonUtility.FromJson<CentralDidFailToInitializePayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onCentralDidFailToInitialize (payload.message);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Device Manager when a Peripheral did connect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidConnect (string payloadJson) {
      if (PilloFramework.onPeripheralDidConnect != null) {
        var payload = JsonUtility.FromJson<PeripheralDidConnectPayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onPeripheralDidConnect (payload.identifier);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Device Manager when a Peripheral did disconnect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidDisconnect (string payloadJson) {
      if (PilloFramework.onPeripheralDidDisconnect != null) {
        var payload = JsonUtility.FromJson<PeripheralDidDisconnectPayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onPeripheralDidDisconnect (payload.identifier);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Device Manager when a Peripheral did fail to con-
    /// nect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidFailToConnect (string payloadJson) {
      if (PilloFramework.onPeripheralDidFailToConnect != null) {
        var payload = JsonUtility.FromJson<PeripheralDidFailToConnectPayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onPeripheralDidFailToConnect (payload.identifier);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's battery level
    /// did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralBatteryLevelDidChange (string payloadJson) {
      if (PilloFramework.onPeripheralBatteryLevelDidChange != null) {
        var payload = JsonUtility.FromJson<PeripheralBatteryLevelDidChangePayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onPeripheralBatteryLevelDidChange (payload.identifier, payload.batteryLevel);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's pressure did
    /// change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralPressureDidChange (string payloadJson) {
      if (PilloFramework.onPeripheralPressureDidChange != null) {
        var payload = JsonUtility.FromJson<PeripheralPressureDidChangePayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onPeripheralPressureDidChange (payload.identifier, payload.pressure);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Device Manager when the Peripheral's charge state
    /// did change.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralChargeStateDidChange (string payloadJson) {
      if (PilloFramework.onPeripheralChargeStateDidChange != null) {
        var payload = JsonUtility.FromJson<PeripheralChargeStateDidChangePayload> (payloadJson);
        if (payload != null) {
          PilloFramework.onPeripheralChargeStateDidChange (payload.identifier, payload.chargeState);
        }
      }
    }

    /// <summary>
    /// This instantiates a new GameObject and adds this class as its component
    /// in order to receive callbacks from the native plugin.
    /// </summary>
    internal static void Instantiate () {
      // TODO -- Replace the need of this class with actual callbacks.
      var gameObject = new GameObject ("~DeviceManagerCallbackListener");
      gameObject.AddComponent<DeviceManagerCallbackListener> ();
      gameObject.hideFlags = HideFlags.HideInHierarchy;
      GameObject.DontDestroyOnLoad (gameObject);
    }
  }
}