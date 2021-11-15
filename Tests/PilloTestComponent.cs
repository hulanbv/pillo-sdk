using UnityEngine;
using UnityEngine.UI;
using Hulan.Pillo.SDK.InputSystem;

namespace Hulan.Pillo.SDK.Tests {

  [AddComponentMenu ("Pillo SDK/Tests/Pillo Test Component")]
  public class PilloTestComponent : MonoBehaviour {

    // public Text textDebug;

    public void Start () {
      //   PilloInput.onDidInitialize += this.OnDidInitialize;
      //   PilloInput.onDidFailToInitialize += this.OnDidFailToInitialize;
      //   PilloInput.onDidConnect += this.OnDidConnect;
      //   PilloInput.onDidDisconnect += this.OnDidDisconnect;
      //   PilloInput.onDidFailToConnect += this.OnDidFailToConnect;
      //   PilloInput.onBatteryLevelDidChange += this.OnBatteryLevelDidChange;
      //   PilloInput.onPressureDidChange += this.OnPressureDidChange;
    }

    // public void OnDestroy () {
    //   PilloInput.onDidInitialize -= this.OnDidInitialize;
    //   PilloInput.onDidFailToInitialize -= this.OnDidFailToInitialize;
    //   PilloInput.onDidConnect -= this.OnDidConnect;
    //   PilloInput.onDidDisconnect -= this.OnDidDisconnect;
    //   PilloInput.onDidFailToConnect -= this.OnDidFailToConnect;
    //   PilloInput.onBatteryLevelDidChange -= this.OnBatteryLevelDidChange;
    //   PilloInput.onPressureDidChange -= this.OnPressureDidChange;
    // }

    // public void OnDidInitialize () {
    //   this.Log ("Pillo Test Component: did initialize");
    // }

    // public void OnDidFailToInitialize (string reason) {
    //   this.Log ("Pillo Test Component: did fail to initialize");
    // }

    // public void OnDidConnect (string identifier) {
    //   this.Log ("Pillo Test Component: connection successful: " + identifier);
    // }

    // public void OnDidDisconnect (string identifier) {
    //   this.Log ("Pillo Test Component: disconnected: " + identifier);
    // }

    // public void OnDidFailToConnect (string identifier) {
    //   this.Log ("Pillo Test Component: connection failed: " + identifier);
    // }

    // public void OnBatteryLevelDidChange (string identifier, int batteryLevel) {
    //   this.Log ("Pillo Test Component: battery level did change: " + batteryLevel + ", " + identifier);
    // }

    // public void OnPressureDidChange (string identifier, int pressure) {
    //   this.Log ("Pillo Test Component: pressure did change: " + pressure + ", " + identifier);
    // }

    // private void Log (string text) {
    //   if (this.textDebug != null) {
    //     this.textDebug.text = text + "\n" + this.textDebug.text;
    //   }
    // }
  }
}