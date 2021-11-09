using UnityEngine;
using UnityEngine.UI;
using Hulan.Pillo.SDK;

namespace Hulan.Pillo.Test {
  public class PilloTestComponent : MonoBehaviour {

    public Text text;

    public void Start () {
      PilloFramework.Initialize ();
      PilloFramework.onDidInitialize += this.OnDidInitialize;
      PilloFramework.onDidFailToInitialize += this.OnDidFailToInitialize;
      PilloFramework.onPilloDidConnect += this.OnPilloDidConnect;
      PilloFramework.onPilloDidDisconnect += this.OnPilloDidDisconnect;
      PilloFramework.onPilloDidFailToConnect += this.OnPilloDidFailToConnect;
      PilloFramework.onBatteryLevelDidChange += this.OnBatteryLevelDidChange;
      PilloFramework.onPressureDidChange += this.OnPressureDidChange;
    }

    public void OnDestroy () {
      PilloFramework.onDidInitialize -= this.OnDidInitialize;
      PilloFramework.onDidFailToInitialize -= this.OnDidFailToInitialize;
      PilloFramework.onPilloDidConnect -= this.OnPilloDidConnect;
      PilloFramework.onPilloDidDisconnect -= this.OnPilloDidDisconnect;
      PilloFramework.onPilloDidFailToConnect -= this.OnPilloDidFailToConnect;
      PilloFramework.onBatteryLevelDidChange -= this.OnBatteryLevelDidChange;
      PilloFramework.onPressureDidChange -= this.OnPressureDidChange;
    }

    public void OnDidInitialize () {
      this.Log ("Pillo Test Component: did initialize");
    }

    public void OnDidFailToInitialize (string reason) {
      this.Log ("Pillo Test Component: did fail to initialize");
    }

    public void OnPilloDidConnect (string identifier) {
      this.Log ("Pillo Test Component: connection successful: " + identifier);
    }

    public void OnPilloDidDisconnect (string identifier) {
      this.Log ("Pillo Test Component: disconnected: " + identifier);
    }

    public void OnPilloDidFailToConnect (string identifier) {
      this.Log ("Pillo Test Component: connection failed: " + identifier);
    }

    public void OnBatteryLevelDidChange (string identifier, int batteryLevel) {
      this.Log ("Pillo Test Component: battery level did change: " + batteryLevel + ", " + identifier);
    }

    public void OnPressureDidChange (string identifier, int pressure) {
      this.Log ("Pillo Test Component: pressure did change: " + pressure + ", " + identifier);
    }

    private void Log (string text) {
      if (this.text != null) {
        this.text.text = text + "\n" + this.text.text;
      }
    }
  }
}