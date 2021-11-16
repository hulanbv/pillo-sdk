using UnityEngine;
using UnityEngine.UI;
using Hulan.PilloSDK.InputSystem;

namespace Hulan.PilloSDK.Tests {

  [AddComponentMenu ("Pillo SDK/Tests/Pillo Test Component")]
  public class PilloTestComponent : MonoBehaviour {

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

    public void OnCentralDidFailToInitialize (string reason) {
      this.Log ("Pillo Test Component: did fail to initialize: " + reason);
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
      this.Log ("Pillo Test Component: state change: " + pilloInputDevice.identifier);
    }

    private void Log (string text) {
      if (this.textDebug != null) {
        this.textDebug.text = text + "\n" + this.textDebug.text;
      }
    }
  }
}