using Hulan.PilloSDK.InputSystem.Core;
using Hulan.PilloSDK.Framework;

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
    public PilloInputDevice(string identifier) {
      this.identifier = identifier;
    }

    /// <summary>
    /// Powers of the Pillo Input Device.
    /// </summary>
    public void PowerOff() {
      PilloFramework.PowerOffPeripheral(identifier);
    }

    /// <summary>
    /// Forces the LED of the Pillo Input Device off.
    /// </summary>
    /// <param name="enabled">Defines whether the LED should be forced off.</param>
    public void ForceLedOff(bool enabled) {
      PilloFramework.ForceLedOff(identifier, enabled);
    }

    /// <summary>
    /// Starts the calibration of the Pillo Input Device.
    /// </summary>
    public void StartCalibration() {
      PilloFramework.StartPeripheralCalibration(identifier);
    }
  }
}