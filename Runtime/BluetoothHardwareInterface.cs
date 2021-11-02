using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class BluetoothLEHardwareInterface {
  public enum CBCharacteristicProperties {
    CBCharacteristicPropertyBroadcast = 0x01,
    CBCharacteristicPropertyRead = 0x02,
    CBCharacteristicPropertyWriteWithoutResponse = 0x04,
    CBCharacteristicPropertyWrite = 0x08,
    CBCharacteristicPropertyNotify = 0x10,
    CBCharacteristicPropertyIndicate = 0x20,
    CBCharacteristicPropertyAuthenticatedSignedWrites = 0x40,
    CBCharacteristicPropertyExtendedProperties = 0x80,
    CBCharacteristicPropertyNotifyEncryptionRequired = 0x100,
    CBCharacteristicPropertyIndicateEncryptionRequired = 0x200,
  };

  public enum ScanMode {
    LowPower = 0,
    Balanced = 1,
    LowLatency = 2
  }

  public enum ConnectionPriority {
    LowPower = 0,
    Balanced = 1,
    High = 2,
  }

  public enum iOSProximity {
    Unknown = 0,
    Immediate = 1,
    Near = 2,
    Far = 3,
  }

  public struct iBeaconData {
    public string UUID;
    public int Major;
    public int Minor;
    public int RSSI;
    public int AndroidSignalPower;
    public iOSProximity iOSProximity;
  }

  public enum CBAttributePermissions {
    CBAttributePermissionsReadable = 0x01,
    CBAttributePermissionsWriteable = 0x02,
    CBAttributePermissionsReadEncryptionRequired = 0x04,
    CBAttributePermissionsWriteEncryptionRequired = 0x08,
  };

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLELog (string message);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEInitialize (bool asCentral, bool asPeripheral);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEDeInitialize ();

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEPauseMessages (bool isPaused);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEScanForPeripheralsWithServices (string serviceUUIDsString, bool allowDuplicates, bool rssiOnly, bool clearPeripheralList);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLERetrieveListOfPeripheralsWithServices (string serviceUUIDsString);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEStopScan ();

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEConnectToPeripheral (string name);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEDisconnectPeripheral (string name);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEReadCharacteristic (string name, string service, string characteristic);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEWriteCharacteristic (string name, string service, string characteristic, byte[] data, int length, bool withResponse);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLESubscribeCharacteristic (string name, string service, string characteristic);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEUnSubscribeCharacteristic (string name, string service, string characteristic);

  [DllImport ("__Internal")]
  private static extern void _iOSBluetoothLEDisconnectAll ();

  private static BluetoothDeviceScript bluetoothDeviceScript;

  public static void Log (string message) {
    if (!Application.isEditor) {
      _iOSBluetoothLELog (message);
    }
  }

  public static BluetoothDeviceScript Initialize (bool asCentral, bool asPeripheral, Action action, Action<string> errorAction) {
    bluetoothDeviceScript = null;

    GameObject bluetoothLEReceiver = GameObject.Find ("BluetoothLEReceiver");
    if (bluetoothLEReceiver == null)
      bluetoothLEReceiver = new GameObject ("BluetoothLEReceiver");

    if (bluetoothLEReceiver != null) {
      bluetoothDeviceScript = bluetoothLEReceiver.GetComponent<BluetoothDeviceScript> ();
      if (bluetoothDeviceScript == null)
        bluetoothDeviceScript = bluetoothLEReceiver.AddComponent<BluetoothDeviceScript> ();

      if (bluetoothDeviceScript != null) {
        bluetoothDeviceScript.InitializedAction = action;
        bluetoothDeviceScript.ErrorAction = errorAction;
      }
    }

    GameObject.DontDestroyOnLoad (bluetoothLEReceiver);

    if (Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.SendMessage ("OnBluetoothMessage", "Initialized");
    } else {
      _iOSBluetoothLEInitialize (asCentral, asPeripheral);
    }
    return bluetoothDeviceScript;
  }

  public static void DeInitialize (Action action) {
    if (bluetoothDeviceScript != null)
      bluetoothDeviceScript.DeinitializedAction = action;

    if (Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.SendMessage ("OnBluetoothMessage", "DeInitialized");
    } else {
      _iOSBluetoothLEDeInitialize ();
    }
  }

  public static void FinishDeInitialize () {
    GameObject bluetoothLEReceiver = GameObject.Find ("BluetoothLEReceiver");
    if (bluetoothLEReceiver != null)
      GameObject.Destroy (bluetoothLEReceiver);
  }

  public static void BluetoothEnable (bool enable) {
    if (!Application.isEditor) {
      //_iOSBluetoothLELog (message);
    }
  }

  public static void BluetoothScanMode (ScanMode scanMode) {
    if (!Application.isEditor) {
    }
  }

  public static void BluetoothConnectionPriority (ConnectionPriority connectionPriority) {
    if (!Application.isEditor) {
    }
  }

  public static void PauseMessages (bool isPaused) {
    if (!Application.isEditor) {
      _iOSBluetoothLEPauseMessages (isPaused);
    }
  }

  // scanning for beacons requires that you know the Proximity UUID
  public static void ScanForBeacons (string[] proximityUUIDs, Action<iBeaconData> actionBeaconResponse) {
    if (proximityUUIDs != null && proximityUUIDs.Length >= 0) {
      if (!Application.isEditor) {
        if (bluetoothDeviceScript != null)
          bluetoothDeviceScript.DiscoveredBeaconAction = actionBeaconResponse;

        string proximityUUIDsString = null;

        if (proximityUUIDs != null && proximityUUIDs.Length > 0) {
          proximityUUIDsString = "";

          foreach (string proximityUUID in proximityUUIDs)
            proximityUUIDsString += proximityUUID + "|";

          proximityUUIDsString = proximityUUIDsString.Substring (0, proximityUUIDsString.Length - 1);
        }
      }
    }
  }

  public static void RequestMtu (string name, int mtu, Action<string, int> action) {
    if (bluetoothDeviceScript != null) {
      bluetoothDeviceScript.RequestMtuAction = action;
    }
    if (mtu > 180)
      mtu = 180;
    // HACK disabled this to make it work
    // _iOSBluetoothLERequestMtu (name, mtu);
  }

  public static void ScanForPeripheralsWithServices (string[] serviceUUIDs, Action<string, string> action, Action<string, string, int, byte[]> actionAdvertisingInfo = null, bool rssiOnly = false, bool clearPeripheralList = true, int recordType = 0xFF) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        bluetoothDeviceScript.DiscoveredPeripheralAction = action;
        bluetoothDeviceScript.DiscoveredPeripheralWithAdvertisingInfoAction = actionAdvertisingInfo;

        if (bluetoothDeviceScript.DiscoveredDeviceList != null)
          bluetoothDeviceScript.DiscoveredDeviceList.Clear ();
      }

      string serviceUUIDsString = null;

      if (serviceUUIDs != null && serviceUUIDs.Length > 0) {
        serviceUUIDsString = "";

        foreach (string serviceUUID in serviceUUIDs)
          serviceUUIDsString += serviceUUID + "|";

        serviceUUIDsString = serviceUUIDsString.Substring (0, serviceUUIDsString.Length - 1);
      }

      _iOSBluetoothLEScanForPeripheralsWithServices (serviceUUIDsString, (actionAdvertisingInfo != null), rssiOnly, clearPeripheralList);
    }
  }

  public static void RetrieveListOfPeripheralsWithServices (string[] serviceUUIDs, Action<string, string> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        bluetoothDeviceScript.RetrievedConnectedPeripheralAction = action;

        if (bluetoothDeviceScript.DiscoveredDeviceList != null)
          bluetoothDeviceScript.DiscoveredDeviceList.Clear ();
      }

      string serviceUUIDsString = serviceUUIDs.Length > 0 ? "" : null;

      foreach (string serviceUUID in serviceUUIDs)
        serviceUUIDsString += serviceUUID + "|";

      // strip the last delimeter
      serviceUUIDsString = serviceUUIDsString.Substring (0, serviceUUIDsString.Length - 1);

      _iOSBluetoothLERetrieveListOfPeripheralsWithServices (serviceUUIDsString);
    }
  }

  public static void StopScan () {
    if (!Application.isEditor) {
      _iOSBluetoothLEStopScan ();
    }
  }

  public static void StopBeaconScan () {
    if (!Application.isEditor) {
    }
  }

  public static void DisconnectAll () {
    if (!Application.isEditor) {
      _iOSBluetoothLEDisconnectAll ();
    }
  }

  public static void ConnectToPeripheral (string name, Action<string> connectAction, Action<string, string> serviceAction, Action<string, string, string> characteristicAction, Action<string> disconnectAction = null) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        bluetoothDeviceScript.ConnectedPeripheralAction = connectAction;
        bluetoothDeviceScript.DiscoveredServiceAction = serviceAction;
        bluetoothDeviceScript.DiscoveredCharacteristicAction = characteristicAction;
        bluetoothDeviceScript.ConnectedDisconnectPeripheralAction = disconnectAction;
      }
      _iOSBluetoothLEConnectToPeripheral (name);
    }
  }

  public static void DisconnectPeripheral (string name, Action<string> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.DisconnectedPeripheralAction = action;
      _iOSBluetoothLEDisconnectPeripheral (name);
    }
  }

  public static void ReadCharacteristic (string name, string service, string characteristic, Action<string, byte[]> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        if (!bluetoothDeviceScript.DidUpdateCharacteristicValueAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateCharacteristicValueAction[name] = new Dictionary<string, Action<string, byte[]>> ();
        bluetoothDeviceScript.DidUpdateCharacteristicValueAction[name][characteristic] = action;
      }
      _iOSBluetoothLEReadCharacteristic (name, service, characteristic);
    }
  }

  public static void WriteCharacteristic (string name, string service, string characteristic, byte[] data, int length, bool withResponse, Action<string> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.DidWriteCharacteristicAction = action;
      _iOSBluetoothLEWriteCharacteristic (name, service, characteristic, data, length, withResponse);
    }
  }

  public static void SubscribeCharacteristic (string name, string service, string characteristic, Action<string> notificationAction, Action<string, byte[]> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        name = name.ToUpper ();
        service = service.ToUpper ();
        characteristic = characteristic.ToUpper ();

        if (!bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction[name] = new Dictionary<string, Action<string>> ();
        bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction[name][characteristic] = notificationAction;

        if (!bluetoothDeviceScript.DidUpdateCharacteristicValueAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateCharacteristicValueAction[name] = new Dictionary<string, Action<string, byte[]>> ();
        bluetoothDeviceScript.DidUpdateCharacteristicValueAction[name][characteristic] = action;
      }

      _iOSBluetoothLESubscribeCharacteristic (name, service, characteristic);
    }
  }

  public static void SubscribeCharacteristicWithDeviceAddress (string name, string service, string characteristic, Action<string, string> notificationAction, Action<string, string, byte[]> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        name = name.ToUpper ();
        service = service.ToUpper ();
        characteristic = characteristic.ToUpper ();

        if (!bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicWithDeviceAddressAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicWithDeviceAddressAction[name] = new Dictionary<string, Action<string, string>> ();
        bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicWithDeviceAddressAction[name][characteristic] = notificationAction;

        if (!bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction[name] = new Dictionary<string, Action<string>> ();
        bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction[name][characteristic] = null;

        if (!bluetoothDeviceScript.DidUpdateCharacteristicValueWithDeviceAddressAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateCharacteristicValueWithDeviceAddressAction[name] = new Dictionary<string, Action<string, string, byte[]>> ();
        bluetoothDeviceScript.DidUpdateCharacteristicValueWithDeviceAddressAction[name][characteristic] = action;
      }
      _iOSBluetoothLESubscribeCharacteristic (name, service, characteristic);
    }
  }

  public static void UnSubscribeCharacteristic (string name, string service, string characteristic, Action<string> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null) {
        name = name.ToUpper ();
        service = service.ToUpper ();
        characteristic = characteristic.ToUpper ();

        if (!bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicWithDeviceAddressAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicWithDeviceAddressAction[name] = new Dictionary<string, Action<string, string>> ();
        bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicWithDeviceAddressAction[name][characteristic] = null;

        if (!bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction.ContainsKey (name))
          bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction[name] = new Dictionary<string, Action<string>> ();
        bluetoothDeviceScript.DidUpdateNotificationStateForCharacteristicAction[name][characteristic] = action;
      }

      _iOSBluetoothLEUnSubscribeCharacteristic (name, service, characteristic);
    }
  }

  public static void PeripheralName (string newName) {
    if (!Application.isEditor) {
    }
  }

  public static void CreateService (string uuid, bool primary, Action<string> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.ServiceAddedAction = action;
    }
  }

  public static void RemoveService (string uuid) {
    if (!Application.isEditor) {
    }
  }

  public static void RemoveServices () {
    if (!Application.isEditor) {
    }
  }

  public static void CreateCharacteristic (string uuid, CBCharacteristicProperties properties, CBAttributePermissions permissions, byte[] data, int length, Action<string, byte[]> action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.PeripheralReceivedWriteDataAction = action;
    }
  }

  public static void RemoveCharacteristic (string uuid) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.PeripheralReceivedWriteDataAction = null;
    }
  }

  public static void RemoveCharacteristics () {
    if (!Application.isEditor) {
    }
  }

  public static void StartAdvertising (Action action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.StartedAdvertisingAction = action;
    }
  }

  public static void StopAdvertising (Action action) {
    if (!Application.isEditor) {
      if (bluetoothDeviceScript != null)
        bluetoothDeviceScript.StoppedAdvertisingAction = action;
    }
  }

  public static void UpdateCharacteristicValue (string uuid, byte[] data, int length) {
    if (!Application.isEditor) {
    }
  }

  public static string FullUUID (string uuid) {
    if (uuid.Length == 4)
      return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
    return uuid;
  }
}