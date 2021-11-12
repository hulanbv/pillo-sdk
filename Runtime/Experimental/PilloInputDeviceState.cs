#if PILLO_SDK_UNITY_INPUTSYSTEM

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace Hulan.Pillo.SDK.InputSystemDevice {

  // The state struct describes the memory format used by a device. Each device 
  // can receive and store memory in its custom format. InputControls are then 
  // connected the individual pieces of memory and read out values from them.
  public struct PilloInputDeviceState : IInputStateTypeInfo {

    // You must tag every state with a FourCC code for type checking. The
    // characters can be anything. Choose something that allows you to easily
    // recognize memory that belongs to your own Device.
    public FourCC format => new FourCC ('P', 'I', 'L', '0');

    // Create a floating-point axis which contains the pressure which should
    // contain a value from 0 to 255.
    // TODO for some reason this doesn't appear in the Input System
    [InputControl (layout = "axis", name = "Pressure")]
    public ushort pressure;
  }
}

#endif