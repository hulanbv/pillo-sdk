using UnityEngine;
using Hulan.Pillo.SDK.InputSystem;

// Unity Engine Pillo SDK Core Framework
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
    internal void OnCentralDidInitialize () {
      PilloInput.OnCentralDidInitialize ();
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when it has failed to
    /// initialize.
    /// </summary>
    /// <param name="payload">Contaning the reason.</param>
    internal void OnCentralDidFailToInitialize (string payload) {
      PilloInput.OnCentralDidFailToInitialize (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// connected.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    internal void OnPeripheralDidConnect (string payload) {
      PilloInput.OnPeripheralDidConnect (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// disconnected.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    internal void OnPeripheralDidDisconnect (string payload) {
      PilloInput.OnPeripheralDidDisconnect (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has failed
    /// to connect.
    /// </summary>
    internal void OnPeripheralDidFailToConnect () {
      PilloInput.OnPeripheralDidFailToConnect ();
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the battery level
    /// has changed.
    /// </summary>
    /// <param name="payload">Containing the battery level.</param>
    internal void OnPeripheralBatteryLevelDidChange (string payload) {
      var parts = payload.Split ('~');
      var identifier = parts[0];
      var batteryLevel = int.Parse (parts[1]);
      PilloInput.OnPeripheralBatteryLevelDidChange (identifier, batteryLevel);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the Pillo 
    /// Peripherals's pressure has ben changed.
    /// </summary>
    /// <param name="payload">Containing the pressure.</param>
    internal void OnPeripheralPressureDidChange (string payload) {
      var parts = payload.Split ('~');
      var identifier = parts[0];
      var pressure = int.Parse (parts[1]);
      PilloInput.OnPeripheralPressureDidChange (identifier, pressure);
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