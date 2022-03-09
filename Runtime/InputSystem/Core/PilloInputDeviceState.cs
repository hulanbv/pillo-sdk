// Unity Engine Pillo SDK Input System Core
// Author: Jeffrey Lanters at Hulan

namespace Hulan.PilloSDK.InputSystem.Core {

  /// <summary>
  /// The State of a Pillo Input Device.
  /// </summary>
  public abstract class PilloInputDeviceState {

    /// <summary>
    /// The Pillo Input Device's unique identifier assigned by the Pillo
    /// Bluetooth peripheral.
    /// </summary>
    public string identifier { internal set; get; } = "";

    /// <summary>
    /// Internally assigned player indexes. These indexes are assigned
    /// automatically and are used to identify the player that the device
    /// belongs to. If a device disconnects, the player index is reassigned.
    /// </summary>  
    public int playerIndex { internal set; get; } = -1;

    /// <summary>
    /// The Pillo Input Device's pressure level.
    /// </summary>
    public int pressure { internal set; get; } = 0;

    /// <summary>
    /// The Pillo Input Device's battery level.
    /// </summary>
    public int batteryLevel { internal set; get; } = 0;

    /// <summary>
    /// The Pillo Input Device's charging state.
    /// TODO -- Implement this!
    /// </summary>
    public PilloInputDeviceChargingState chargingState { internal set; get; } = PilloInputDeviceChargingState.UNKNOWN;
  }
}