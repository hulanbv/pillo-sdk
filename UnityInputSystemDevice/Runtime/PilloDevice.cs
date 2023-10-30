using System.Runtime.Serialization;
using Hulan.PilloSDK.DeviceManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace Hulan.PilloSDK.InputSystemDevice {
  public struct PilloDeviceState : IInputStateTypeInfo {
    public readonly FourCC format => new('P', 'I', 'L', 'O');

    [InputControl(name = "pressure", layout = "Axis")]
    public int pressure;
  }

  [InputControlLayout(stateType = typeof(PilloDeviceState), displayName = "Pillo")]
  public class PilloDevice : InputDevice {
    public string Id { get; private set; }

    public AxisControl Pressure { get; private set; }

    protected override void FinishSetup() {
      base.FinishSetup();
      Pressure = GetChildControl<AxisControl>("pressure");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializeOnLoad() {
      PilloDeviceManager.onPeripheralDidConnect += (string id) => {
        var device = InputSystem.AddDevice<PilloDevice>();
        device.Id = id;
      };
      PilloDeviceManager.onPeripheralDidDisconnect += (string id) => {
        foreach (var device in InputSystem.devices) {
          if (device is not PilloDevice pillodevice || pillodevice.Id != id) {
            continue;
          }
          InputSystem.RemoveDevice(device);
        }
      };
      PilloDeviceManager.onPeripheralPressureDidChange += (string id, int pressure) => {
        foreach (var device in InputSystem.devices) {
          if (device is not PilloDevice pillodevice || pillodevice.Id != id) {
            continue;
          }
          InputSystem.QueueStateEvent(pillodevice, new PilloDeviceState { pressure = pressure });
        }
      };
    }
  }
}