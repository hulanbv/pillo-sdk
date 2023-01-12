![readme banner](https://github.com/hulanbv/pillo-sdk-mono/blob/master/.github/wiki/readme-banner.png?raw=true)

The Pillo SDK Mono Repository contains the source for various Pillo SDK packages and libraries. Everything you'll need to get working on and working with the Pillo SDK is included in this repository. Visit the —

- [Unity Pillo SDK Framework](#unity-pillo-sdk-framework) documention for more information on the core functionality for interacting with the Pillo Hardware.
- [Unity Pillo SDK Input System](#unity-pillo-sdk-input-system) documention for more information on the functionality for interacting with the Pillo Hardware via the Unity Input System.
- [Bluetooth Attribute Profile](#bluetooth-attribute-profile) documention for more information on the Bluetooth Attribute Profile used by the Pillo Hardware.
- [Development and Contribution](#development-and-contribution) documention for more information on how to contribute to the Pillo SDK Mono Repository.

The Pillo SDK Mono Repository is licensed by Hulan BV where all rights are reserved.

<br/><br/><br/>

## Unity Pillo SDK Framework

The Pillo SDK Framework provides the core functionality for interacting with the Pillo Hardware.

### Compatibility

The Pillo SDK Framework is compatible with the following platforms:

- Unity Engine for AppleTV running tvOS 9.0 or later
- Unity Engine for Apple iPhone running iOS 5.0 or later
- Unity Engine for Apple iPad running iPadOS 5.0 or later

### Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory.

```json
"nl.hulan.pillo-sdk.framework.unity": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnityFramework"
```

<br/><br/><br/>

## Unity Pillo SDK Input System

The Pillo SDK Input System provides the functionality for interacting with the Pillo Hardware via the Unity Input System. This package requires the Pillo SDK Framework to be installed.

### Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory. _Note that the Pillo SDK Input System requires the [Pillo SDK Framework](#unity-pillo-sdk-framework) to be installed as well._

```json
"nl.hulan.pillo-sdk.input-system.unity": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnityInputSystem"
```

### Documentation

The Pillo SDK Input System is based around `PilloInputDevice`s. These devices are created when a Pillo is connected to the device. The `PilloInputDevice` exposes a set of controls that can be used to interact with the Pillo Hardware, and a set of properties that can be used to retrieve information about the Pillo Hardware. The `PilloInputSystem` can be used to subscribe to specific events as well as retrieve a list of all connected Pillo devices.

#### Pillo Input System Delegates

##### Pillo Input Device Did Connect

The `OnPilloInputDeviceDidConnect` delegate is called when a Pillo is connected to the device. A new `PilloInputDevice` is created and passed to the delegate.

```csharp
public delegate void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice);
```

```csharp
PilloInputSystem.onPilloInputDeviceDidConnect += (PilloInputDevice pilloInputDevice) => {
    Debug.Log($"Pillo {pilloInputDevice.identifier} connected.");
};
```

##### Pillo Input Device Did Disconnect

The `OnPilloInputDeviceDidDisconnect` delegate is called when a Pillo is disconnected from the device. The `PilloInputDevice` is passed to the delegate.

```csharp
public delegate void OnPilloInputDeviceDidDisconnect (PilloInputDevice pilloInputDevice);
```

```csharp
PilloInputSystem.onPilloInputDeviceDidDisconnect += (PilloInputDevice pilloInputDevice) => {
    Debug.Log($"Pillo {pilloInputDevice.identifier} disconnected.");
};
```

##### Pillo Input Device Did Fail To Connect

The `OnPilloInputDeviceDidFailToConnect` delegate is called when a Pillo fails to connect to the device.

```csharp
public delegate void OnPilloInputDeviceDidFailToConnect ();
```

```csharp
PilloInputSystem.onPilloInputDeviceDidFailToConnect += () => {
    Debug.Log("Pillo failed to connect.");
};
```

##### Pillo Input Device State Did Change

The `OnPilloInputDeviceStateDidChange` delegate is called when the state of a Pillo changes. The `PilloInputDevice` which derives from the `PilloInputDeviceState` is passed to the delegate.

```csharp
public delegate void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice);
```

```csharp
PilloInputSystem.onPilloInputDeviceStateDidChange += (PilloInputDevice pilloInputDevice) => {
    Debug.Log($"Pillo {pilloInputDevice.identifier} state changed.");
};
```

#### Pillo Input System Methods

##### Reset Pillo Input Device Player Indexes

The `ResetPilloInputDevicePlayerIndexes` method can be used to reset the player indexes of all connected Pillo devices.

```csharp
public static void ResetPilloInputDevicePlayerIndexes ()
```

#### Pillo Input System Properties

##### Pillo Input Devices

The `pilloInputDevices` property can be used to retrieve a list of all connected Pillo devices.

```csharp
public static List<PilloInputDevice> pilloInputDevices { get; }
```

##### Pillo Input Device Count

The `pilloInputDeviceCount` property can be used to retrieve the number of connected Pillo devices.

```csharp
public static int pilloInputDeviceCount { get; }
```

#### Pillo Input Device State Properties

##### Pillo Input Device State Identifier

The Pillo Input Device's unique identifier assigned by the Pillo Bluetooth peripheral.

```csharp
public string identifier { get; }
```

##### Pillo Input Device State Player Index

Internally assigned player indexes. These indexes are assigned automatically and are used to identify the player that the device belongs to. If a device disconnects, the player index is reassigned.

```csharp
public int playerIndex { get; }
```

##### Pillo Input Device State Pressure

The Pillo Input Device's current pressure value.

```csharp
public int pressure { get; }
```

The pressure value is a value between `0` and `1024` where `0` is the lowest pressure and `1` is the highest pressure.

##### Pillo Input Device State Battery Level

The Pillo Input Device's current battery level.

```csharp
public int batteryLevel { get; }
```

The battery level is a value between `0` and `100` where `0` is the lowest battery level and `100` is the highest battery level.

##### Pillo Input Device State Charge State

The Pillo Input Device's current charge state.

```csharp
public PilloInputDeviceChargeState chargeState { get; }
```

- `-1` `UNKNOWN` - The charge state is unknown.
- `0` `PRE_CHARGE` - The Pillo is preparing for charging.
- `1` `FAST_CHARGE` - The Pillo is charging.
- `2` `CHARGE_DONE` - The Pillo is fully charged.
- `3` `SLEEP_MODE` - The Pillo is not charging and is in sleep mode.

#### Pillo Input Device Methods

##### Power Off the Pillo Input Device

The `PowerOff` method can be used to power off the Pillo Hardware.

```csharp
public void PowerOff ();
```

##### Start Calibrating the Pillo Input Device

The `StartCalibrating` method can be used to start calibrating the Pillo Hardware.

```csharp
public void StartCalibration ();
```

<br/><br/><br/>

## Bluetooth Attribute Profile

The following Bluetooth Low Energy Generic Attribute Profile Definitions are based on version `0.4` of the `VT_HULPIL_20220308_BLE_GATT_PROFILES` protocol.

### Generic Attribute Services

#### Battery GATT

| Type           | UUID   | Name            | Definition            | Description                                                          |
| -------------- | ------ | --------------- | --------------------- | -------------------------------------------------------------------- |
| Service        | `180f` | Battery Service |                       | The Battery Service exposes the battery level of the Pillo Hardware. |
| Characteristic | `2a19` | Battery Level   | 1 Byte, Read & Notify | The Battery Level characteristic exposes the battery level.          |

The battery level is represented as a value between `0` and `100` where `0` is the lowest battery level and `100` is the highest battery level.

#### Device Information GATT

| Type           | UUID   | Name                       | Definition   | Description                                                                 |
| -------------- | ------ | -------------------------- | ------------ | --------------------------------------------------------------------------- |
| Service        | `180a` | Device Information Service |              | The Device Information Service exposes the device information of the Pillo. |
| Characteristic | `2a29` | Manufacturer Name String   | String, Read | The Manufacturer Name String characteristic exposes the manufacturer name.  |
| Characteristic | `2a24` | Model Number String        | String, Read | The Model Number String characteristic exposes the model number.            |
| Characteristic | `2a27` | Hardware Revision String   | String, Read | The Hardware Revision String characteristic exposes the hardware revision.  |
| Characteristic | `2a26` | Firmware Revision String   | String, Read | The Firmware Revision String characteristic exposes the firmware revision.  |

#### Pressure GATT

| Type           | UUID                                   | Name                    | Definition            | Description                                                      |
| -------------- | -------------------------------------- | ----------------------- | --------------------- | ---------------------------------------------------------------- |
| Service        | `579ba43d-a351-463d-92c7-911ec1b54e35` | Pressure Service        |                       | The Pressure Service exposes the pressure of the Pillo Hardware. |
| Characteristic | `1470ca75-5d7e-4e16-a70d-d1476e8d0c6f` | Pressure Characteristic | 1 byte, Read & Notify | The Pressure Characteristic exposes the pressure.                |

The pressure is represented a value between `0` and `1024` where `0` is the lowest pressure and `1024` is the highest pressure.

#### Charging GATT

| Type           | UUID                                   | Name                          | Definition            | Description                                                    |
| -------------- | -------------------------------------- | ----------------------------- | --------------------- | -------------------------------------------------------------- |
| Service        | `579ba43d-a351-463d-92c7-911ec1b54e35` | Charging Service              |                       | The Charging Service exposes the charging status of the Pillo. |
| Characteristic | `22feb891-0057-4a3e-af5b-ec769849077c` | Charging State Characteristic | 1 byte, Read & Notify | The Charging Characteristic exposes the charging status.       |

The charging state is represented by the following values:

- `0`: The device is in pre-charge state
- `1`: The device is fast charging
- `2`: The device is fully charged
- `3`: The device is in sleep mode

#### Command GATT

| Type           | UUID                                   | Name                   | Definition      | Description                                                    |
| -------------- | -------------------------------------- | ---------------------- | --------------- | -------------------------------------------------------------- |
| Service        | `6acccabd-1728-4697-9b4a-bf25ecca14aa` | Command Service        |                 | The Command Service exposes the command of the Pillo Hardware. |
| Characteristic | `a9147e1f-e91f-4a02-b6e4-2869e0fe69bb` | Command Characteristic | 1-5 byte, Write | The Command Characteristic exposes the command.                |

The command characteristic expects the following values:

- `0x01`: Set Max Pressure Value
- `0x0F`: Power Off

#### Calibration GATT

| Type           | UUID                                   | Name                             | Definition           | Description                                                                   |
| -------------- | -------------------------------------- | -------------------------------- | -------------------- | ----------------------------------------------------------------------------- |
| Service        | `7e238267-146f-461c-8615-39b358a428a5` | Calibration Service              |                      | The Calibration Service exposes the calibration of the Pillo.                 |
| Characteristic | `60b89ebc-d1c2-45ed-8b30-aa3ebd6ded65` | Calibration Value Characteristic | 1 byte, Read & Write | The Calibration Value Characteristic exposes the calibration.                 |
| Characteristic | `46f9ab5b-d01a-4353-9db4-176c4f3200cf` | Start Calibration Characteristic | 1 byte, Write        | When any value is written to this characteristic, the calibration will start. |

<br/><br/><br/>

## Development and Contribution

### Working on the Unity Packages

To start development on the Unity packages of the Pillo SDK, clone the entirety of this repository in your Unity project's `Assets` directory as if it were a normal asset. This will allow you to make changes to the packages and test them in your Unity project without having them to be installed via the Unity Package Manager.

### Working on the Native Libraries

To start development on the native libraries of the Pillo SDK, clone the entirety of this repository in any directory on your computer. Open this XCode project which can be found in the `Plugin` directory within the Pillo SDK Framework.

To test the native libraries in an actual Unity environment, a build must be made using the first step. This will generate an XCode project which can be opened and run on a physical device or simulator.
