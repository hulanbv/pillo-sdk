namespace Hulan.Pillo.SDK.InputSystem {

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
  }
}