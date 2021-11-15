using UnityEngine;
using Hulan.Pillo.SDK.InputSystem;

// Pillo SDK for Unity
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK.Core.Framework {

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
    internal void OnDidInitialize () {
      PilloInput.OnDidInitialize ();
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when it has failed to
    /// initialize.
    /// </summary>
    /// <param name="payload">Contaning the reason.</param>
    internal void OnDidFailToInitialize (string payload) {
      PilloInput.OnDidFailToInitialize (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// connected.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    internal void OnDeviceDidConnect (string payload) {
      PilloInput.OnDeviceDidConnect (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// disconnected.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    internal void OnDeviceDidDisconnect (string payload) {
      PilloInput.OnDeviceDidDisconnect (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has failed
    /// to connect.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    internal void OnDeviceDidFailToConnect (string payload) {
      PilloInput.OnDeviceDidFailToConnect (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the battery level
    /// has changed.
    /// </summary>
    /// <param name="payload">Containing the battery level.</param>
    internal void OnDeviceBatteryLevelDidChange (string payload) {
      var parts = payload.Split ('~');
      var identifier = parts[0];
      var batteryLevel = int.Parse (parts[1]);
      PilloInput.OnBatteryLevelDidChange (identifier, batteryLevel);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the Pillo 
    /// Peripherals's pressure has ben changed.
    /// </summary>
    /// <param name="payload">Containing the pressure.</param>
    internal void OnDevicePressureDidChange (string payload) {
      var parts = payload.Split ('~');
      var identifier = parts[0];
      var pressure = int.Parse (parts[1]);
      PilloInput.OnPressureDidChange (identifier, pressure);
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
      Object.DontDestroyOnLoad (gameObject);
    }
  }
}