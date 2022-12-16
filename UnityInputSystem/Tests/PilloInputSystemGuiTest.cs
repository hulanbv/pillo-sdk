using System;
using UnityEngine;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem.Tests {
  /// <summary>
  /// The Pillo Input system Test MonoBehaviour will be display all the incoming
  /// Pillo Input System events in the Unity Engine GUI.
  /// </summary>
  [AddComponentMenu ("Hulan/Pillo SDK/Input System/Tests/Pillo Input System Gui Test")]
  internal class PilloInputSystemGuiTest : MonoBehaviour {
    /// <summary>
    /// The scroll position of the GUI.
    /// </summary>
    private Vector2 scrollPosition = Vector2.zero;

    /// <summary>
    /// Draws the Pillo Input System Gui Test.
    /// </summary>
    private void OnGUI () {
      // Setting the GUI skin to a screen relative font size.
      GUI.skin.label.fontSize = (int)(Screen.height * 0.02f);
      GUI.skin.button.fontSize = (int)(Screen.height * 0.02f);
      GUI.skin.label.fixedHeight = (int)(Screen.height * 0.03f);
      GUI.skin.button.fixedHeight = (int)(Screen.height * 0.05f);
      // Displaying the Pillo Input System Gui Test.
      GUILayout.Label ("Pillo Input System Gui Test");
      scrollPosition = GUILayout.BeginScrollView (scrollPosition);
      for (var i = 0; i < PilloInputSystem.pilloInputDeviceCount; i++) {
        // Displaying each of the Pillo Input Devices.
        var pilloInputDevice = PilloInputSystem.pilloInputDevices[i];
        GUILayout.Label ($"Pillo Input Device {i}");
        GUILayout.Label ($" - Identifier: {pilloInputDevice.identifier}");
        GUILayout.Label ($" - Firmware Version: {pilloInputDevice.firmwareVersion}");
        GUILayout.Label ($" - Hardware Version: {pilloInputDevice.hardwareVersion}");
        GUILayout.Label ($" - Model Number: {pilloInputDevice.modelNumber}");
        GUILayout.Label ($" - Player Index: {pilloInputDevice.playerIndex}");
        GUILayout.Label ($" - Pressure: {pilloInputDevice.pressure}");
        GUILayout.Label ($" - Battery Level: {pilloInputDevice.batteryLevel}");
        GUILayout.Label ($" - Charge State: {pilloInputDevice.chargeState}");
        // Displaying the Pillo Input Device buttons.
        if (GUILayout.Button ("Power Off")) {
          pilloInputDevice.PowerOff ();
        }
        if (GUILayout.Button ("Start Calibration")) {
          pilloInputDevice.StartCalibration ();
        }
      }
      GUILayout.EndScrollView ();
    }
  }
}
