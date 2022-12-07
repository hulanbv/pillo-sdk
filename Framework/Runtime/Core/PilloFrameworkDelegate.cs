// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Core {
  /// <summary>
  /// Containing the delegate definitions for the Pillo Framework.
  /// </summary>
  public static class PilloFrameworkDelegate {
    /// <summary>
    /// Delegate should be invoked when the Central has been initialized.
    /// </summary>
    public delegate void OnCentralDidInitialize ();

    /// <summary>
    /// Delegate should be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    public delegate void OnCentralDidFailToInitialize (string message);

    /// <summary>
    /// Delegate should be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    public delegate void OnPeripheralDidConnect (string identifier);

    /// <summary>
    /// Delegate should be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    public delegate void OnPeripheralDidDisconnect (string identifier);

    /// <summary>
    /// Delegate should be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    public delegate void OnPeripheralDidFailToConnect (string identifier);

    /// <summary>
    /// Delegate should be invoked when the Peripheral's battery level did 
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The battery level of the Peripheral.</param>
    public delegate void OnPeripheralBatteryLevelDidChange (string identifier, int batteryLevel);

    /// <summary>
    /// Delegate should be invoked when the Peripheral's pressure did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel"> The pressure of the Peripheral.</param>
    public delegate void OnPeripheralPressureDidChange (string identifier, int pressure);

    /// <summary>
    /// Delegate should be invoked when the Peripheral's charge state did 
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargeState">The charge state of the Peripheral.</param>
    public delegate void OnPeripheralChargeStateDidChange (string identifier, PeripheralChargeState chargeState);
  }
}