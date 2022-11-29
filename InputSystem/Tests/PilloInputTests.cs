using UnityEngine;
using Hulan.PilloSDK.InputSystem;

namespace Hulan.PilloSDK.Tests {

  [AddComponentMenu ("Pillo SDK/Tests/Pillo Input Tests")]
  public class PilloInputTests : MonoBehaviour {
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
      Debug.Log ("Pillo Test Component: did initialize");
    }

    public void OnCentralDidFailToInitialize (string message) {
      Debug.Log ("Pillo Test Component: did fail to initialize: " + message);
    }

    public void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice) {
      Debug.Log ("Pillo Test Component: connection successful: " + pilloInputDevice.identifier + " (" + pilloInputDevice.playerIndex + ")");
    }

    public void OnPilloInputDeviceDidDisconnect (PilloInputDevice pilloInputDevice) {
      Debug.Log ("Pillo Test Component: disconnected: " + pilloInputDevice.identifier + " (" + pilloInputDevice.playerIndex + ")");
    }

    public void OnPilloInputDeviceDidFailToConnect () {
      Debug.Log ("Pillo Test Component: connection failed");
    }

    public void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice) {
      Debug.Log ("Pillo Test Component: state change: " + pilloInputDevice.identifier + " (" + pilloInputDevice.playerIndex + ")" + "\n- Pressure: " + pilloInputDevice.pressure + "\n- Battery: " + pilloInputDevice.batteryLevel);
    }
  }
}