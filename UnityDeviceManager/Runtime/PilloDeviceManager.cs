using AOT;
using Hulan.PilloSDK.DeviceManager.Core;
using UnityEngine;

namespace Hulan.PilloSDK.DeviceManager {
  /// <summary>
  /// The Pillo Device Manager manages the Native Plugin.
  /// </summary>
  public class PilloDeviceManager {
    /// <summary>
    /// Mono Callback for the Central Did Initialize event.
    /// </summary>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidInitialize))]
    static void OnCentralDidInitialize() => onCentralDidInitialize?.Invoke();

    /// <summary>
    /// Mono Callback for the Central Did Fail To Initialize event.
    /// </summary>
    /// <param name="message">The error message.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidFailToInitialize))]
    static void OnCentralDidFailToInitialize(string message) => onCentralDidFailToInitialize?.Invoke(message);

    /// <summary>
    /// Mono Callback for the Central Did Start Scanning event.
    /// </summary>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidStartScanning))]
    static void OnCentralDidStartScanning() => onCentralDidStartScanning?.Invoke();

    /// <summary>
    /// Mono Callback for the Central Did Stop Scanning event.
    /// </summary>
    [MonoPInvokeCallback(typeof(Delegates.OnCentralDidStopScanning))]
    static void OnCentralDidStopScanning() => onCentralDidStopScanning?.Invoke();

    /// <summary>
    /// Mono Callback for the Peripheral Did Connect event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidConnect))]
    static void OnPeripheralDidConnect(string identifier) => onPeripheralDidConnect?.Invoke(identifier);

    /// <summary>
    /// Mono Callback for the Peripheral Did Disconnect event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidDisconnect))]
    static void OnPeripheralDidDisconnect(string identifier) => onPeripheralDidDisconnect?.Invoke(identifier);

    /// <summary>
    /// Mono Callback for the Peripheral Did Fail To Connect event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralDidFailToConnect))]
    static void OnPeripheralDidFailToConnect(string identifier) => onPeripheralDidFailToConnect?.Invoke(identifier);

    /// <summary>
    /// Mono Callback for the Peripheral Battery Level Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="batteryLevel">The battery level of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralBatteryLevelDidChange))]
    static void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) => onPeripheralBatteryLevelDidChange?.Invoke(identifier, batteryLevel);

    /// <summary>
    /// Mono Callback for the Peripheral Pressure Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="pressure">The pressure of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralPressureDidChange))]
    static void OnPeripheralPressureDidChange(string identifier, int pressure) => onPeripheralPressureDidChange?.Invoke(identifier, pressure);

    /// <summary>
    /// Mono Callback for the Peripheral Charging State Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="chargingState">The charging state of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralChargingStateDidChange))]
    static void OnPeripheralChargingStateDidChange(string identifier, ChargingState chargingState) => onPeripheralChargingStateDidChange?.Invoke(identifier, chargingState);

