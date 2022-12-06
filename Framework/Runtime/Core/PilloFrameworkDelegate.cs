// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Core {
  /// <summary>
  /// Containing the delegate definitions for the Pillo Framework.
  /// </summary>
  internal static class PilloFrameworkDelegate {
    /// <summary>
    /// Delegate should be invoked when the Central has been initialized.
    /// </summary>
    internal delegate void OnCentralDidInitialize ();

    /// <summary>
    /// Delegate should be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    internal delegate void OnCentralDidFailToInitialize (string message);

    /// <summary>
    /// Delegate should be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal delegate void OnPeripheralDidConnect (string identifier);

    /// <summary>
    /// Delegate should be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal delegate void OnPeripheralDidDisconnect (string identifier);

    /// <summary>
    /// Delegate should be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    internal delegate void OnPeripheralDidFailToConnect (string identifier);

    /// <summary>
    /// Delegate should be invoked when the Peripheral's battery level did 
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The battery level of the Peripheral.</param>
    internal delegate void OnPeripheralBatteryLevelDidChange (string identifier, int batteryLevel);

    /// <summary>
    /// Delegate should be invoked when the Peripheral's pressure did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel"> The pressure of the Peripheral.</param>
    internal delegate void OnPeripheralPressureDidChange (string identifier, int pressure);

    /// <summary>
    /// Delegate should be invoked when the Peripheral's charge state did 
    /// change.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargeState">The charge state of the Peripheral.</param>
    internal delegate void OnPeripheralChargeStateDidChange (string identifier, PeripheralChargeState chargeState);
  }
}