// Pillo Framework Unity SDK
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK.Core {

  /// <summary>
  /// Containing the delegate definitions for the Pillo SDK.
  /// </summary>
  public static class PilloInputDelegateDefinitions {

    /// <summary>
    /// Delegate invoked when the Framework has been initialized.
    /// </summary>
    public delegate void OnDidInitialize ();

    /// <summary>
    /// Delegate invoked when the Framework has failed to initialize.
    /// </summary>
    /// <param name="reason">The reason why it failed.</param>
    public delegate void OnDidFailToInitialize (string reason);

    /// <summary>
    /// Delegate invoked when a Pillo has been connected.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo.</param>
    public delegate void OnDidConnect (string identifier);

    /// <summary>
    /// Delegate invoked when a Pillo has been disconnected.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo.</param>
    public delegate void OnDidDisconnect (string identifier);

    /// <summary>
    /// Delegate invoked when a Pillo has failed to connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo.</param>
    public delegate void OnDidFailToConnect (string identifier);

    /// <summary>
    /// Delegate invoked when the battery level has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo.</param>
    /// <param name="batteryLevel">A number from 0 to 100 reflecting the battery level.</param>
    public delegate void OnBatteryLevelDidChange (string identifier, int batteryLevel);

    /// <summary>
    /// Delegate invoked when the Pillo  Peripherals's pressure has ben changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo.</param>
    /// <param name="pressure">A number from 0 to 255 pressure the pressure level.</param>
    public delegate void OnPressureDidChange (string identifier, int pressure);
  }
}