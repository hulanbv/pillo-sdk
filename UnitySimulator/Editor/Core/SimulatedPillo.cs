using Hulan.PilloSDK.Framework;
using Hulan.PilloSDK.Framework.Payloads;

// Unity Engine Pillo SDK Simulator
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Simulator.Core {
  /// <summary>
  /// In order to simulate, we need to keep track of simulated Pillos.
  /// </summary>
  class SimulatedPillo {
    /// <summary>
    /// The Pillo's identifier.
    /// </summary>
    internal readonly string Identifier;

    internal readonly PublishedValue<string> firmwareVersion;
    internal readonly PublishedValue<string> hardwareVersion;
    internal readonly PublishedValue<bool> isConnected;
    internal readonly PublishedValue<PeripheralChargeState> chargeState;
    internal readonly PublishedValue<int> batteryLevel;
    internal readonly PublishedValue<string> modelNumber;
    internal readonly PublishedValue<int> pressure;

    /// <summary>
    /// Instanciates a new simulated peripheral.
    /// </summary>
    /// <param name="identifier">The peripheral's identifier.</param>
    internal SimulatedPillo(string identifier) {
      Identifier = identifier;
      firmwareVersion = new(string.Empty, HandleFirwareVersionDidChange);
      hardwareVersion = new(string.Empty, HandleHardwareVersionDidChange);
      isConnected = new(HandleIsConnectedDidChange);
      chargeState = new(HandleChargeStateDidChange);
      batteryLevel = new(HandleBatteryLevelDidChange);
      modelNumber = new(string.Empty, HandleModelNumberDidChange);
      pressure = new(HandlePressureDidChange);
    }

    void HandleFirwareVersionDidChange(string firmwareVersion) {
      var payload = new PeripheralFirmwareVersionDidChangePayload() {
        identifier = Identifier,
        firmwareVersion = firmwareVersion
      };
      DeviceSimulator.InvokeCallback("OnPeripheralFirmwareVersionDidChange", payload);
    }

    void HandleHardwareVersionDidChange(string hardwareVersion) {
      var payload = new PeripheralHardwareVersionDidChangePayload() {
        identifier = Identifier,
        hardwareVersion = hardwareVersion
      };
      DeviceSimulator.InvokeCallback("OnPeripheralHardwareVersionDidChange", payload);
    }

    void HandleIsConnectedDidChange(bool isConnected) {
      if (isConnected) {
        var payload = new PeripheralDidConnectPayload() {
          identifier = Identifier,
        };
        DeviceSimulator.InvokeCallback("OnPeripheralDidConnect", payload);
      }
      else {
        var payload = new PeripheralDidDisconnectPayload() {
          identifier = Identifier,
        };
        DeviceSimulator.InvokeCallback("OnPeripheralDidDisconnect", payload);
      }
    }

    void HandleChargeStateDidChange(PeripheralChargeState chargeState) {
      var payload = new PeripheralChargeStateDidChangePayload() {
        identifier = Identifier,
        chargeState = chargeState
      };
      DeviceSimulator.InvokeCallback("OnPeripheralChargeStateDidChange", payload);
    }

    void HandleBatteryLevelDidChange(int batteryLevel) {
      var payload = new PeripheralBatteryLevelDidChangePayload() {
        identifier = Identifier,
        batteryLevel = batteryLevel
      };
      DeviceSimulator.InvokeCallback("OnPeripheralBatteryLevelDidChange", payload);
    }

    void HandleModelNumberDidChange(string modelNumber) {
      var payload = new PeripheralModelNumberDidChangePayload() {
        identifier = Identifier,
        modelNumber = modelNumber
      };
      DeviceSimulator.InvokeCallback("OnPeripheralModelNumberDidChange", payload);
    }

    void HandlePressureDidChange(int pressure) {
      var payload = new PeripheralPressureDidChangePayload() {
        identifier = Identifier,
        pressure = pressure
      };
      DeviceSimulator.InvokeCallback("OnPeripheralPressureDidChange", payload);
    }
  }
}