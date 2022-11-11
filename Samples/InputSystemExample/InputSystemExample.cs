using UnityEngine;
using UnityEngine.UI;
using Hulan.PilloSDK.InputSystem;

namespace Hulan.PilloSDK.Samples.InputSystemExample {

  [AddComponentMenu ("Pillo SDK/Samples/Input System Example")]
  public class InputSystemExample : MonoBehaviour {

    public Text textDebug;

    public void Start () {
      PilloInput.onCentralDidInitialize += this.OnCentralDidInitialize;
      PilloInput.onCentralDidFailToInitialize += this.OnCentralDidFailToInitialize;
      PilloInput.onPilloInputDeviceDidConnect += this.OnPilloInputDeviceDidConnect;
      PilloInput.onPilloInputDeviceDidDisconnect += this.OnPilloInputDeviceDidDisconnect;
      PilloInput.onPilloInputDeviceDidFailToConnect += this.OnPilloInputDeviceDidFailToConnect;
      PilloInput.onPilloInputDeviceStateDidChange += this.OnPilloInputDeviceStateDidChange;
    }

    public void OnDestroy () {
      PilloInput.onCentralDidInitialize -= this.OnCentralDidInitialize;
      PilloInput.onCentralDidFailToInitialize -= this.OnCentralDidFailToInitialize;
      PilloInput.onPilloInputDeviceDidConnect -= this.OnPilloInputDeviceDidConnect;
      PilloInput.onPilloInputDeviceDidDisconnect -= this.OnPilloInputDeviceDidDisconnect;
      PilloInput.onPilloInputDeviceDidFailToConnect -= this.OnPilloInputDeviceDidFailToConnect;
      PilloInput.onPilloInputDeviceStateDidChange -= this.OnPilloInputDeviceStateDidChange;
    }

    public void OnCentralDidInitialize () {
      this.Log ("Pillo Test Component: did initialize");
    }

    public void OnCentralDidFailToInitialize (string message) {
      this.Log ("Pillo Test Component: did fail to initialize: " + message);
    }

    public void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice) {
      this.Log ("Pillo Test Component: connection successful: " + pilloInputDevice.identifier);
    }

    public void OnPilloInputDeviceDidDisconnect (PilloInputDevice pilloInputDevice) {
      this.Log ("Pillo Test Component: disconnected: " + pilloInputDevice.identifier);
    }

    public void OnPilloInputDeviceDidFailToConnect () {
      this.Log ("Pillo Test Component: connection failed");
    }

    public void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice) {
      this.Log ("Pillo Test Component: state change: " + pilloInputDevice.identifier + "\n> Pressure: " + pilloInputDevice.pressure + "\n> Battery: " + pilloInputDevice.batteryLevel);
    }

    private void Log (string text) {
      if (this.textDebug != null) {
        this.textDebug.text = text + "\n" + this.textDebug.text;
      }
    }
  }
}