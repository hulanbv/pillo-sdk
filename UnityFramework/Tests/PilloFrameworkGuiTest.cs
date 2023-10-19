using System.Collections.Generic;
using UnityEngine;

namespace Hulan.PilloSDK.Framework.Tests {
  /// <summary>
  /// The Pillo Framework Test MonoBehaviour will be show the Pillo Framework
  /// events in the Unity GUI.
  /// </summary>
  [AddComponentMenu("Hulan/Pillo SDK/Framework/Tests/Pillo Framework GUI Test")]
  class PilloFrameworkGuiTest : MonoBehaviour {
    /// <summary>
    /// The Virtual Peripheral class is used to store the Peripheral's data.
    /// </summary>
    class VirtualPeripheral {
      /// <summary>
      /// The identifier of the Peripheral.
      /// </summary>
      internal string identifier;

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

    /// <summary>
    /// The list of Virtual Peripherals.
    /// </summary>
    readonly List<VirtualPeripheral> peripherals = new();

    /// <summary>
    /// Determines if the Central has been initialized.
    /// </summary>
    bool isCentralInitialized;

    /// <summary>
    /// Determines if the Central has failed to initialize.
    /// </summary>
    bool didCentralFailToInitialize;

    /// <summary>
    /// Determines if the Central is scanning.
    /// </summary>
    bool isCentralScanning;

    /// <summary>
    /// Binds the Pillo Framework events to the Pillo Framework Test MonoBe-
    /// haviour.
    /// </summary>
    void Start() {
      PilloFramework.onCentralDidInitialize += OnCentralDidInitialize;
      PilloFramework.onCentralDidFailToInitialize += OnCentralDidFailToInitialize;
      PilloFramework.onCentralDidStartScanning += OnCentralDidStartScanning;
      PilloFramework.onCentralDidStopScanning += OnCentralDidStopScanning;
      PilloFramework.onPeripheralDidConnect += OnPeripheralDidConnect;
      PilloFramework.onPeripheralDidDisconnect += OnPeripheralDidDisconnect;
      PilloFramework.onPeripheralDidFailToConnect += OnPeripheralDidFailToConnect;
      PilloFramework.onPeripheralBatteryLevelDidChange += OnPeripheralBatteryLevelDidChange;
      PilloFramework.onPeripheralPressureDidChange += OnPeripheralPressureDidChange;
      PilloFramework.onPeripheralChargingStateDidChange += OnPeripheralChargingStateDidChange;
      PilloFramework.onPeripheralFirmwareVersionDidChange += OnPeripheralFirmwareVersionDidChange;
      PilloFramework.onPeripheralHardwareVersionDidChange += OnPeripheralHardwareVersionDidChange;
      PilloFramework.onPeripheralModelNumberDidChange += OnPeripheralModelNumberDidChange;
#if UNITY_EDITOR
      peripherals.Add(new VirtualPeripheral());
#endif
    }

    /// <summary>
    /// Draws the Pillo Framework events in the Unity GUI.
    /// </summary>
    void OnGUI() {
      GUILayout.BeginVertical();
      GUILayout.Label($"Central Initialized: {isCentralInitialized}");
      GUILayout.Label($"Central Failed To Initialize: {didCentralFailToInitialize}");
      GUILayout.Label($"Central Scanning: {isCentralScanning}");
      GUILayout.Label($"Peripherals: {peripherals.Count}");
      foreach (var peripheral in peripherals) {
        GUILayout.Label($"  Peripheral: {peripheral.identifier}");
        GUILayout.Label($"  Battery Level: {peripheral.batteryLevel}");
        GUILayout.Label($"  Pressure: {peripheral.pressure}");
        GUILayout.Label($"  Charging State: {peripheral.chargingState}");
        GUILayout.Label($"  Firmware Version: {peripheral.firmwareVersion}");
        GUILayout.Label($"  Hardware Version: {peripheral.hardwareVersion}");
        GUILayout.Label($"  Model Number: {peripheral.modelNumber}");
        if (GUILayout.Button("Disable Force Peripheral LED Off")) {
          PilloFramework.ForcePeripheralLedOff(peripheral.identifier, false);
        }
        if (GUILayout.Button("Enable Force Peripheral LED Off")) {
          PilloFramework.ForcePeripheralLedOff(peripheral.identifier, true);
        }
        if (GUILayout.Button("Start Peripheral Calibration")) {
          PilloFramework.StartPeripheralCalibration(peripheral.identifier);
        }
        if (GUILayout.Button("Cancel Peripheral Connection")) {
          PilloFramework.CancelPeripheralConnection(peripheral.identifier);
        }
        if (GUILayout.Button("Power Off Peripheral")) {
          PilloFramework.PowerOffPeripheral(peripheral.identifier);
        }
      }
      GUILayout.EndVertical();
    }

    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    void OnCentralDidInitialize() {
      isCentralInitialized = true;
    }

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    void OnCentralDidFailToInitialize(string message) {
      didCentralFailToInitialize = true;
    }

    /// <summary>
    /// Delegate will be invoked when the Central has started scanning.
    /// </summary>
    void OnCentralDidStartScanning() {
      isCentralScanning = true;
    }

    /// <summary>
    /// Delegate will be invoked when the Central has stopped scanning.
    /// </summary>
    void OnCentralDidStopScanning() {
      isCentralScanning = false;
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidConnect(string identifier) {
      peripherals.Add(new VirtualPeripheral() {
        identifier = identifier,
      });
    }

    /// <summary>
    /// Delegate should be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidDisconnect(string identifier) {
      peripherals.RemoveAll(peripheral => peripheral.identifier == identifier);
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidFailToConnect(string identifier) {
      peripherals.RemoveAll(peripheral => peripheral.identifier == identifier);
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The battery level of the Peripheral.</param>
    void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) {
      var peripheral = peripherals.Find(peripheral => peripheral.identifier == identifier);
      if (peripheral != null) {
        peripheral.batteryLevel = batteryLevel;
      }
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel"> The pressure of the Peripheral.</param>
    void OnPeripheralPressureDidChange(string identifier, int pressure) {
      var peripheral = peripherals.Find(peripheral => peripheral.identifier == identifier);
      if (peripheral != null) {
        peripheral.pressure = pressure;
      }
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargingState">The charge state of the Peripheral.</param>
    void OnPeripheralChargingStateDidChange(string identifier, ChargingState chargingState) {
      var peripheral = peripherals.Find(peripheral => peripheral.identifier == identifier);
      if (peripheral != null) {
        peripheral.chargingState = chargingState;
      }
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's model number did change.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="modelNumber"></param>
    void OnPeripheralModelNumberDidChange(string identifier, string modelNumber) {
      var peripheral = peripherals.Find(peripheral => peripheral.identifier == identifier);
      if (peripheral != null) {
        peripheral.modelNumber = modelNumber;
      }
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's hardware version did 
    /// change.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="hardwareVersion"></param>
    void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) {
      var peripheral = peripherals.Find(peripheral => peripheral.identifier == identifier);
      if (peripheral != null) {
        peripheral.hardwareVersion = hardwareVersion;
      }
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's firmware version did
    /// change.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="firmwareVersion"></param>
    void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) {
      var peripheral = peripherals.Find(peripheral => peripheral.identifier == identifier);
      if (peripheral != null) {
        peripheral.firmwareVersion = firmwareVersion;
      }
    }
  }
}
