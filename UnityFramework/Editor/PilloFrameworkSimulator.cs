#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Hulan.PilloSDK.Framework.Payloads;

// Unity Engine Pillo SDK Framework Editor
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Editor {
  /// <summary>
  /// Pillo Framework Simulator can be used to test the Pillo Framework in the 
  /// Unity Editor.
  /// </summary>
  internal class PilloFrameworkSimulator : EditorWindow {
    /// <summary>
    /// The Pillo Framework Simulator window instance.
    /// </summary>
    private static PilloFrameworkSimulator instance;

    /// <summary>
    /// In order to simulate the Pillo Framework, we need to keep track of
    /// simulated peripherals.
    /// </summary>
    private class SimulatedPeripheral {
      /// <summary>
      /// A field with a change callback.
      /// </summary>
      /// <typeparam name="FieldType">The field's type</typeparam>
      internal class FieldWithChangeCallback<FieldType> {
        /// <summary>
        /// The field's protected value field. This field is used to store the
        /// field's value privately.
        /// </summary>
        private FieldType value;

        /// <summary>
        /// The field's change callback. This callback is invoked when the
        /// field's value changes to a new value. The new value is passed as
        /// an argument to the callback.
        /// </summary>
        private System.Action<FieldType> callback;

        /// <summary>
        /// Creates a new field with a change callback.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="callback">The callback method.</param>
        public FieldWithChangeCallback(FieldType value, System.Action<FieldType> callback) {
          this.value = value;
          this.callback = callback;
        }

        /// <summary>
        /// Gets or sets the field's value. When the value changes, the callback
        /// is invoked.
        /// </summary>
        public FieldType Value {
          get => value;
          set {
            if (!this.value.Equals(value)) {
              this.value = value;
              callback(value);
            }
          }
        }
      }


      /// <summary>
      /// Instanciates a new simulated peripheral.
      /// </summary>
      /// <param name="identifier">The peripheral's identifier.</param>
      internal SimulatedPeripheral(string identifier) {
        // Set all of the peripheral's fields.
        this.identifier = identifier;
        firmwareVersion = new FieldWithChangeCallback<string>(string.Empty, (value) => {
          SimulateInvokeUnityCallback("OnPeripheralFirmwareVersionDidChange", new PeripheralFirmwareVersionDidChangePayload() {
            identifier = identifier,
            firmwareVersion = value
          });
        });
        hardwareVersion = new FieldWithChangeCallback<string>(string.Empty, (value) => {
          SimulateInvokeUnityCallback("OnPeripheralHardwareVersionDidChange", new PeripheralHardwareVersionDidChangePayload() {
            identifier = identifier,
            hardwareVersion = value
          });
        });
        modelNumber = new FieldWithChangeCallback<string>(string.Empty, (value) => {
          SimulateInvokeUnityCallback("OnPeripheralModelNumberDidChange", new PeripheralModelNumberDidChangePayload() {
            identifier = identifier,
            modelNumber = value
          });
        });
        isConnected = new FieldWithChangeCallback<bool>(false, (value) => {
          if (value == true) {
            SimulateInvokeUnityCallback("OnPeripheralDidConnect", new PeripheralDidConnectPayload() {
              identifier = identifier
            });
          }
          else {
            SimulateInvokeUnityCallback("OnPeripheralDidDisconnect", new PeripheralDidDisconnectPayload() {
              identifier = identifier
            });
          }
        });
        chargeState = new FieldWithChangeCallback<PeripheralChargeState>(PeripheralChargeState.UNKNOWN, (value) => {
          SimulateInvokeUnityCallback("OnPeripheralChargeStateDidChange", new PeripheralChargeStateDidChangePayload() {
            identifier = identifier,
            chargeState = value
          });
        });
        batteryLevel = new FieldWithChangeCallback<int>(0, (value) => {
          SimulateInvokeUnityCallback("OnPeripheralBatteryLevelDidChange", new PeripheralBatteryLevelDidChangePayload() {
            identifier = identifier,
            batteryLevel = value
          });
        });
        pressure = new FieldWithChangeCallback<int>(0, (value) => {
          SimulateInvokeUnityCallback("OnPeripheralPressureDidChange", new PeripheralPressureDidChangePayload() {
            identifier = identifier,
            pressure = value
          });
        });
      }

      /// <summary>
      /// The simulated peripheral identifier.
      /// </summary>
      internal readonly string identifier;

      ///  <summary> 
      /// The simulated peripheral connection state.
      /// </summary> 
      internal readonly FieldWithChangeCallback<bool> isConnected;

      /// <summary>
      /// The simulated peripheral firmware version.
      /// </summary>
      internal readonly FieldWithChangeCallback<string> firmwareVersion;

      /// <summary>
      /// The simulated peripheral hardware version.
      /// </summary>
      internal readonly FieldWithChangeCallback<string> hardwareVersion;

      /// <summary>
      /// The simulated peripheral model number.
      /// </summary>
      internal readonly FieldWithChangeCallback<string> modelNumber;

      /// <summary>
      /// The simulated peripheral charge state.
      /// </summary>
      internal readonly FieldWithChangeCallback<PeripheralChargeState> chargeState;

      /// <summary>
      /// The simulated peripheral battery level.
      /// </summary>
      internal readonly FieldWithChangeCallback<int> batteryLevel;

      /// <summary>
      /// The simulated peripheral pressure.
      /// </summary>
      internal readonly FieldWithChangeCallback<int> pressure;
    }

    /// <summary>
    /// The simulated peripherals.
    /// </summary>
    private List<SimulatedPeripheral> peripherals;

    /// <summary>
    /// The simulated peripheral identifier.
    /// </summary>
    private int peripheralIdentifier;

    /// <summary>
    /// The scroll view position.
    /// </summary>
    private Vector2 scrollViewPosition;

    /// <summary>
    /// Shows the Pillo Framework Simulator window.
    /// </summary>
    [MenuItem("Window/Pillo Framework Simulator")]
    private static void ShowWindow() {
      EditorWindow.GetWindow(typeof(PilloFrameworkSimulator));
    }

    /// <summary>
    /// Method which simulates the Unity callback which is usually invoked by
    /// the native Pillo Framework.
    /// </summary>
    /// <param name="methodName">The name of the method to invoke.</param>
    /// <param name="payload">The payload to pass to the method.</param>
    private static void SimulateInvokeUnityCallback(string methodName, object payload) {
      var listener = GameObject.Find("~DeviceManagerCallbackListener");
      listener?.SendMessage(methodName, payload == null ? "" : JsonUtility.ToJson(payload));
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral to remove.</param>
    public static void RemovePeripheral(string identifier) {
      if (instance == null) {
        return;
      }
      foreach (var peripheral in instance.peripherals) {
        if (peripheral.identifier == identifier) {
          peripheral.isConnected.Value = false;
          instance.peripherals.Remove(peripheral);
          instance.Repaint();
          return;
        }
      }
    }

    /// <summary>
    /// Method invoked when the Pillo Framework Simulator window is enabled.
    /// </summary>
    private void OnEnable() {
      instance = this;
      // Setting the titleContent property is required to show the window
      // title.
      titleContent = new GUIContent("Pillo Framework Simulator");
      peripherals = new List<SimulatedPeripheral>();
      peripheralIdentifier = 0;
    }

    /// <summary>
    /// Method invoked when the Pillo Framework Simulator window is disabled.
    /// </summary>
    private void OnDisable() {
      instance = null;
      // Disconnect all peripherals before closing the window.
      foreach (var peripheral in peripherals) {
        peripheral.isConnected.Value = false;
      }
    }

    /// <summary>
    /// Adds a simulated peripheral.
    /// </summary>
    private void AddSimulatedPeripheral() {
      var peripheral = new SimulatedPeripheral($"simulated-peripheral-{++peripheralIdentifier}");
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
    private void RemoveSimulatedPeripheral(SimulatedPeripheral peripheral) {
      peripheral.isConnected.Value = false;
      peripherals.Remove(peripheral);
    }

    /// <summary>
    /// Draws the Pillo Framework Simulator window.
    /// </summary>
    private void OnGUI() {
      // Draw the Pillo Framework Simulator toolbar.
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
      // Don't draw the Pillo Framework Simulator content if the application
      // is not playing.
      if (Application.isPlaying == false) {
        EditorGUILayout.HelpBox("The Pillo Framework Simulator can only be used in Play Mode.", MessageType.Warning);
        return;
      }
      // Draw the Pillo Framework Simulator content.
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
        EditorGUILayout.TextField(peripheral.identifier);
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
#endif