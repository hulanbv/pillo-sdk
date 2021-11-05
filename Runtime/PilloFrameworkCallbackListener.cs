using UnityEngine;

namespace Hulan.Pillo.SDK {
  public class PilloFrameworkCallbackListener : MonoBehaviour {
    public IPilloFrameworkDelegate delegateObject;

    public void OnDidInitialize () =>
      this.delegateObject.OnDidInitialize ();
    public void OnBluetoothNotAvailable () =>
      this.delegateObject.OnBluetoothNotAvailable ();
    public void OnConnectionSuccessful (string parameter) =>
      this.delegateObject.OnConnectionSuccessful (parameter);
    public void OnConnectionFailed (string parameter) =>
      this.delegateObject.OnConnectionFailed (parameter);
    public void OnBatteryLevelDidChange (string parameter) =>
      this.delegateObject.OnBatteryLevelDidChange (int.Parse (parameter));
    public void OnPressureDidChange (string parameter) =>
      this.delegateObject.OnPressureDidChange (int.Parse (parameter));
  }
}