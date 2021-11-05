using UnityEngine;
using Hulan.Pillo.SDK;

namespace Hulan.Pillo.Test {
  public class PilloTestComponent : MonoBehaviour, IPilloFrameworkDelegate {
    public PilloFramework pilloFramework;

    public void Start () {
      this.pilloFramework = new PilloFramework (this);
    }

    public void OnDidInitialize () {
      Debug.Log ("Pillo Test Component did initialize");
    }

    public void OnBluetoothNotAvailable () {
      Debug.Log ("Pillo Test Component bluetooth not available");
    }

    public void OnConnectionSuccessful (string peripheralIdentifier) {
      Debug.Log ("Pillo Test Component connection successful: " + peripheralIdentifier);
    }

    public void OnConnectionFailed (string peripheralIdentifier) {
      Debug.Log ("Pillo Test Component connection failed: " + peripheralIdentifier);
    }

    public void OnBatteryLevelDidChange (int batteryLevel) {
      Debug.Log ("Pillo Test Component battery level did change: " + batteryLevel);
    }

    public void OnPressureDidChange (int pressure) {
      Debug.Log ("Pillo Test Component pressure did change: " + pressure);
    }
  }
}