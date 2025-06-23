using Hulan.PilloSDK.DeviceManager;

namespace Hulan.PilloSDK.Debugger.Core {
  /// <summary>
  /// The Dummy Peripheral class is used to store the Peripheral's data.
  /// </summary>
  class DummyPeripheral {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    internal string identifier;

    /// <summary>
    /// Defines if the Peripheral is simulated or not.
    /// </summary>
    internal bool isSimulated;

    /// <summary>
    /// The battery level of the Peripheral.
    /// </summary>
    internal int batteryLevel;

    /// <summary>
    /// The pressure of the Peripheral.
    /// </summary>
    internal int pressure;

    /// <summary>
    /// The charging state of the Peripheral.
    /// </summary>
    internal ChargingState chargingState;

    /// <summary>
    /// The firmware version of the Peripheral.
    /// </summary>
    internal string firmwareVersion;

    /// <summary>
    /// The hardware version of the Peripheral.
    /// </summary>
    internal string hardwareVersion;

    /// <summary>
    /// The model number of the Peripheral.
    /// </summary>
    internal string modelNumber;
  }
}