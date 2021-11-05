using UnityEngine;
using Hulan.Pillo.SDK;

namespace Hulan.Pillo.Test {
  public class PilloTestComponent : MonoBehaviour {
    public void Start () {
      PilloFramework.Initialize ();
      PilloFramework.onDidInitialize += this.OnDidInitialize;
      PilloFramework.onDidFailToInitialize += this.OnDidFailToInitialize;
      PilloFramework.onPilloDidConnect += this.OnPilloDidConnect;
      PilloFramework.onPilloDidFailToConnect += this.OnPilloDidFailToConnect;
      PilloFramework.onBatteryLevelDidChange += this.OnBatteryLevelDidChange;
      PilloFramework.onPressureDidChange += this.OnPressureDidChange;
    }

    public void OnDidInitialize () {
      Debug.Log ("Pillo Test Component did initialize");
    }

    public void OnDidFailToInitialize (string reason) {
      Debug.Log ("Pillo Test Component did fail to initialize");
    }

    public void OnPilloDidConnect (string identifier) {
      Debug.Log ("Pillo Test Component connection successful: " + identifier);
    }

    public void OnPilloDidFailToConnect (string identifier) {
      Debug.Log ("Pillo Test Component connection failed: " + identifier);
    }

    public void OnBatteryLevelDidChange (int batteryLevel) {
      Debug.Log ("Pillo Test Component battery level did change: " + batteryLevel);
    }

    public void OnPressureDidChange (int pressure) {
      Debug.Log ("Pillo Test Component pressure did change: " + pressure);
    }
  }
}