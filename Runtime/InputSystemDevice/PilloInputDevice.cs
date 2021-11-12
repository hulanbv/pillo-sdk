#if PILLO_SDK_UNITY_INPUTSYSTEM
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace Hulan.Pillo.SDK.InputSystemDevice {

  // https://wilcoboode.com/2020/04/16/elementor-86/
  // https://github.com/Unity-Technologies/InputSystem/blob/develop/Packages/com.unity.inputsystem/Documentation~/Devices.md
  // https://github.com/Unity-Technologies/InputSystem/blob/develop/Packages/com.unity.inputsystem/InputSystem/Plugins/DualShock/DualShockGamepadHID.cs

  // InputControlLayoutAttribute attribute is only necessary if you want to 
  // override default behavior that occurs when registering your device as a 
  // layout. The most common use of InputControlLayoutAttribute is to direct the 
  // system to a custom "state struct" through the `stateType` property.
  [InputControlLayout (displayName = "Pillo", stateType = typeof (PilloInputDeviceState))]
  public class PilloInputDevice : InputDevice, IInputUpdateCallbackReceiver {

    public ButtonControl hugGentlyButton { get; private set; }
    public ButtonControl hugFirmlyButton { get; private set; }

    // The Input System calls this method after it constructs the Device,
    // but before it adds the device to the system. Do any last-minute setup
    // here.
    protected override void FinishSetup () {
      base.FinishSetup ();

      this.hugGentlyButton = this.GetChildControl<ButtonControl> ("hugGently");
      this.hugFirmlyButton = this.GetChildControl<ButtonControl> ("hugFirmly");
    }

    public void OnUpdate () {
      // In practice, this would read out data from an external
      // API. This example uses some empty input.
      var state = new PilloInputDeviceState ();
      // state.press = (ushort)Random.Range (0, 255);
      InputSystem.QueueStateEvent (this, state);
    }

#if UNITY_EDITOR
    [MenuItem ("Pillo SDK/Input System/Register Layout")]
    private static void PilloSDKInputSystemRegisterLayout () {
      var pilloInputDeviceMatcher = new InputDeviceMatcher ()
        .WithInterface ("Pillo HID")
        .WithProduct ("Pillo")
        .WithManufacturer ("Hulan");
      InputSystem.RegisterLayout<PilloInputDevice> ();
      InputSystem.RegisterLayoutMatcher<PilloInputDevice> (pilloInputDeviceMatcher);
    }

    [MenuItem ("Pillo SDK/Input System/Add Virtual Device")]
    private static void PilloSDKInputSystemAddVirtualDevice () {
      // Adds a new device which matches the device description of the Pillo
      // Input Device.
      InputSystem.AddDevice (new InputDeviceDescription {
        interfaceName = "Pillo HID",
        product = "Pillo",
        manufacturer = "Hulan",
        // The serial indicated which Pillo is connected.
        serial = "PILLO_HID_VIRTUAL",
      });
    }

    [MenuItem ("Pillo SDK/Input System/Remove Virtual Device")]
    private static void PilloSDKInputSystemRemoveVirtualDevice () {
      // Removed a device which matches the device description of the Pillo
      // Input Device.
      var pilloInputDeviceDescription = new InputDeviceDescription {
        interfaceName = "Pillo HID",
        product = "Pillo",
        manufacturer = "Hulan",
        // The serial indicated which Pillo is connected.
        serial = "PILLO_HID_VIRTUAL"
      };
      // If a device matches the description, remove it.
      var device = InputSystem.devices.FirstOrDefault (
        device => device.description == pilloInputDeviceDescription);
      if (device != null) {
        InputSystem.RemoveDevice (device);
      }
    }
#endif
  }
}
#endif