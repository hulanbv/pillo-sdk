using Hulan.Pillo.SDK.InputSystem;

// Pillo SDK for Unity
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK.Core {

  /// <summary>
  /// Containing the delegate definitions for the Pillo Input.
  /// </summary>
  public static class PilloInputDelegate {

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
    /// Delegate invoked when a device has been connected.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    public delegate void OnDeviceDidConnect (PilloInputDevice pilloInputDevice);

    /// <summary>
    /// Delegate invoked when a device has been disconnected.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    public delegate void OnDeviceDidDisconnect (PilloInputDevice pilloInputDevice);

    /// <summary>
    /// Delegate invoked when a device has failed to connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo.</param>
    public delegate void OnDeviceDidFailToConnect (string identifier);

    /// <summary>
    /// Delegate invoked when the Pillo's state did change.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    public delegate void OnDeviceStateDidChange (PilloInputDevice pilloInputDevice);
  }
}