#if PILLO_SDK_UNITY_INPUTSYSTEM

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;

namespace Hulan.Pillo.SDK.InputSystemDevice {

  // InputControlLayoutAttribute attribute is only necessary if you want to 
  // override default behavior that occurs when registering your device as a 
  // layout. The most common use of InputControlLayoutAttribute is to direct the 
  // system to a custom "state struct" through the `stateType` property.
  [InputControlLayout (displayName = "Pillo", stateType = typeof (PilloInputDeviceState))]
  public class PilloInputDevice : InputDevice, IInputUpdateCallbackReceiver {

    // public AxisControl pressure { get; private set; }

    // The Input System calls this method after it constructs the Device,
    // but before it adds the device to the system. Do any last-minute setup
    // here.
    protected override void FinishSetup () {
      base.FinishSetup ();

      // NOTE: The Input System creates the Controls automatically.
      //       This is why don't do `new` here but rather just look
      //       the Controls up.
      // pressure = GetChildControl<AxisControl> ("pressure");
    }

    public void OnUpdate () {
      // In practice, this would read out data from an external
      // API. This example uses some empty input.
      var state = new PilloInputDeviceState ();
      state.pressure = (ushort)Random.Range (0, 255);
      InputSystem.QueueStateEvent (this, state);
    }
  }
}

#endif