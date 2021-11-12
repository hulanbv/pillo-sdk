#if UNITY_EDITOR && PILLO_SDK_UNITY_INPUTSYSTEM

using System.Linq;
using UnityEditor;
using UnityEngine.InputSystem.Layouts;
using Hulan.Pillo.SDK.InputSystemDevice;
using UnityEngine.InputSystem;

namespace Hulan.Pillo.SDK.Editor.Experimental {
  public static class PilloInputEditor {

    [MenuItem ("Pillo SDK/Editor Input/Register Layout")]
    public static void EditorInputRegisterLayout () {
      var pilloInputDeviceMatcher = new InputDeviceMatcher ()
        .WithInterface ("Pillo HID")
        .WithProduct ("Pillo")
        .WithManufacturer ("Hulan");
      InputSystem.RegisterLayout<PilloInputDevice> ();
      InputSystem.RegisterLayoutMatcher<PilloInputDevice> (pilloInputDeviceMatcher);
    }

    [MenuItem ("Pillo SDK/Editor Input/Add Virtual Device")]
    public static void EditorInputAddVirtualDevice () {
      // Adds a new device which matches the device description of the Pillo
      // Input Device.
      InputSystem.AddDevice (new InputDeviceDescription {
        interfaceName = "Pillo HID",
        product = "Pillo",
        manufacturer = "Hulan",
        // The serial indicated which Pillo is connected.
        serial = "VIR",
      });
    }

    [MenuItem ("Pillo SDK/Editor Input/Remove Virtual Device")]
    public static void EditorInputRemoveVirtualDevice () {
      // Removed a device which matches the device description of the Pillo
      // Input Device.
      var pilloInputDeviceDescription = new InputDeviceDescription {
        interfaceName = "Pillo HID",
        product = "Pillo",
        manufacturer = "Hulan",
        // The serial indicated which Pillo is connected.
        serial = "VIR"
      };
      // If a device matches the description, remove it.
      var device = InputSystem.devices.FirstOrDefault (
        device => device.description == pilloInputDeviceDescription);
      if (device != null) {
        InputSystem.RemoveDevice (device);
      }
    }
  }
}

#endif