    /// <summary>
    /// Mono Callback for the Peripheral Firmware Version Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="firmwareVersion">The firmware version of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralFirmwareVersionDidChange))]
    static void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) => onPeripheralFirmwareVersionDidChange?.Invoke(identifier, firmwareVersion);

    /// <summary>
    /// Mono Callback for the Peripheral Hardware Version Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="hardwareVersion">The hardware version of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralHardwareVersionDidChange))]
    static void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) => onPeripheralHardwareVersionDidChange?.Invoke(identifier, hardwareVersion);

    /// <summary>
    /// Mono Callback for the Peripheral Model Number Did Change event.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="modelNumber">The model number of the peripheral.</param>
    [MonoPInvokeCallback(typeof(Delegates.OnPeripheralModelNumberDidChange))]
    static void OnPeripheralModelNumberDidChange(string identifier, string modelNumber) => onPeripheralModelNumberDidChange?.Invoke(identifier, modelNumber);

    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    public static Delegates.OnCentralDidInitialize onCentralDidInitialize;

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    public static Delegates.OnCentralDidFailToInitialize onCentralDidFailToInitialize;

    /// <summary>
    /// Delegate will be invoked when the Central has started scanning.
    /// </summary>
    public static Delegates.OnCentralDidStartScanning onCentralDidStartScanning;

    /// <summary>
    /// Delegate will be invoked when the Central has stopped scanning.
    /// </summary>
    public static Delegates.OnCentralDidStopScanning onCentralDidStopScanning;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    public static Delegates.OnPeripheralDidConnect onPeripheralDidConnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did disconnect.
    /// </summary>
    public static Delegates.OnPeripheralDidDisconnect onPeripheralDidDisconnect;

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// </summary>
    public static Delegates.OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did 
    /// </summary>
    public static Delegates.OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    public static Delegates.OnPeripheralPressureDidChange onPeripheralPressureDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    public static Delegates.OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's firmware version did 
    /// change.
    /// </summary>
    public static Delegates.OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's hardware version did
    /// change.
    /// </summary>
    public static Delegates.OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange;

    /// <summary>
    /// Delegate will be invoked when the Peripheral's model number did change.
    /// </summary>
    public static Delegates.OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange;

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void RuntimeInitializeOnLoad() {
      PluginBridge.SetDelegates(OnCentralDidInitialize, OnCentralDidFailToInitialize, OnCentralDidStartScanning, OnCentralDidStopScanning, OnPeripheralDidConnect, OnPeripheralDidDisconnect, OnPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange, OnPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange);
      PluginBridge.StartService();
    }

    /// <summary>
    /// Cancels a Peripheral connection.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void CancelPeripheralConnection(string identifier) {
      PluginBridge.CancelPeripheralConnection(identifier);
    }

    /// <summary>
    /// Powers off a Peripheral.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void PowerOffPeripheral(string identifier) {
      PluginBridge.PowerOffPeripheral(identifier);
    }

    /// <summary>
    /// Forces the LED of a Peripheral to be turned off.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    /// <param name="enabled">Defines whether the LED should be forced off.</param>
    public static void ForcePeripheralLedOff(string identifier, bool enabled) {
      PluginBridge.ForcePeripheralLedOff(identifier, enabled);
    }

    /// <summary>
    /// Starts a Peripheral calibration.
    /// </summary>
    /// <param name="identifier">The identifier of the peripheral.</param>
    public static void StartPeripheralCalibration(string identifier) {
      PluginBridge.StartPeripheralCalibration(identifier);
    }

#if UNITY_ANDROID

    /// <summary>
    /// Handle permission results from Android (Android only).
    /// Call this from Unity's OnRequestPermissionsResult.
    /// </summary>
    /// <param name="requestCode">The request code from the permission request.</param>
    /// <param name="permissions">The permissions that were requested.</param>
    /// <param name="grantResults">The results of the permission requests.</param>
    public static void OnPermissionResult(int requestCode, string[] permissions, int[] grantResults) {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("onPermissionResult", requestCode, permissions, grantResults);
          }
        }
      }
    }
    
    /// <summary>
    /// Handle Bluetooth enable result from Android (Android only).
    /// Call this from Unity's OnActivityResult.
    /// </summary>
    /// <param name="enabled">Whether Bluetooth was enabled.</param>
    public static void OnBluetoothEnableResult(bool enabled) {
      using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
        using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
          using (AndroidJavaClass deviceManagerClass = new AndroidJavaClass("com.hulan.devicemanager.PilloDeviceManagerBridge")) {
            deviceManagerClass.CallStatic("onBluetoothEnableResult", enabled);
          }
        }
      }
    }
#endif
  }

#if UNITY_ANDROID
  /// <summary>
  /// Manages Android callbacks on the main thread to avoid threading issues
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

  /// <summary>
  /// AndroidJavaProxy classes for direct Java callback communication
  /// </summary>
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
      UnityEngine.Debug.Log($"[PressureProxy] Received pressure callback from Java: {identifier} = {pressure}");
      if (callback != null) {
        UnityEngine.Debug.Log($"[PressureProxy] Switching to main thread...");
        await Awaitable.MainThreadAsync();
        UnityEngine.Debug.Log($"[PressureProxy] Invoking C# callback: {identifier} = {pressure}");
        callback.Invoke(identifier, pressure);
        UnityEngine.Debug.Log($"[PressureProxy] C# callback completed");
      } else {
        UnityEngine.Debug.LogWarning($"[PressureProxy] Callback is NULL!");
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