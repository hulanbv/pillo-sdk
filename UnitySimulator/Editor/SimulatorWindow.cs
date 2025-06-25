using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Hulan.PilloSDK.DeviceManager;
using Hulan.PilloSDK.Simulator.Core;
using System;

namespace Hulan.PilloSDK.Simulator {
  /// <summary>
  /// Pillo Simulator can be used to test the Pillo Device Manager in the 
  /// Unity Editor without the need of a physical Pillo device.
  /// </summary>
  class SimulatorWindow : EditorWindow, IHasCustomMenu {
    /// <summary>
    /// Key used to store the Pillo Simulator settings.
    /// </summary>
    const string settingsBaseKey = "Hulan.PilloSDK.Simulator";

    /// <summary>
    /// Key used to store the Pillo Simulator auto connect setting.
    /// </summary>
    const string autoConnectSettingKey = settingsBaseKey + ".AutoConnect";

    /// <summary>
    /// Defines if the Pillo Simulator should automatically connect to
    /// peripherals when entering Play Mode.
    /// </summary>
    static bool autoConnect;

    /// <summary>
    /// The list of physical peripherals.
    /// </summary>
    readonly List<DummyPeripheral> peripherals = new();

    /// <summary>
    /// The scroll view position.
    /// </summary>
    Vector2 scrollViewPosition;

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
    /// Shows the Pillo Simulator window.
    /// </summary>
    [MenuItem("Window/Pillo/Simulator", priority = 500)]
    static void ShowWindow() {
      GetWindow<SimulatorWindow>();
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is enabled.
    /// </summary>
    void OnEnable() {
      var icon = EditorGUIUtility.IconContent("d_PreMatCube");
      titleContent = new GUIContent("Pillo Simulator", icon.image);
      autoConnect = EditorPrefs.GetBool(autoConnectSettingKey, false);
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
      if (autoConnect) {
        for (int i = 0; i < 2; i++) {
          AddSimulatedPeripheral();
        }
      }
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is disabled.
    /// </summary>
    void OnDisable() {
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
      isCentralInitialized = false;
      didCentralFailToInitialize = false;
      isCentralScanning = false;
      peripherals.Clear();
    }

    /// <summary>
    /// Returns Generic Menu items for the Pillo Simulator window.
    /// </summary>
    /// <param name="menu">The Generic Menu to add items to.</param>
    void IHasCustomMenu.AddItemsToMenu(GenericMenu menu) {
      menu.AddItem(new GUIContent("Auto Connect on Play"), autoConnect, () => {
        EditorPrefs.SetBool(autoConnectSettingKey, autoConnect = !autoConnect);
      });
    }

    /// <summary>
    /// Adds a simulated peripheral.
    /// </summary>
    void AddSimulatedPeripheral() {
      var identifier = $"SIMULATED_${Guid.NewGuid()}";
      PilloDeviceManager.onPeripheralDidConnect(identifier);
      PilloDeviceManager.onPeripheralBatteryLevelDidChange(identifier, UnityEngine.Random.Range(25, 100));
      PilloDeviceManager.onPeripheralChargingStateDidChange(identifier, ChargingState.SLEEP_MODE);
      PilloDeviceManager.onPeripheralFirmwareVersionDidChange(identifier, "0.0.1");
      PilloDeviceManager.onPeripheralHardwareVersionDidChange(identifier, "0.0.1");
      PilloDeviceManager.onPeripheralModelNumberDidChange(identifier, "SIMPIL02");
      PilloDeviceManager.onPeripheralPressureDidChange(identifier, 0);
    }

    /// <summary>
    /// Draws the Pillo Simulator window.
    /// </summary>
    void OnGUI() {
      // Draw the Pillo Simulator toolbar.
      GUILayout.BeginHorizontal(EditorStyles.toolbar);
      EditorGUI.BeginDisabledGroup(Application.isPlaying == false);
      if (GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), EditorStyles.toolbarDropDown)) {
        var genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Simulated Peripheral"), false, AddSimulatedPeripheral);
        genericMenu.ShowAsContext();
      }
      EditorGUI.EndDisabledGroup();
      EditorGUI.BeginDisabledGroup(true);
      if (isCentralScanning) {
        GUILayout.Label("Scanning for Pillo devices...");
      }
      EditorGUI.EndDisabledGroup();
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      if (Application.isPlaying == false) {
        // Draw a help box to inform the user that the Pillo Simulator
        // can only be used in Play Mode.
        EditorGUILayout.HelpBox("The Pillo Simulator can only be used in Play Mode.", MessageType.Info);
        return;
      }
      if (isCentralInitialized == false) {
        // Draw a help box to inform the user that the Central is not initialized.
        EditorGUILayout.HelpBox("The Pillo Central is not initialized, please wait for the Central to initialize.", MessageType.Warning);
      }
      if (didCentralFailToInitialize) {
        // Draw a help box to inform the user that the Central has failed to initialize.
        EditorGUILayout.HelpBox("The Pillo Central failed to initialize, please check the console for more details.", MessageType.Error);
      }
      // Draw the Pillo Simulator content.
      scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition);
      GUILayout.BeginHorizontal();
      for (int i = 0; i < peripherals.Count; i++) {
        var peripheral = peripherals[i];
        // Draw the simulated peripheral.
        GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
        GUILayout.Label("Pillo Device", EditorStyles.largeLabel);
        GUILayout.Label(peripheral.isSimulated ? "Simulated" : "Physical", EditorStyles.miniLabel);
        GUILayout.Space(10);
        // Draw the simulated peripheral identifier and versions.
        GUILayout.Label("Identifier", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField(peripheral.identifier);
        EditorGUI.EndDisabledGroup();
        GUILayout.Label("Hardware Version", EditorStyles.boldLabel);
        if (peripheral.isSimulated) {
          var hardwareVersionBefore = peripheral.hardwareVersion;
          peripheral.hardwareVersion = EditorGUILayout.TextField(peripheral.hardwareVersion);
          if (peripheral.hardwareVersion != hardwareVersionBefore) {
            PilloDeviceManager.onPeripheralHardwareVersionDidChange(peripheral.identifier, peripheral.hardwareVersion);
          }
        }
        else {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.TextField(peripheral.hardwareVersion);
          EditorGUI.EndDisabledGroup();
        }
        GUILayout.Label("Firmware Version", EditorStyles.boldLabel);
        if (peripheral.isSimulated) {
          var firmwareVersionBefore = peripheral.firmwareVersion;
          peripheral.firmwareVersion = EditorGUILayout.TextField(peripheral.firmwareVersion);
          if (peripheral.firmwareVersion != firmwareVersionBefore) {
            PilloDeviceManager.onPeripheralFirmwareVersionDidChange(peripheral.identifier, peripheral.firmwareVersion);
          }
        }
        else {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.TextField(peripheral.firmwareVersion);
          EditorGUI.EndDisabledGroup();
        }
        GUILayout.Label("Model Number", EditorStyles.boldLabel);
        if (peripheral.isSimulated) {
          var modelNumberBefore = peripheral.modelNumber;
          peripheral.modelNumber = EditorGUILayout.TextField(peripheral.modelNumber);
          if (peripheral.modelNumber != modelNumberBefore) {
            PilloDeviceManager.onPeripheralModelNumberDidChange(peripheral.identifier, peripheral.modelNumber);
          }
        }
        else {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.TextField(peripheral.modelNumber);
          EditorGUI.EndDisabledGroup();
        }
        GUILayout.Space(10);
        // Draw the simulated peripheral battery services.
        GUILayout.Label("Battery State", EditorStyles.largeLabel);
        GUILayout.Label("Battery Level", EditorStyles.boldLabel);
        if (peripheral.isSimulated) {
          var batteryLevelBefore = peripheral.batteryLevel;
          peripheral.batteryLevel = EditorGUILayout.IntSlider(peripheral.batteryLevel, 0, 100);
          if (peripheral.batteryLevel != batteryLevelBefore) {
            PilloDeviceManager.onPeripheralBatteryLevelDidChange(peripheral.identifier, peripheral.batteryLevel);
          }
        }
        else {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.IntSlider(peripheral.batteryLevel, 0, 100);
          EditorGUI.EndDisabledGroup();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.Label("Charge State", EditorStyles.boldLabel);
        EditorGUI.EndDisabledGroup();
        if (peripheral.isSimulated) {
          var chargingStateBefore = peripheral.chargingState;
          peripheral.chargingState = (ChargingState)EditorGUILayout.EnumPopup(peripheral.chargingState);
          if (peripheral.chargingState != chargingStateBefore) {
            PilloDeviceManager.onPeripheralChargingStateDidChange(peripheral.identifier, peripheral.chargingState);
          }
        }
        else {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.EnumPopup(peripheral.chargingState);
          EditorGUI.EndDisabledGroup();
        }
        GUILayout.Space(10);
        // Draw the simulated peripheral pressure.
        GUILayout.Label("Pressure", EditorStyles.largeLabel);
        GUILayout.Label("Pressure Level", EditorStyles.boldLabel);
        if (peripheral.isSimulated) {
          var pressureBefore = peripheral.pressure;
          peripheral.pressure = EditorGUILayout.IntSlider(peripheral.pressure, 0, 1024);
          if (peripheral.pressure != pressureBefore) {
            PilloDeviceManager.onPeripheralPressureDidChange(peripheral.identifier, peripheral.pressure);
          }
        }
        else {
          EditorGUI.BeginDisabledGroup(true);
          EditorGUILayout.IntSlider(peripheral.pressure, 0, 1024);
          EditorGUI.EndDisabledGroup();
        }
        // Draw the simulated peripheral actions.
        GUILayout.FlexibleSpace();
        GUILayout.Label("Actions", EditorStyles.largeLabel);
        EditorGUI.BeginDisabledGroup(peripheral.isSimulated);
        if (GUILayout.Button("Disable Force LED Off")) {
          PilloDeviceManager.ForcePeripheralLedOff(peripheral.identifier, false);
        }
        if (GUILayout.Button("Enable Force LED Off")) {
          PilloDeviceManager.ForcePeripheralLedOff(peripheral.identifier, true);
        }
        if (GUILayout.Button("Start Calibration")) {
          PilloDeviceManager.StartPeripheralCalibration(peripheral.identifier);
        }
        if (GUILayout.Button("Cancel Connection")) {
          PilloDeviceManager.CancelPeripheralConnection(peripheral.identifier);
        }
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("Power Off")) {
          if (peripheral.isSimulated) {
            PilloDeviceManager.onPeripheralDidDisconnect(peripheral.identifier);
          }
          else {
            PilloDeviceManager.PowerOffPeripheral(peripheral.identifier);
          }
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndHorizontal();
      GUILayout.EndScrollView();
    }

    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    void OnCentralDidInitialize() {
      isCentralInitialized = true;
      Repaint();
    }

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    void OnCentralDidFailToInitialize(string message) {
      didCentralFailToInitialize = true;
      Repaint();
    }

    /// <summary>
    /// Delegate will be invoked when the Central has started scanning.
    /// </summary>
    void OnCentralDidStartScanning() {
      isCentralScanning = true;
      Repaint();
    }

    /// <summary>
    /// Delegate will be invoked when the Central has stopped scanning.
    /// </summary>
    void OnCentralDidStopScanning() {
      isCentralScanning = false;
      Repaint();
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidConnect(string identifier) {
      peripherals.Add(new DummyPeripheral() {
        identifier = identifier,
        isSimulated = identifier.StartsWith("SIMULATED_"),
      });
      Repaint();
    }

    /// <summary>
    /// Delegate should be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidDisconnect(string identifier) {
      peripherals.RemoveAll(peripheral => peripheral.identifier == identifier);
      Repaint();
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    void OnPeripheralDidFailToConnect(string identifier) {
      peripherals.RemoveAll(peripheral => peripheral.identifier == identifier);
      Repaint();
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
      Repaint();
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
      Repaint();
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
      Repaint();
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
      Repaint();
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
      Repaint();
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
      Repaint();
    }

    /// <summary>
    /// Disconnects all peripherals.
    /// </summary>
    void CancelAllPhysicalPeripheralConnections() {
      foreach (var peripheral in peripherals) {
        PilloDeviceManager.CancelPeripheralConnection(peripheral.identifier);
      }
      Repaint();
    }

    /// <summary>
    /// Powers off all peripherals.
    /// </summary>
    void PowerOffAllperipherals() {
      foreach (var peripheral in peripherals) {
        PilloDeviceManager.PowerOffPeripheral(peripheral.identifier);
      }
    }

    /// <summary>
    /// Calibrates all peripherals.
    /// </summary>
    void CalibrateAllperipherals() {
      foreach (var peripheral in peripherals) {
        PilloDeviceManager.StartPeripheralCalibration(peripheral.identifier);
      }
    }
  }
}