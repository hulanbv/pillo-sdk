using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Hulan.PilloSDK.Framework;
using Hulan.PilloSDK.Simulator.Core;

// Unity Engine Pillo SDK Simulator
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Simulator {
  /// <summary>
  /// Pillo Simulator can be used to test the Pillo Framework in the 
  /// Unity Editor.
  /// </summary>
  class DeviceSimulator : EditorWindow {
    /// <summary>
    /// The Pillo Simulator window instance.
    /// </summary>
    static DeviceSimulator instance;

    /// <summary>
    /// The simulated peripherals.
    /// </summary>
    List<SimulatedPillo> peripherals;

    /// <summary>
    /// The simulated peripheral identifier.
    /// </summary>
    int peripheralIdentifier;

    /// <summary>
    /// The scroll view position.
    /// </summary>
    Vector2 scrollViewPosition;

    /// <summary>
    /// Shows the Pillo Simulator window.
    /// </summary>
    [MenuItem("Window/Pillo Simulator")]
    static void ShowWindow() {
      GetWindow(typeof(DeviceSimulator));
    }

    /// <summary>
    /// Method which simulates the Unity callback which is usually invoked by
    /// the native Pillo Framework.
    /// </summary>
    /// <param name="methodName">The name of the method to invoke.</param>
    /// <param name="payload">The payload to pass to the method.</param>
    static internal void InvokeCallback(string methodName, object payload) {
      var listener = GameObject.Find("~DeviceManagerCallbackListener");
      listener.SendMessage(methodName, payload == null ? "" : JsonUtility.ToJson(payload));
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral to remove.</param>
    internal static void RemovePeripheral(string identifier) {
      if (instance == null) {
        return;
      }
      foreach (var peripheral in instance.peripherals) {
        if (peripheral.Identifier != identifier) {
          continue;
        }
        peripheral.isConnected.Value = false;
        instance.peripherals.Remove(peripheral);
        instance.Repaint();
        return;
      }
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is enabled.
    /// </summary>
    void OnEnable() {
      instance = this;
      titleContent = new GUIContent("Pillo Simulator");
      peripherals = new List<SimulatedPillo>();
      peripheralIdentifier = 0;
    }

    /// <summary>
    /// Method invoked when the Pillo Simulator window is disabled.
    /// </summary>
    void OnDisable() {
      instance = null;
      // Disconnect all peripherals before closing the window.
      foreach (var peripheral in peripherals) {
        peripheral.isConnected.Value = false;
      }
    }

    /// <summary>
    /// Adds a simulated peripheral.
    /// </summary>
    void AddSimulatedPeripheral() {
      var peripheral = new SimulatedPillo($"simulated-peripheral-{++peripheralIdentifier}");
      peripheral.isConnected.Value = true;
      peripheral.chargeState.Value = PeripheralChargeState.SLEEP_MODE;
      peripheral.batteryLevel.Value = 100;
      peripheral.modelNumber.Value = "simulated-peripheral 0.1f";
      peripheral.firmwareVersion.Value = "0.0.1f";
      peripheral.hardwareVersion.Value = "0.0.1f";
      peripherals.Add(peripheral);
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="peripheral">The peripheral to remove.</param>
    void RemoveSimulatedPeripheral(SimulatedPillo peripheral) {
      peripheral.isConnected.Value = false;
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
      // Don't draw the Pillo Simulator content if the application
      // is not playing.
      if (Application.isPlaying == false) {
        EditorGUILayout.HelpBox("The Pillo Simulator can only be used in Play Mode.", MessageType.Warning);
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
        GUI.enabled = false;
        GUILayout.Label("Identifier", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.Identifier);
        GUILayout.Label("Hardware Version", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.hardwareVersion.Value);
        GUILayout.Label("Firmware Version", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.firmwareVersion.Value);
        GUILayout.Label("Model Number", EditorStyles.boldLabel);
        EditorGUILayout.TextField(peripheral.modelNumber.Value);
        GUI.enabled = true;
        GUILayout.Space(10);
        // Draw the simulated peripheral battery services.
        GUILayout.Label("Battery State", EditorStyles.largeLabel);
        GUILayout.Label("Battery Level", EditorStyles.boldLabel);
        peripheral.batteryLevel.Value = EditorGUILayout.IntSlider(peripheral.batteryLevel.Value, 0, 100);
        GUILayout.Label("Charge State", EditorStyles.boldLabel);
        peripheral.chargeState.Value = (PeripheralChargeState)EditorGUILayout.EnumPopup(peripheral.chargeState.Value);
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