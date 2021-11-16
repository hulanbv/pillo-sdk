using Hulan.PilloSDK.InputSystem;

// Unity Engine Pillo SDK Input System Core
// Author: Jeffrey Lanters at Hulan

namespace Hulan.PilloSDK.InputSystem.Core {

  /// <summary>
  /// Containing the delegate definitions for the Pillo Input.
  /// </summary>
  public static class PilloInputDelegate {

    /// <summary>
    /// Delegate invoked when the Framework has been initialized.
    /// </summary>
    public delegate void OnCentralDidInitialize ();

    /// <summary>
    /// Delegate invoked when the Framework has failed to initialize.
    /// </summary>
    /// <param name="reason">The reason why it failed.</param>
    public delegate void OnCentralDidFailToInitialize (string reason);

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has been connected.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    public delegate void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice);

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has been disconnected.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    public delegate void OnPilloInputDeviceDidDisconnect (PilloInputDevice pilloInputDevice);

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has failed to connect.
    /// </summary>
    public delegate void OnPilloInputDeviceDidFailToConnect ();

    /// <summary>
    /// Delegate invoked when the Pillo Input Device's state did change.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    public delegate void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice);
  }
}