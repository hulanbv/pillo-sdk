using Hulan.PilloSDK.Framework.Core;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// The Pillo Framework is responsible binding the events sent by the Pillo
  /// Frmaework iOS Plugin to the Unity Engine. It also exposes a set of methods
  /// and delegates to interact with the Pillo Framework. The data coming from
  /// and going to the iOS Plugin can be matched one to one.
  /// </summary>
  internal class PilloFramework {
    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    internal static PilloFrameworkDelegate.OnCentralDidInitialize onCentralDidInitialize;

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    internal static PilloFrameworkDelegate.OnCentralDidFailToInitialize onCentralDidFailToInitialize;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralDidConnect onPeripheralDidConnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did disconnect.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralDidDisconnect onPeripheralDidDisconnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did 
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralPressureDidChange onPeripheralPressureDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    internal static PilloFrameworkDelegate.OnPeripheralChargeStateDidChange onPeripheralChargeStateDidChange;
  }
}