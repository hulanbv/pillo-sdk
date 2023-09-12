using UnityEngine;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem.Tests {
  /// <summary>
  /// The Pillo Input system Test MonoBehaviour will be display all the incoming
  /// Pillo Input System events in the Unity Engine GUI.
  /// </summary>
  [AddComponentMenu("Hulan/Pillo SDK/Input System/Tests/Pillo Input System Gui Test")]
  class PilloInputSystemGuiTest : MonoBehaviour {
    /// <summary>
    /// The scroll position of the GUI.
    /// </summary>
    Vector2 scrollPosition;

    /// <summary>
    /// Defines if the cursor click is requested.
    /// </summary>
    bool requestCursorClick;

    /// <summary>
    /// The cursor selection position.
    /// </summary>
    int cursorSelectionPosition;

    /// <summary>
    /// The cursor drawing position.
    /// </summary>
    int cursorDrawingPosition;

    /// <summary>
    /// Updates the Pillo Input System Gui Test.
    /// </summary>
    void Update() {
      if (Input.GetKeyDown(KeyCode.Joystick1Button14) || Input.GetKeyDown(KeyCode.Return)) {
        // Requesting the cursor click when either the return key or the AppleTV
        // remote touchpad is pressed.
        requestCursorClick = true;
      }
      if (Input.GetKeyDown(KeyCode.Joystick1Button15) || Input.GetKeyDown(KeyCode.Space)) {
        // Moving the cursor selection position up when either the spacebar
        // or the AppleTV remote play/pause button is pressed.
        cursorSelectionPosition++;
      }
    }

    /// <summary>
    /// Draws the Pillo Input System Gui Test.
    /// </summary>
    void OnGUI() {
      cursorDrawingPosition = 0;
      // Setting the GUI skin to a screen relative font size.
      GUI.skin.label.fontSize = (int)(Screen.height * 0.02f);
      GUI.skin.button.fontSize = (int)(Screen.height * 0.02f);
      GUI.skin.label.fixedHeight = (int)(Screen.height * 0.03f);
      GUI.skin.button.fixedHeight = (int)(Screen.height * 0.05f);
      // Displaying the Pillo Input System Gui Test.
      GUILayout.Label("Pillo Input System Gui Test");
      scrollPosition = GUILayout.BeginScrollView(scrollPosition);
      for (var i = 0; i < PilloInputSystem.pilloInputDeviceCount; i++) {
        // Displaying each of the Pillo Input Devices.
        var pilloInputDevice = PilloInputSystem.pilloInputDevices[i];
        GUILayout.Label($"Pillo Input Device {i}");
        GUILayout.Label($" - Identifier: {pilloInputDevice.identifier}");
        GUILayout.Label($" - Firmware Version: {pilloInputDevice.firmwareVersion}");
        GUILayout.Label($" - Hardware Version: {pilloInputDevice.hardwareVersion}");
        GUILayout.Label($" - Model Number: {pilloInputDevice.modelNumber}");
        GUILayout.Label($" - Player Index: {pilloInputDevice.playerIndex}");
        GUILayout.Label($" - Pressure: {pilloInputDevice.pressure}");
        GUILayout.Label($" - Battery Level: {pilloInputDevice.batteryLevel}");
        GUILayout.Label($" - Charge State: {pilloInputDevice.chargeState}");
        // Displaying the Pillo Input Device buttons.
        if (DrawButton("Power Off")) {
          pilloInputDevice.PowerOff();
        }
        if (DrawButton("Start Calibration")) {
          pilloInputDevice.StartCalibration();
        }
      }
      GUILayout.EndScrollView();
      // Resetting the cursor selection position when overflow.
      if (cursorSelectionPosition >= cursorDrawingPosition) {
        cursorSelectionPosition = 0;
      }
      requestCursorClick = false;
    }

    bool DrawButton(string label) {
      var isSelected = cursorSelectionPosition == cursorDrawingPosition++;
      GUI.skin.button.fontStyle = isSelected ? FontStyle.BoldAndItalic : FontStyle.Normal;
      var didClickButton = GUILayout.Button(label);
      return didClickButton || (isSelected && requestCursorClick);
    }
  }
}
