using System.Runtime.InteropServices;
using Hulan.Pillo.SDK.Core;
using UnityEngine;

// Pillo Framework Unity SDK
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK {

  /// <summary>
  /// The Pillo Framework exposes a set of methods and delegates to interact 
  /// with the Pillo Peripherals, Services and Characteristics.
  /// </summary>
  public static class PilloFramework {

    /// <summary>
    /// Exposed method which invoked a native method from within the Pillo
    /// Framework. This initializes the Pillo Framework and sets up the
    /// connection to the Pillo Peripherals.
    /// </summary>
    [DllImport ("__Internal")] private static extern void PilloFrameworkInitialize ();

    /// <summary>
    /// Delegate invoked when the Framework has been initialized.
    /// </summary>
    public static DelegateDefinitions.OnDidInitialize onDidInitialize;

    /// <summary>
    /// Delegate invoked when the Framework has failed to initialize.
    /// </summary>
    public static DelegateDefinitions.OnDidFailToInitialize onDidFailToInitialize;

    /// <summary>
    /// Delegate invoked when a Pillo has been connected.
    /// </summary>
    public static DelegateDefinitions.OnPilloDidConnect onPilloDidConnect;

    /// <summary>
    /// Delegate invoked when a Pillo has been disconnected.
    /// </summary>
    public static DelegateDefinitions.OnPilloDidDisconnect onPilloDidDisconnect;

    /// <summary>
    /// Delegate invoked when a Pillo has failed to connect.
    /// </summary>
    public static DelegateDefinitions.OnPilloDidFailToConnect onPilloDidFailToConnect;

    /// <summary>
    /// Delegate invoked when the battery level has changed.
    /// </summary>
    public static DelegateDefinitions.OnBatteryLevelDidChange onBatteryLevelDidChange;

    /// <summary>
    /// Delegate invoked when the Pillo Peripherals's pressure has ben changed.
    /// </summary>
    public static DelegateDefinitions.OnPressureDidChange onPressureDidChange;

    /// <summary>
    /// Initializes the Pillo Framework and sets up the connection to the
    /// Pillo Peripherals. This method should be invoked before any other
    /// Pillo Framework methods are invoked.
    /// </summary>
    public static void Initialize () {
      // Instantiates the Callback Listener.
      CallbackListener.Instantiate ();
#if !UNITY_EDITOR && UNITY_TVOS
      // Initialize the native Pillo Framework when we're on the right
      // platform while not being in the Unity Editor.
      PilloFrameworkInitialize ();
#endif
    }
  }
}