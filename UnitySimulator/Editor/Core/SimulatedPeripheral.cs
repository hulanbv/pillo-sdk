using Hulan.PilloSDK.DeviceManager;

namespace Hulan.PilloSDK.Simulator.Core {
  /// <summary>
  /// Simulated Pillo, used to test the Pillo Device Manager in the Unity Editor
  /// without the need of a physical Pillo device.
  /// </summary>
  class SimulatedPeripheral {
    /// <summary>
    /// The next identifier to use for a simulated Pillo.
    /// </summary>
    static int nextIdentifier = 0;

    /// <summary>
    /// The identifier of the simulated Pillo.
    /// </summary>
    internal readonly string identifier;

    /// <summary>
    /// The firmware version of the simulated Pillo.
    /// </summary>
    internal readonly PublishedValue<string> firmwareVersion;

    /// <summary>
    /// The hardware version of the simulated Pillo.
    /// </summary>
    internal readonly PublishedValue<string> hardwareVersion;

    /// <summary>
    /// The charging state of the simulated Pillo.
    /// </summary>
    internal readonly PublishedValue<ChargingState> chargingState;

    /// <summary>
    /// The battery level of the simulated Pillo.
    /// </summary>
    internal readonly PublishedValue<int> batteryLevel;

    /// <summary>
    /// The model number of the simulated Pillo.
    /// </summary>
    internal readonly PublishedValue<string> modelNumber;

    /// <summary>
    /// The pressure of the simulated Pillo.
    /// </summary>
    internal readonly PublishedValue<int> pressure;

    /// <summary>
    /// Creates a new simulated Pillo.
    /// </summary>
    internal SimulatedPeripheral() {
      // Invoke the peripheral did connect event.
      PilloDeviceManager.onPeripheralDidConnect?.Invoke(identifier = $"SimulatedPeripheral_{nextIdentifier++}");
      // Create the published values.
      firmwareVersion = new("0.0.0", value => PilloDeviceManager.onPeripheralFirmwareVersionDidChange?.Invoke(identifier, value));
      hardwareVersion = new("0.0.0", value => PilloDeviceManager.onPeripheralHardwareVersionDidChange?.Invoke(identifier, value));
      chargingState = new(ChargingState.SLEEP_MODE, value => PilloDeviceManager.onPeripheralChargingStateDidChange?.Invoke(identifier, value));
      batteryLevel = new(100, value => PilloDeviceManager.onPeripheralBatteryLevelDidChange?.Invoke(identifier, value));
      modelNumber = new("Simulated Pillo", value => PilloDeviceManager.onPeripheralModelNumberDidChange?.Invoke(identifier, value));
      pressure = new(0, value => PilloDeviceManager.onPeripheralPressureDidChange?.Invoke(identifier, value));
    }

    /// <summary>
    /// Disconnects the simulated Pillo.
    /// </summary>
    internal void Disconnect() {
      PilloDeviceManager.onPeripheralDidDisconnect?.Invoke(identifier);
    }
  }
}