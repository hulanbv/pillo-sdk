using System.Runtime.InteropServices;
using UnityEngine;

namespace Hulan.Pillo.SDK {

  public static class PilloFramework {

    /// <summary>
    /// Exposed method which invoked a native method from within the Pillo
    /// Framework. This initializes the Pillo Framework and sets up the
    /// connection to the Pillo Peripherals.
    /// </summary>
    [DllImport ("__Internal")] private static extern void PilloFrameworkInitialize ();

    public static class DelegateDefinitions {
      public delegate void OnDidInitialize ();
      public delegate void OnBluetoothDidInitialize ();
      public delegate void OnBluetoothDidFailToInitialize ();
      public delegate void OnPilloDidConnect (string identifier);
      public delegate void OnPilloDidFailToConnect (string identifier);
      public delegate void OnBatteryLevelDidChange (int batteryLevel);
      public delegate void OnPressureDidChange (int pressure);
    }

    public static DelegateDefinitions.OnDidInitialize onDidInitialize;
    public static DelegateDefinitions.OnBluetoothDidInitialize onBluetoothDidInitialize;
    public static DelegateDefinitions.OnBluetoothDidFailToInitialize onBluetoothDidFailToInitialize;
    public static DelegateDefinitions.OnPilloDidConnect onPilloDidConnect;
    public static DelegateDefinitions.OnPilloDidFailToConnect onPilloDidFailToConnect;
    public static DelegateDefinitions.OnBatteryLevelDidChange onBatteryLevelDidChange;
    public static DelegateDefinitions.OnPressureDidChange onPressureDidChange;

    /// <summary>
    /// The Callback Listener MonoBehaviour will be assigned to a specificly
    /// named GameObject in the scene. This GameObject will be created by the
    /// Pillo Framework and is used to listen for Pillo Framework events invoked
    /// by the native Pillo Framework.
    /// </summary>
    private class CallbackListener : MonoBehaviour {
      /// TODO check if private methods can be invoked from the native code.

      /// <summary>
      /// Method invoked by the native Pillo Framework when it has been 
      /// initialized.
      /// </summary>
      private void OnDidInitialize () {
        PilloFramework.onDidInitialize?.Invoke ();
      }

      /// <summary>
      /// Method invoked by the native Pillo Framework when the Bluetooth
      /// has been initialized.
      /// </summary>
      private void OnBluetoothDidInitialize () {
        PilloFramework.onBluetoothDidInitialize?.Invoke ();
      }

      /// <summary>
      /// Method invoked by the native Pillo Framework when the Bluetooth
      /// was unable to initialize and has been caught.
      /// </summary>
      private void OnBluetoothDidFailToInitialize () {
        PilloFramework.onBluetoothDidFailToInitialize?.Invoke ();
      }

      /// <summary>
      /// Method invoked by the native Pillo Framework when a Pillo has been
      /// connected.
      /// </summary>
      /// <param name="parameter">Contaning the Peripheral UUID.</param>
      private void OnPilloDidConnect (string parameter) {
        PilloFramework.onPilloDidConnect?.Invoke (parameter);
      }

      /// <summary>
      /// Method invoked by the native Pillo Framework when a Pillo has failed
      /// to connect.
      /// </summary>
      /// <param name="parameter">Contaning the Peripheral UUID.</param>
      private void OnPilloDidFailToConnect (string parameter) {
        PilloFramework.onPilloDidFailToConnect?.Invoke (parameter);
      }

      /// <summary>
      /// Method invoked by the native Pillo Framework when the battery level
      /// has changed.
      /// </summary>
      /// <param name="parameter">Containing the battery level.</param>
      private void OnBatteryLevelDidChange (string parameter) {
        PilloFramework.onBatteryLevelDidChange?.Invoke (int.Parse (parameter));
      }

      /// <summary>
      /// Method invoked by the native Pillo Framework when the Pillo 
      /// Peripherals's pressure has ben changed.
      /// </summary>
      /// <param name="parameter">Containing the pressure.</param>
      private void OnPressureDidChange (string parameter) {
        PilloFramework.onPressureDidChange?.Invoke (int.Parse (parameter));
      }
    }

    /// <summary>
    /// Initializes the Pillo Framework and sets up the connection to the
    /// Pillo Peripherals. This method should be invoked before any other
    /// Pillo Framework methods are invoked.
    /// </summary>
    public static void Initialize () {
      // We'll ensure there is only one instance of the Callback Listener.
      if (GameObject.FindObjectOfType<CallbackListener> () == null) {
        var gameObject = new GameObject ("~PilloFrameworkCallbackListener");
        var callbackListener = gameObject.AddComponent<CallbackListener> ();
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        Object.DontDestroyOnLoad (gameObject);
#if !UNITY_EDITOR && UNITY_TVOS
        // Initialize the native Pillo Framework when we're on the right
        // platform while not being in the Unity Editor.
        PilloFrameworkInitialize ();
#endif
      }
    }
  }
}