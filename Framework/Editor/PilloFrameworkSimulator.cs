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
      /// The simulated peripheral identifier.
      /// </summary>
      internal string identifier;

      /// <summary>
      /// The local simulated peripheral connection state.
      /// </summary>
      private bool localIsConnected;

      /// <summary>
      /// The simulated peripheral connection state.
      /// </summary>
      internal bool isConnected {
        get => localIsConnected;
        set {
          // Only invoke the callback if the connection state has changed.
          if (localIsConnected != value && value == true) {
            SimulateInvokeUnityCallback ("OnPeripheralDidConnect", new PeripheralDidConnectPayload () {
              identifier = identifier
            });
          } else if (localIsConnected != value && value == false) {
            SimulateInvokeUnityCallback ("OnPeripheralDidDisconnect", new PeripheralDidDisconnectPayload () {
              identifier = identifier
            });
          }
          localIsConnected = value;
        }
      }

      /// <summary>
      /// The local simulated peripheral charge state.
      /// </summary>
      private PeripheralChargeState localChargeState;

      /// <summary>
      /// The simulated peripheral charge state.
      /// </summary>
      internal PeripheralChargeState chargeState {
        get => localChargeState;
        set {
          // Only invoke the callback if the charge state has changed.
          if (localChargeState != value) {
            SimulateInvokeUnityCallback ("OnPeripheralChargeStateDidChange", new PeripheralChargeStateDidChangePayload () {
              identifier = identifier,
              chargeState = value
            });
          }
          localChargeState = value;
        }
      }

      /// <summary>
      /// The local simulated peripheral battery level.
      /// </summary>
      private int localBatteryLevel;

      /// <summary>
      /// The simulated peripheral battery level.
      /// </summary>
      internal int batteryLevel {
        get => localBatteryLevel;
        set {
          // Only invoke the callback if the battery level has changed.
          if (localBatteryLevel != value) {
            SimulateInvokeUnityCallback ("OnPeripheralBatteryLevelDidChange", new PeripheralBatteryLevelDidChangePayload () {
              identifier = identifier,
              batteryLevel = value
            });
          }
          localBatteryLevel = value;
        }
      }

      /// <summary>
      /// The local simulated peripheral pressure.
      /// </summary>
      private int localPressure;

      /// <summary>
      /// The simulated peripheral pressure.
      /// </summary>
      internal int pressure {
        get => localPressure;
        set {
          // Only invoke the callback if the pressure has changed.
          if (localPressure != value) {
            SimulateInvokeUnityCallback ("OnPeripheralPressureDidChange", new PeripheralPressureDidChangePayload () {
              identifier = identifier,
              pressure = value
            });
          }
          localPressure = value;
        }
      }
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
    [MenuItem ("Window/Pillo Framework Simulator")]
    private static void ShowWindow () {
      EditorWindow.GetWindow (typeof (PilloFrameworkSimulator));
    }

    /// <summary>
    /// Method which simulates the Unity callback which is usually invoked by
    /// the native Pillo Framework.
    /// </summary>
    /// <param name="methodName">The name of the method to invoke.</param>
    /// <param name="payload">The payload to pass to the method.</param>
    private static void SimulateInvokeUnityCallback (string methodName, object payload) {
      var listener = GameObject.Find ("~DeviceManagerCallbackListener");
      listener?.SendMessage (methodName, payload == null ? "" : JsonUtility.ToJson (payload));
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral to remove.</param>
    public static void RemovePeripheral (string identifier) {
      if (instance == null) {
        return;
      }
      foreach (var peripheral in instance.peripherals) {
        if (peripheral.identifier == identifier) {
          peripheral.isConnected = false;
          instance.peripherals.Remove (peripheral);
          instance.Repaint ();
          return;
        }
      }
    }

    /// <summary>
    /// Method invoked when the Pillo Framework Simulator window is enabled.
    /// </summary>
    private void OnEnable () {
      instance = this;
      // Setting the titleContent property is required to show the window
      // title.
      titleContent = new GUIContent ("Pillo Framework Simulator");
      peripherals = new List<SimulatedPeripheral> ();
      peripheralIdentifier = 0;
    }

    /// <summary>
    /// Adds a simulated peripheral.
    /// </summary>
    private void AddSimulatedPeripheral () {
      peripherals.Add (new SimulatedPeripheral () {
        identifier = $"sp-{++peripheralIdentifier}",
        isConnected = true,
        chargeState = PeripheralChargeState.SLEEP_MODE,
        batteryLevel = 100,
      });
    }

    /// <summary>
    /// Removes a simulated peripheral.
    /// </summary>
    /// <param name="peripheral">The peripheral to remove.</param>
    private void RemoveSimulatedPeripheral (SimulatedPeripheral peripheral) {
      peripheral.isConnected = false;
      peripherals.Remove (peripheral);
    }

    /// <summary>
    /// Draws the Pillo Framework Simulator window.
    /// </summary>
    private void OnGUI () {
      if (Application.isPlaying == false) {
        EditorGUILayout.HelpBox ("The Pillo Framework Simulator can only be used in Play Mode.", MessageType.Warning);
        return;
      }
      // Draw the Pillo Framework Simulator toolbar.
      GUILayout.BeginHorizontal (EditorStyles.toolbar);
      if (GUILayout.Button ("Connect New", EditorStyles.toolbarButton)) {
        AddSimulatedPeripheral ();
      }
      GUILayout.FlexibleSpace ();
      GUILayout.EndHorizontal ();
      // Draw the Pillo Framework Simulator content.
      scrollViewPosition = GUILayout.BeginScrollView (scrollViewPosition);
      GUILayout.BeginHorizontal ();
      for (int i = 0; i < peripherals.Count; i++) {
        var peripheral = peripherals[i];
        // Draw the simulated peripheral.
        GUILayout.BeginVertical (EditorStyles.helpBox, GUILayout.Width (200));
        GUILayout.Label ("Simulated Peripheral", EditorStyles.largeLabel);
        GUILayout.Space (10);
        // Draw the simulated peripheral identifier in a read-only text field.
        GUILayout.Label ("Identifier", EditorStyles.boldLabel);
        GUI.enabled = false;
        EditorGUILayout.TextField (peripheral.identifier);
        GUI.enabled = true;
        GUILayout.Space (10);
        // Draw the simulated peripheral battery services.
        GUILayout.Label ("Battery State", EditorStyles.largeLabel);
        GUILayout.Label ("Battery Level", EditorStyles.boldLabel);
        peripheral.batteryLevel = EditorGUILayout.IntSlider (peripheral.batteryLevel, 0, 100);
        GUILayout.Label ("Charge State", EditorStyles.boldLabel);
        peripheral.chargeState = (PeripheralChargeState)EditorGUILayout.EnumPopup (peripheral.chargeState);
        GUILayout.Space (10);
        // Draw the simulated peripheral pressure.
        GUILayout.Label ("Pressure", EditorStyles.largeLabel);
        GUILayout.Label ("Pressure Level", EditorStyles.boldLabel);
        peripheral.pressure = EditorGUILayout.IntSlider (peripheral.pressure, 0, 1024);
        // Draw the simulated peripheral actions.
        GUILayout.FlexibleSpace ();
        GUILayout.Label ("Actions", EditorStyles.largeLabel);
        if (GUILayout.Button ("Power Off")) {
          RemoveSimulatedPeripheral (peripheral);
        }
        GUILayout.EndVertical ();
      }
      GUILayout.EndHorizontal ();
      GUILayout.EndScrollView ();
    }
  }
}
#endif