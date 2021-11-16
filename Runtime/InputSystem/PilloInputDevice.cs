using Hulan.PilloSDK.Core;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan

namespace Hulan.PilloSDK.InputSystem {

  /// <summary>
  /// The Pillo input device represents a fysical Pillo peripheral.
  /// </summary>
  public class PilloInputDevice : PilloInputDeviceState {

    /// <summary>
    /// Initializes a new instance of a Pillo Input Device.
    /// </summary>
    /// <param name="identifier">The bluetooth peripheral identifier.</param>
    public PilloInputDevice (string identifier) {
      this.identifier = identifier;
    }

    /// <summary>
    /// Powers of the Pillo Input Device.
    /// </summary>
    public void PowerOff () {
      // TODO Implement this!
    }

    /// <summary>
    /// Sets the maximum pressure value of the Pillo Input Device.
    /// </summary>
    /// <param name="maxPressureValue">The maximum pressure value.</param>
    public void SetMaximumPressure (int maximumPressureValue) {
      // TODO Implement this!
    }
  }
}