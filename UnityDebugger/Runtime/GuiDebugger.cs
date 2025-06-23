using System.Collections.Generic;
using Hulan.PilloSDK.Debugger.Core;
using Hulan.PilloSDK.DeviceManager;
using UnityEngine;

namespace Hulan.PilloSDK.Debugger {
  /// <summary>
  /// The Pillo Test MonoBehaviour will be show the Pillo events in Unity GUI.
  /// </summary>
  [AddComponentMenu("Pillo SDK/Debugger/GUI Debugger")]
  class GuiDebugger : MonoBehaviour {
    /// <summary>
    /// The font used to display the Pillo Device Manager events in the Unity GUI.
    /// </summary>
    public Font font;

    /// <summary>
    /// The virtual cursor texture.
    /// </summary>
    public Texture virtualCursorTexture;

    /// <summary>
    /// The virtual cursor position.
    /// </summary>
    Vector2 virtualCursorPosition;

    /// <summary>
    /// Determines if the virtual cursor click was requested.
    /// </summary>
    bool didRequestVirtualCursorClick;

    /// <summary>
    /// The list of Dummy Peripherals.
    /// </summary>
    readonly List<DummyPeripheral> peripherals = new();

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
    /// Binds the Pillo Device Manager events.
    /// </summary>
    void Awake() {
      PilloDeviceManager.onCentralDidInitialize += OnCentralDidInitialize;
      PilloDeviceManager.onCentralDidFailToInitialize += OnCentralDidFailToInitialize;
      PilloDeviceManager.onCentralDidStartScanning += OnCentralDidStartScanning;
      PilloDeviceManager.onCentralDidStopScanning += OnCentralDidStopScanning;
      PilloDeviceManager.onPeripheralDidConnect += OnPeripheralDidConnect;
      PilloDeviceManager.onPeripheralDidDisconnect += OnPeripheralDidDisconnect;
      PilloDeviceManager.onPeripheralDidFailToConnect += OnPeripheralDidFailToConnect;
      PilloDeviceManager.onPeripheralBatteryLevelDidChange += OnPeripheralBatteryLevelDidChange;
      PilloDeviceManager.onPeripheralPressureDidChange += OnPeripheralPressureDidChange;
      PilloDeviceManager.onPeripheralChargingStateDidChange += OnPeripheralChargingStateDidChange;
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange += OnPeripheralFirmwareVersionDidChange;
      PilloDeviceManager.onPeripheralHardwareVersionDidChange += OnPeripheralHardwareVersionDidChange;
      PilloDeviceManager.onPeripheralModelNumberDidChange += OnPeripheralModelNumberDidChange;
    }

    /// <summary>
    /// Unbinds the Pillo Device Manager events.
    /// </summary>
    void Destroy() {
      PilloDeviceManager.onCentralDidInitialize -= OnCentralDidInitialize;
      PilloDeviceManager.onCentralDidFailToInitialize -= OnCentralDidFailToInitialize;
      PilloDeviceManager.onCentralDidStartScanning -= OnCentralDidStartScanning;
      PilloDeviceManager.onCentralDidStopScanning -= OnCentralDidStopScanning;
      PilloDeviceManager.onPeripheralDidConnect -= OnPeripheralDidConnect;
      PilloDeviceManager.onPeripheralDidDisconnect -= OnPeripheralDidDisconnect;
      PilloDeviceManager.onPeripheralDidFailToConnect -= OnPeripheralDidFailToConnect;
      PilloDeviceManager.onPeripheralBatteryLevelDidChange -= OnPeripheralBatteryLevelDidChange;
      PilloDeviceManager.onPeripheralPressureDidChange -= OnPeripheralPressureDidChange;
      PilloDeviceManager.onPeripheralChargingStateDidChange -= OnPeripheralChargingStateDidChange;
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange -= OnPeripheralFirmwareVersionDidChange;
      PilloDeviceManager.onPeripheralHardwareVersionDidChange -= OnPeripheralHardwareVersionDidChange;
      PilloDeviceManager.onPeripheralModelNumberDidChange -= OnPeripheralModelNumberDidChange;
    }

    /// <summary>
    /// Draws the Pillo Device Manager events in the Unity GUI.
    /// </summary>
    void OnGUI() {
      GUI.skin.label.font = GUI.skin.button.font = GUI.skin.box.font = font;
      GUILayout.BeginHorizontal();
      GUILayout.BeginVertical();
      GUILayout.Label($"Central Initialized: {isCentralInitialized}");
      GUILayout.Label($"Central Failed To Initialize: {didCentralFailToInitialize}");
      GUILayout.Label($"Central Scanning: {isCentralScanning}");
      GUILayout.Space(10);
      GUILayout.EndVertical();
      foreach (var peripheral in peripherals) {
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        GUILayout.Label($"Identifier: {peripheral.identifier}");
        GUILayout.Label($"Battery Level: {peripheral.batteryLevel}");
        GUILayout.Label($"Pressure: {peripheral.pressure}");
        GUILayout.Label($"Charging State: {peripheral.chargingState}");
        GUILayout.Label($"Firmware Version: {peripheral.firmwareVersion}");
        GUILayout.Label($"Hardware Version: {peripheral.hardwareVersion}");
        GUILayout.Label($"Model Number: {peripheral.modelNumber}");
        if (Button("Disable Force LED Off")) {
          PilloDeviceManager.ForcePeripheralLedOff(peripheral.identifier, false);
        }
        if (Button("Enable Force LED Off")) {
          PilloDeviceManager.ForcePeripheralLedOff(peripheral.identifier, true);
        }
        if (Button("Start Calibration")) {
          PilloDeviceManager.StartPeripheralCalibration(peripheral.identifier);
        }
        if (Button("Cancel Connection")) {
          PilloDeviceManager.CancelPeripheralConnection(peripheral.identifier);
        }
        if (Button("Power Off")) {
          PilloDeviceManager.PowerOffPeripheral(peripheral.identifier);
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndHorizontal();
      GUI.Label(new Rect(virtualCursorPosition.x, virtualCursorPosition.y, 25, 25), virtualCursorTexture);
    }

    /// <summary>
    /// Updates the virtual cursor position.
    /// </summary>
    void Update() {
      if (Input.touchCount <= 0) {
        return;
      }
      didRequestVirtualCursorClick = Input.GetKey(KeyCode.JoystickButton14);
      if (Input.GetKey(KeyCode.JoystickButton14)) {
        return;
      }
      var deltaPosition = Input.GetTouch(0).deltaPosition;
      virtualCursorPosition += new Vector2(deltaPosition.x, -deltaPosition.y) / 4;
    }

    /// <summary>
    /// Draws a button in the Unity GUI.
    /// </summary>
    /// <param name="text">The text of the button.</param>
    /// <returns>True if the button was clicked.</returns>
    bool Button(string text) {
      var button = GUILayout.Button($"[ {text} ]", GUI.skin.label);
      if (button) {
        return true;
      }
      var buttonRect = GUILayoutUtility.GetLastRect();
      if (didRequestVirtualCursorClick && buttonRect.Contains(virtualCursorPosition)) {
        didRequestVirtualCursorClick = false;
        return true;
      }
      return false;
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
      peripherals.Add(new DummyPeripheral() {
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
