// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem {
  /// <summary>
  /// The Charging State of a Pillo Input Device.
  /// </summary>
  public enum PilloInputDeviceChargeState {
    /// <summary>
    /// The Pillo Input Device's charging state is unkown.
    /// </summary>
    UNKNOWN = -1,

    /// <summary>
    /// The Pillo Input Device is prepairing for charge.
    /// </summary>
    PRE_CHARGE = 0,

    /// <summary>
    /// The Pillo Input Device is fast charging.
    /// </summary>
    FAST_CHARGE = 1,

    /// <summary>
    /// The Pillo Input Device is done or tickle charging.
    /// </summary>
    CHARGE_DONE = 2,

    /// <summary>
    /// The Pillo Input Device is sleeping or not charging.
    /// </summary>
    SLEEP_MODE = 3,
  }
}