using Hulan.PilloSDK.Framework;

namespace Hulan.PilloSDK.Simulator.Core {
  /// <summary>
  /// In order to simulate, we need to keep track of simulated Pillos.
  /// </summary>
  class SimulatedPillo {
    static int nextIdentifier = 0;
    internal readonly string identifier;
    internal readonly PublishedValue<string> firmwareVersion;
    internal readonly PublishedValue<string> hardwareVersion;
    internal readonly PublishedValue<ChargingState> chargingState;
    internal readonly PublishedValue<int> batteryLevel;
    internal readonly PublishedValue<string> modelNumber;
    internal readonly PublishedValue<int> pressure;

    internal SimulatedPillo() {
      identifier = $"SimulatedPillo{nextIdentifier++}";
      PilloFramework.onPeripheralDidConnect?.Invoke(identifier);
      firmwareVersion = new("0.0.0", value => PilloFramework.onPeripheralFirmwareVersionDidChange?.Invoke(identifier, value));
      hardwareVersion = new("0.0.0", value => PilloFramework.onPeripheralHardwareVersionDidChange?.Invoke(identifier, value));
      chargingState = new(ChargingState.SLEEP_MODE, value => PilloFramework.onPeripheralChargingStateDidChange?.Invoke(identifier, value));
      batteryLevel = new(100, value => PilloFramework.onPeripheralBatteryLevelDidChange?.Invoke(identifier, value));
      modelNumber = new("Simulated Pillo", value => PilloFramework.onPeripheralModelNumberDidChange?.Invoke(identifier, value));
      pressure = new(0, value => PilloFramework.onPeripheralPressureDidChange?.Invoke(identifier, value));
    }

    internal void Disconnect() {
      PilloFramework.onPeripheralDidDisconnect?.Invoke(identifier);
    }
  }
}