using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Hulan.PilloSDK.DeviceManager;
using Hulan.PilloSDK.Simulator.Core;

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
    /// The simulated peripherals.
    /// </summary>
    static readonly List<SimulatedPeripheral> peripherals = new();

    /// <summary>
    /// The scroll view position.
    /// </summary>
    Vector2 scrollViewPosition;

    /// <summary>
    /// Shows the Pillo Simulator window.
    /// </summary>
    [MenuItem("Window/Pillo Simulator", priority = 500)]
    static void ShowWindow() {
      GetWindow<SimulatorWindow>();
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is enabled.
    /// </summary>
    void OnEnable() {
      EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
      var icon = EditorGUIUtility.IconContent("d_PreMatCube");
      titleContent = new GUIContent("Pillo Simulator", icon.image);
      autoConnect = EditorPrefs.GetBool(autoConnectSettingKey, false);
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is disabled.
    /// </summary>
    void OnDisable() {
      EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
      // Disconnect all peripherals before closing the window.
      foreach (var peripheral in peripherals) {
        peripheral.Disconnect();
      }
      peripherals.Clear();
    }

    /// <summary>
    /// Method invoked when the Play Mode state changes.
    /// </summary>
    /// <param name="state">The new Play Mode state.</param>
    void OnPlayModeStateChanged(PlayModeStateChange state) {
      switch (state) {
        case PlayModeStateChange.EnteredPlayMode:
          // Add new peripherals when entering Play Mode.
          if (!autoConnect) {
            return;
          }
          for (int i = 0; i < 2; i++) {
            AddSimulatedPeripheral();
          }
          break;
        case PlayModeStateChange.ExitingPlayMode:
          // Disconnect all peripherals when exiting Play Mode.
          foreach (var peripheral in peripherals) {
            peripheral.Disconnect();
          }
          break;
      }
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
      var peripheral = new SimulatedPeripheral();
      peripherals.Add(peripheral);
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="peripheral">The peripheral to remove.</param>
    void RemoveSimulatedPeripheral(SimulatedPeripheral peripheral) {
      peripheral.Disconnect();
      peripherals.Remove(peripheral);
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
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      if (Application.isPlaying == false) {
        // Draw a help box to inform the user that the Pillo Simulator
        // can only be used in Play Mode.
        EditorGUILayout.HelpBox("The Pillo Simulator can only be used in Play Mode.", MessageType.Info);
        return;
      }
      // Draw the Pillo Simulator content.
      scrollViewPosition = GUILayout.BeginScrollView(scrollViewPosition);
      GUILayout.BeginHorizontal();
      for (int i = 0; i < peripherals.Count; i++) {
        var peripheral = peripherals[i];
        // Draw the simulated peripheral.
        GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(200));
        GUILayout.Label("Simulated Peripheral", EditorStyles.largeLabel);
        GUILayout.Space(10);
        // Draw the simulated peripheral identifier in a read-only text field.
        EditorGUI.BeginDisabledGroup(true);
        GUILayout.Label("Identifier", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.identifier);
        GUILayout.Label("Hardware Version", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.hardwareVersion.Value);
        GUILayout.Label("Firmware Version", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.firmwareVersion.Value);
        GUILayout.Label("Model Number", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.modelNumber.Value);
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10);
        // Draw the simulated peripheral battery services.
        GUILayout.Label("Battery State", EditorStyles.largeLabel);
        GUILayout.Label("Battery Level", EditorStyles.boldLabel);
        peripheral.batteryLevel.Value = EditorGUILayout.IntSlider(peripheral.batteryLevel.Value, 0, 100);
        GUILayout.Label("Charge State", EditorStyles.boldLabel);
        peripheral.chargingState.Value = (ChargingState)EditorGUILayout.EnumPopup(peripheral.chargingState.Value);
        GUILayout.Space(10);
        // Draw the simulated peripheral pressure.
        GUILayout.Label("Pressure", EditorStyles.largeLabel);
        GUILayout.Label("Pressure Level", EditorStyles.boldLabel);
        peripheral.pressure.Value = EditorGUILayout.IntSlider(peripheral.pressure.Value, 0, 1024);
        // Draw the simulated peripheral actions.
        GUILayout.FlexibleSpace();
        GUILayout.Label("Actions", EditorStyles.largeLabel);
        if (GUILayout.Button("Power Off")) {
          RemoveSimulatedPeripheral(peripheral);
        }
        GUILayout.EndVertical();
      }
      GUILayout.EndHorizontal();
      GUILayout.EndScrollView();
    }
  }
}