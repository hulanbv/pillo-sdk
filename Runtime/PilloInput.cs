using System.Runtime.InteropServices;
using Hulan.Pillo.SDK.Core;
using UnityEngine;

// Pillo Framework Unity SDK
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK {

  /// <summary>
  /// The Pillo Input class exposes a set of methods and delegates to interact 
  /// with the Pillo Peripherals, Services and Characteristics.
  /// </summary>
  public static class PilloInput {

    /// <summary>
    /// Delegate invoked when the Framework has been initialized.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnDidInitialize onDidInitialize;

    /// <summary>
    /// Delegate invoked when the Framework has failed to initialize.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnDidFailToInitialize onDidFailToInitialize;

    /// <summary>
    /// Delegate invoked when a Pillo has been connected.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnDidConnect onDidConnect;

    /// <summary>
    /// Delegate invoked when a Pillo has been disconnected.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnDidDisconnect onDidDisconnect;

    /// <summary>
    /// Delegate invoked when a Pillo has failed to connect.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnDidFailToConnect onDidFailToConnect;

    /// <summary>
    /// Delegate invoked when the battery level has changed.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnBatteryLevelDidChange onBatteryLevelDidChange;

    /// <summary>
    /// Delegate invoked when the Pillo Peripherals's pressure has ben changed.
    /// </summary>
    public static PilloInputDelegateDefinitions.OnPressureDidChange onPressureDidChange;

    /// <summary>
    /// 
    /// </summary>
    public static int connectedDeviceCount;
  }
}