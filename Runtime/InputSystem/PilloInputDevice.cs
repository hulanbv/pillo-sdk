using Hulan.PilloSDK.InputSystem.Core;

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
    /// The Pillo Input Device's model number.
    /// TODO -- Implement this!
    /// </summary>
    public string modelNumber { internal set; get; } = "";

    /// <summary>
    /// The Pillo Input Device's firmware version.
    /// TODO -- Implement this!
    /// </summary>
    public string firmwareVersion { internal set; get; } = "";

    /// <summary>
    /// The Pillo Input Device's hardware version.
    /// TODO -- Implement this!
    /// </summary>
    public string hardwareVersion { internal set; get; } = "";

    /// <summary>
    /// Powers of the Pillo Input Device.
    /// </summary>
    public void PowerOff () {
      // TODO -- Implement this!
    }

    /// <summary>
    /// Sets the maximum pressure value of the Pillo Input Device.
    /// </summary>
    /// <param name="maxPressureValue">The maximum pressure value.</param>
    public void SetMaximumPressure (int maximumPressureValue) {
      // TODO -- Implement this!
    }
  }
}