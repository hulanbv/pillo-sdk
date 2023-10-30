namespace Hulan.PilloSDK.DeviceManager {
  /// <summary>
  /// The Charging State of a Peripheral.
  /// </summary>
  public enum ChargingState {
    /// <summary>
    /// The Peripheral's charging state is unkown.
    /// </summary>
    UNKNOWN = -1,

    /// <summary>
    /// The Peripheral is prepairing for charge.
    /// </summary>
    PRE_CHARGE = 0,

    /// <summary>
    /// The Peripheral is fast charging.
    /// </summary>
    FAST_CHARGE = 1,

    /// <summary>
    /// The Peripheral is done or tickle charging.
    /// </summary>
    CHARGE_DONE = 2,

    /// <summary>
    /// The Peripheral is sleeping or not charging.
    /// </summary>
    SLEEP_MODE = 3,
  }
}