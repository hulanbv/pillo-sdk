using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Hulan.PilloSDK.Framework;
using Hulan.PilloSDK.Simulator.Core;

namespace Hulan.PilloSDK.Simulator {
  /// <summary>
  /// Pillo Simulator can be used to test the Pillo Framework in the 
  /// Unity Editor without the need of a physical Pillo device.
  /// </summary>
  class SimulatorWindow : EditorWindow {
    /// <summary>
    /// The simulated peripherals.
    /// </summary>
    static readonly List<SimulatedPillo> peripherals = new();

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
      var icon = EditorGUIUtility.IconContent("d_PreMatCube");
      titleContent = new GUIContent("Pillo Simulator", icon.image);
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is disabled.
    /// </summary>
    void OnDisable() {
      // instance = null;
      // Disconnect all peripherals before closing the window.
      foreach (var peripheral in peripherals) {
        // peripheral.isConnected = false;
        // TODO
      }
    }

    /// <summary>
    /// Adds a simulated peripheral.
    /// </summary>
    void AddSimulatedPeripheral() {
      var peripheral = new SimulatedPillo();
      peripherals.Add(peripheral);
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="peripheral">The peripheral to remove.</param>
    void RemoveSimulatedPeripheral(SimulatedPillo peripheral) {
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