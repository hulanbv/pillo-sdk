using UnityEngine;

namespace Hulan.PilloSDK.DeviceManager.Core {
#if UNITY_ANDROID
  /// <summary>
  /// Android bridge, including Java proxies and main-thread dispatch.
  /// </summary>
  public class AndroidPluginBridge : IPluginBridge {
    public void StartService() {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("startService");
          }
        }
      }
    }

    public void StopService() {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("stopService");
          }
        }
      }
    }

    public void SetDelegates(Delegates.OnCentralDidInitialize onCentralDidInitialize, Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize, Delegates.OnCentralDidStartScanning onCentralDidStartScanning, Delegates.OnCentralDidStopScanning onCentralDidStopScanning, Delegates.OnPeripheralDidConnect onPeripheralDidConnect, Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
      AndroidCallbackManager.SetDelegates(onCentralDidInitialize, onCentralDidFailToInitialize, onCentralDidStartScanning, onCentralDidStopScanning, onPeripheralDidConnect, onPeripheralDidDisconnect, onPeripheralDidFailToConnect, onPeripheralBatteryLevelDidChange, onPeripheralPressureDidChange, onPeripheralChargingStateDidChange, onPeripheralFirmwareVersionDidChange, onPeripheralHardwareVersionDidChange, onPeripheralModelNumberDidChange);
    }

    public void CancelPeripheralConnection(string identifier) {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("cancelPeripheralConnection", identifier);
          }
        }
      }
    }

    public void PowerOffPeripheral(string identifier) {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("powerOffPeripheral", identifier);
          }
        }
      }
    }

    public void ForcePeripheralLedOff(string identifier, bool enabled) {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("forcePeripheralLedOff", identifier, enabled);
          }
        }
      }
    }

    public void StartPeripheralCalibration(string identifier) {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("calibratePeripheral", identifier);
          }
        }
      }
    }
  }

  /// <summary>
  /// Manages Android callbacks on the main thread to avoid threading issues.
  /// </summary>
  public class AndroidCallbackManager : MonoBehaviour {
    static AndroidCallbackManager instance;
    static bool delegatesSet = false;

    // Stored delegates
    static Delegates.OnCentralDidInitialize onCentralDidInitialize;
    static Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize;
    static Delegates.OnCentralDidStartScanning onCentralDidStartScanning;
    static Delegates.OnCentralDidStopScanning onCentralDidStopScanning;
    static Delegates.OnPeripheralDidConnect onPeripheralDidConnect;
    static Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect;
    static Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;
    static Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;
    static Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange;
    static Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange;
    static Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange;
    static Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange;
    static Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange;

    public static void SetDelegates(Delegates.OnCentralDidInitialize centralDidInitialize, Delegates.OnCentralDidFailToInitialize centralDidFailToInitialize, Delegates.OnCentralDidStartScanning centralDidStartScanning, Delegates.OnCentralDidStopScanning centralDidStopScanning, Delegates.OnPeripheralDidConnect peripheralDidConnect, Delegates.OnPeripheralDidDisconnect peripheralDidDisconnect, Delegates.OnPeripheralDidFailToConnect peripheralDidFailToConnect, Delegates.OnPeripheralBatteryLevelDidChange peripheralBatteryLevelDidChange, Delegates.OnPeripheralPressureDidChange peripheralPressureDidChange, Delegates.OnPeripheralChargingStateDidChange peripheralChargingStateDidChange, Delegates.OnPeripheralFirmwareVersionDidChange peripheralFirmwareVersionDidChange, Delegates.OnPeripheralHardwareVersionDidChange peripheralHardwareVersionDidChange, Delegates.OnPeripheralModelNumberDidChange peripheralModelNumberDidChange) {
      onCentralDidInitialize = centralDidInitialize;
      onCentralDidFailToInitialize = centralDidFailToInitialize;
      onCentralDidStartScanning = centralDidStartScanning;
      onCentralDidStopScanning = centralDidStopScanning;
      onPeripheralDidConnect = peripheralDidConnect;
      onPeripheralDidDisconnect = peripheralDidDisconnect;
      onPeripheralDidFailToConnect = peripheralDidFailToConnect;
      onPeripheralBatteryLevelDidChange = peripheralBatteryLevelDidChange;
      onPeripheralPressureDidChange = peripheralPressureDidChange;
      onPeripheralChargingStateDidChange = peripheralChargingStateDidChange;
      onPeripheralFirmwareVersionDidChange = peripheralFirmwareVersionDidChange;
      onPeripheralHardwareVersionDidChange = peripheralHardwareVersionDidChange;
      onPeripheralModelNumberDidChange = peripheralModelNumberDidChange;

      if (instance == null) {
        var go = new GameObject("AndroidCallbackManager");
        instance = go.AddComponent<AndroidCallbackManager>();
        Object.DontDestroyOnLoad(go);
      }
    }

    void Awake() {
      if (instance == null) {
        instance = this;
        Object.DontDestroyOnLoad(gameObject);
      } else if (instance != this) {
        Destroy(gameObject);
      }
    }

    void Start() {
      if (!delegatesSet) {
        SetJavaDelegates();
        delegatesSet = true;
      }
    }

    void SetJavaDelegates() {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManager")) {
            deviceManagerClass.CallStatic("setDelegates",
              new CentralInitializedProxy(onCentralDidInitialize),
              new CentralFailToInitializeProxy(onCentralDidFailToInitialize),
              new CentralStartScanningProxy(onCentralDidStartScanning),
              new CentralStopScanningProxy(onCentralDidStopScanning),
              new PeripheralConnectProxy(onPeripheralDidConnect),
              new PeripheralDisconnectProxy(onPeripheralDidDisconnect),
              new PeripheralFailToConnectProxy(onPeripheralDidFailToConnect),
              new BatteryLevelProxy(onPeripheralBatteryLevelDidChange),
              new PressureProxy(onPeripheralPressureDidChange),
              new ChargingStateProxy(onPeripheralChargingStateDidChange),
              new FirmwareVersionProxy(onPeripheralFirmwareVersionDidChange),
              new HardwareVersionProxy(onPeripheralHardwareVersionDidChange),
              new ModelNumberProxy(onPeripheralModelNumberDidChange));
          }
        }
      }
    }
  }

  // AndroidJavaProxy classes for direct Java callback communication
  public class CentralInitializedProxy : AndroidJavaProxy {
    private Delegates.OnCentralDidInitialize callback;
    public CentralInitializedProxy(Delegates.OnCentralDidInitialize callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnCentralDidInitialize") {
      this.callback = callback;
    }
    public async void onCentralDidInitialize() {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke();
      }
    }
  }

  public class CentralFailToInitializeProxy : AndroidJavaProxy {
    private Delegates.OnCentralDidFailToInitialize callback;
    public CentralFailToInitializeProxy(Delegates.OnCentralDidFailToInitialize callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnCentralDidFailToInitialize") {
      this.callback = callback;
    }
    public async void onCentralDidFailToInitialize(string message) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(message);
      }
    }
  }

  public class CentralStartScanningProxy : AndroidJavaProxy {
    private Delegates.OnCentralDidStartScanning callback;
    public CentralStartScanningProxy(Delegates.OnCentralDidStartScanning callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnCentralDidStartScanning") {
      this.callback = callback;
    }
    public async void onCentralDidStartScanning() {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke();
      }
    }
  }

  public class CentralStopScanningProxy : AndroidJavaProxy {
    private Delegates.OnCentralDidStopScanning callback;
    public CentralStopScanningProxy(Delegates.OnCentralDidStopScanning callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnCentralDidStopScanning") {
      this.callback = callback;
    }
    public async void onCentralDidStopScanning() {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke();
      }
    }
  }

  public class PeripheralConnectProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralDidConnect callback;
    public PeripheralConnectProxy(Delegates.OnPeripheralDidConnect callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralDidConnect") {
      this.callback = callback;
    }
    public async void onPeripheralDidConnect(string identifier) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier);
      }
    }
  }

  public class PeripheralDisconnectProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralDidDisconnect callback;
    public PeripheralDisconnectProxy(Delegates.OnPeripheralDidDisconnect callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralDidDisconnect") {
      this.callback = callback;
    }
    public async void onPeripheralDidDisconnect(string identifier) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier);
      }
    }
  }

  public class PeripheralFailToConnectProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralDidFailToConnect callback;
    public PeripheralFailToConnectProxy(Delegates.OnPeripheralDidFailToConnect callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralDidFailToConnect") {
      this.callback = callback;
    }
    public async void onPeripheralDidFailToConnect(string identifier) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier);
      }
    }
  }

  public class BatteryLevelProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralBatteryLevelDidChange callback;
    public BatteryLevelProxy(Delegates.OnPeripheralBatteryLevelDidChange callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralBatteryLevelDidChange") {
      this.callback = callback;
    }
    public async void onPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier, batteryLevel);
      }
    }
  }

  public class PressureProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralPressureDidChange callback;
    public PressureProxy(Delegates.OnPeripheralPressureDidChange callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralPressureDidChange") {
      this.callback = callback;
    }
    public async void onPeripheralPressureDidChange(string identifier, int pressure) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier, pressure);
      }
    }
  }

  public class ChargingStateProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralChargingStateDidChange callback;
    public ChargingStateProxy(Delegates.OnPeripheralChargingStateDidChange callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralChargingStateDidChange") {
      this.callback = callback;
    }
    public async void onPeripheralChargingStateDidChange(string identifier, int chargingState) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier, (ChargingState)chargingState);
      }
    }
  }

  public class FirmwareVersionProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralFirmwareVersionDidChange callback;
    public FirmwareVersionProxy(Delegates.OnPeripheralFirmwareVersionDidChange callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralFirmwareVersionDidChange") {
      this.callback = callback;
    }
    public async void onPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier, firmwareVersion);
      }
    }
  }

  public class HardwareVersionProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralHardwareVersionDidChange callback;
    public HardwareVersionProxy(Delegates.OnPeripheralHardwareVersionDidChange callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralHardwareVersionDidChange") {
      this.callback = callback;
    }
    public async void onPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier, hardwareVersion);
      }
    }
  }

  public class ModelNumberProxy : AndroidJavaProxy {
    private Delegates.OnPeripheralModelNumberDidChange callback;
    public ModelNumberProxy(Delegates.OnPeripheralModelNumberDidChange callback) : base("com.hulan.devicemanager.PilloDeviceManager$OnPeripheralModelNumberDidChange") {
      this.callback = callback;
    }
    public async void onPeripheralModelNumberDidChange(string identifier, string modelNumber) {
      if (callback != null) {
        await Awaitable.MainThreadAsync();
        callback.Invoke(identifier, modelNumber);
      }
    }
  }
#endif
}


