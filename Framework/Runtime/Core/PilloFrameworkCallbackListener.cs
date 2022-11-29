using UnityEngine;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// The Callback Listener MonoBehaviour will be assigned to a specificly
  /// named GameObject in the scene. This GameObject will be created by the
  /// Pillo Framework and is used to listen for Pillo Framework events invoked
  /// by the native Pillo Framework.
  /// </summary>
  [AddComponentMenu ("")]
  internal class PilloFrameworkCallbackListener : MonoBehaviour {
    /// <summary>
    /// Method invoked by the native Pillo Framework when it has been 
    /// initialized.
    /// </summary>
    internal void OnCentralDidInitialize () {
      PilloFramework.onCentralDidInitialize ();
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when it has failed to
    /// initialize.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnCentralDidFailToInitialize (string payloadJson) {
      var payload = JsonUtility.FromJson<CentralDidFailToInitializePayload> (payloadJson);
      PilloFramework.onCentralDidFailToInitialize (payload.message);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// connected.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidConnect (string payloadJson) {
      var payload = JsonUtility.FromJson<PeripheralDidConnectPayload> (payloadJson);
      PilloFramework.onPeripheralDidConnect (payload.identifier);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// disconnected.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidDisconnect (string payloadJson) {
      var payload = JsonUtility.FromJson<PeripheralDidDisconnectPayload> (payloadJson);
      PilloFramework.onPeripheralDidDisconnect (payload.identifier);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has failed
    /// to connect.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralDidFailToConnect (string payloadJson) {
      var payload = JsonUtility.FromJson<PeripheralDidFailToConnectPayload> (payloadJson);
      PilloFramework.onPeripheralDidFailToConnect (payload.identifier);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the battery level
    /// has changed.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralBatteryLevelDidChange (string payloadJson) {
      var payload = JsonUtility.FromJson<PeripheralBatteryLevelDidChangePayload> (payloadJson);
      PilloFramework.onPeripheralBatteryLevelDidChange (payload.identifier, payload.batteryLevel);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the Pillo 
    /// Peripherals's pressure has been changed.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralPressureDidChange (string payloadJson) {
      var payload = JsonUtility.FromJson<PeripheralPressureDidChangePayload> (payloadJson);
      PilloFramework.onPeripheralPressureDidChange (payload.identifier, payload.pressure);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the Pillo 
    /// Peripherals's charge state has been changed.
    /// </summary>
    /// <param name="payloadJson">The payload as JSON.</param>
    internal void OnPeripheralChargeStateDidChange (string payloadJson) {
      var payload = JsonUtility.FromJson<PeripheralChargeStateDidChangePayload> (payloadJson);
      PilloFramework.onPeripheralChargeStateDidChange (payload.identifier, payload.chargeState);
    }

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// Instantiates a new Callback Listener GameObject and assigns it to the
    /// Pillo Framework Callback Listener which receives messages from the
    /// Native Pillo Framework.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
      var gameObject = new GameObject ("~PilloFrameworkCallbackListener");
      gameObject.AddComponent<PilloFrameworkCallbackListener> ();
      gameObject.hideFlags = HideFlags.HideInHierarchy;
      GameObject.DontDestroyOnLoad (gameObject);
    }
  }
}