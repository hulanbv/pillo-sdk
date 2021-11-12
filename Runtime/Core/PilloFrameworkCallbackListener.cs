using UnityEngine;

// Pillo Framework Unity SDK
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK.Core {

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
    private void OnDidInitialize () {
      PilloInput.onDidInitialize?.Invoke ();
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when it has failed to
    /// initialize.
    /// </summary>
    /// <param name="payload">Contaning the reason.</param>
    private void OnDidFailToInitialize (string payload) {
      PilloInput.onDidFailToInitialize?.Invoke (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// connected.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    private void OnDidConnect (string payload) {
      PilloInput.onDidConnect?.Invoke (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has been
    /// disconnected.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    private void OnDidDisconnect (string payload) {
      PilloInput.onDidDisconnect?.Invoke (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when a Pillo has failed
    /// to connect.
    /// </summary>
    /// <param name="payload">Contaning the Peripheral UUID.</param>
    private void OnDidFailToConnect (string payload) {
      PilloInput.onDidFailToConnect?.Invoke (payload);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the battery level
    /// has changed.
    /// </summary>
    /// <param name="payload">Containing the battery level.</param>
    private void OnBatteryLevelDidChange (string payload) {
      var parts = payload.Split ('~');
      var identifier = parts[0];
      var batteryLevel = int.Parse (parts[1]);
      PilloInput.onBatteryLevelDidChange?.Invoke (identifier, batteryLevel);
    }

    /// <summary>
    /// Method invoked by the native Pillo Framework when the Pillo 
    /// Peripherals's pressure has ben changed.
    /// </summary>
    /// <param name="payload">Containing the pressure.</param>
    private void OnPressureDidChange (string payload) {
      var parts = payload.Split ('~');
      var identifier = parts[0];
      var pressure = int.Parse (parts[1]);
      PilloInput.onPressureDidChange?.Invoke (identifier, pressure);
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