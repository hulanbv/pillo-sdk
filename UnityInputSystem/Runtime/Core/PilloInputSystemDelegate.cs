// Unity Engine Pillo SDK Input System Core
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem.Core {
  /// <summary>
  /// Containing the delegate definitions for the Pillo Input.
  /// </summary>
  public static class PilloInputSystemDelegate {
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