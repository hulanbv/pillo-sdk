using UnityEngine;
using UnityEngine.UI;
using Hulan.PilloSDK.InputSystem;

namespace Hulan.PilloSDK.Samples.InputSystemExample {

  [AddComponentMenu ("Pillo SDK/Samples/Input System Example")]
  public class InputSystemExample : MonoBehaviour {

    public Text textDebug;

    private void Update () {
      var text = "Pillo SDK Pillo Input Tests\n";
      text += "Pillo Input Central Initialized: " + PilloInput.isCentralInitialized + "\n";
      text += "Pillo Input Device Count: " + PilloInput.pilloInputDeviceCount + "\n";
      text += "Pillo Input Devices:\n";
      foreach (var pilloInputDevice in PilloInput.pilloInputDevices) {
        text += "- " + pilloInputDevice.identifier + " (" + pilloInputDevice.playerIndex + ")\n";
        text += "  - Pressure: " + pilloInputDevice.pressure + "\n";
        text += "  - Battery Level: " + pilloInputDevice.batteryLevel + "\n";
        text += "  - Charge State: " + pilloInputDevice.chargeState + "\n";
      }
      this.textDebug.text = text;
    }
  }
}