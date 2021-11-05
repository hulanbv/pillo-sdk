using System.Runtime.InteropServices;
using UnityEngine;

namespace Hulan.Pillo.SDK {
  public interface IPilloFrameworkDelegate {
    void OnDidInitialize ();
    void OnBluetoothNotAvailable ();
    void OnConnectionSuccessful (string peripheralIdentifier);
    void OnConnectionFailed (string peripheralIdentifier);
    void OnBatteryLevelDidChange (int batteryLevel);
    void OnPressureDidChange (int pressure);
  }
}