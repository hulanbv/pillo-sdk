using UnityEngine;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem.Tests {
  /// <summary>
  /// The Pillo Input system Test MonoBehaviour will be debug log all the 
  /// incoming Pillo Input System events.
  /// </summary>
  [AddComponentMenu("Hulan/Pillo SDK/Input System/Tests/Pillo Input System Debug Test")]
  class PilloInputSystemDebugTest : MonoBehaviour {
    /// <summary>
    /// Binds the Pillo Framework events to the Pillo Framework Test MonoBe-
    /// haviour.
    /// </summary>
    void Start() {
      PilloInputSystem.onPilloInputDeviceDidConnect += OnPilloInputDeviceDidConnect;
      PilloInputSystem.onPilloInputDeviceDidDisconnect += OnPilloInputDeviceDidDisconnect;
      PilloInputSystem.onPilloInputDeviceDidFailToConnect += OnPilloInputDeviceDidFailToConnect;
      PilloInputSystem.onPilloInputDeviceStateDidChange += OnPilloInputDeviceStateDidChange;
    }

    /// <summary>
    /// Delegate will be invoked when a Pillo Input Device has been connected.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    void OnPilloInputDeviceDidConnect(PilloInputDevice pilloInputDevice) {
      Debug.Log($"Pillo Input Device did connect with identifier {pilloInputDevice.identifier}");
    }

    /// <summary>
    /// Delegate will be invoked when a Pillo Input Device has been discon-
    /// nected.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    void OnPilloInputDeviceDidDisconnect(PilloInputDevice pilloInputDevice) {
      Debug.Log($"Pillo Input Device did disconnect with identifier {pilloInputDevice.identifier}");
    }

    /// <summary>
    /// Delegate will be invoked when a Pillo Input Device has failed to con-
    /// nect.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    void OnPilloInputDeviceDidFailToConnect() {
      Debug.Log("Pillo Input Device did fail to connect");
    }

    /// <summary>
    /// Delegate will be invoked when a Pillo Input Device has changed state.
    /// </summary>
    /// <param name="pilloInputDevice">The Pillo Input Device.</param>
    void OnPilloInputDeviceStateDidChange(PilloInputDevice pilloInputDevice) {
      Debug.Log($"Pillo Input Device state did change with identifier {pilloInputDevice.identifier}, Pressure: {pilloInputDevice.pressure} Battery Level: {pilloInputDevice.batteryLevel} Charge State: {pilloInputDevice.chargeState}");
    }
  }
}
