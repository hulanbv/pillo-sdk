# Unity Pillo SDK Input System

The Pillo SDK Input System provides the functionality for interacting with the Pillo Hardware via the Unity Input System. This package requires the Pillo SDK Framework to be installed.

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory. _Note that the Pillo SDK Input System requires the [Pillo SDK Framework](#unity-pillo-sdk-framework) to be installed as well._

```json
"nl.hulan.pillo-sdk.input-system.unity": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnityInputSystem"
```

## Documentation

The Pillo SDK Input System is based around `PilloInputDevice`s. These devices are created when a Pillo is connected to the device. The `PilloInputDevice` exposes a set of controls that can be used to interact with the Pillo Hardware, and a set of properties that can be used to retrieve information about the Pillo Hardware. The `PilloInputSystem` can be used to subscribe to specific events as well as retrieve a list of all connected Pillo devices.

### Pillo Input System Delegates

#### Pillo Input Device Did Connect

The `OnPilloInputDeviceDidConnect` delegate is called when a Pillo is connected to the device. A new `PilloInputDevice` is created and passed to the delegate.

```csharp
public delegate void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice);
```

```csharp
PilloInputSystem.onPilloInputDeviceDidConnect += (PilloInputDevice pilloInputDevice) => {
    Debug.Log($"Pillo {pilloInputDevice.identifier} connected.");
};
```

#### Pillo Input Device Did Disconnect

The `OnPilloInputDeviceDidDisconnect` delegate is called when a Pillo is disconnected from the device. The `PilloInputDevice` is passed to the delegate.

```csharp
public delegate void OnPilloInputDeviceDidDisconnect (PilloInputDevice pilloInputDevice);
```

```csharp
PilloInputSystem.onPilloInputDeviceDidDisconnect += (PilloInputDevice pilloInputDevice) => {
    Debug.Log($"Pillo {pilloInputDevice.identifier} disconnected.");
};
```

#### Pillo Input Device Did Fail To Connect

The `OnPilloInputDeviceDidFailToConnect` delegate is called when a Pillo fails to connect to the device.

```csharp
public delegate void OnPilloInputDeviceDidFailToConnect ();
```

```csharp
PilloInputSystem.onPilloInputDeviceDidFailToConnect += () => {
    Debug.Log("Pillo failed to connect.");
};
```

#### Pillo Input Device State Did Change

The `OnPilloInputDeviceStateDidChange` delegate is called when the state of a Pillo changes. The `PilloInputDevice` which derives from the `PilloInputDeviceState` is passed to the delegate.

```csharp
public delegate void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice);
```

```csharp
PilloInputSystem.onPilloInputDeviceStateDidChange += (PilloInputDevice pilloInputDevice) => {
    Debug.Log($"Pillo {pilloInputDevice.identifier} state changed.");
};
```

### Pillo Input System Methods

#### Reset Pillo Input Device Player Indexes

The `ResetPilloInputDevicePlayerIndexes` method can be used to reset the player indexes of all connected Pillo devices.

```csharp
public static void ResetPilloInputDevicePlayerIndexes ()
```

### Pillo Input System Properties

#### Pillo Input Devices

The `pilloInputDevices` property can be used to retrieve a list of all connected Pillo devices.

```csharp
public static List<PilloInputDevice> pilloInputDevices { get; }
```

#### Pillo Input Device Count

The `pilloInputDeviceCount` property can be used to retrieve the number of connected Pillo devices.

```csharp
public static int pilloInputDeviceCount { get; }
```

### Pillo Input Device State Properties

#### Pillo Input Device State Identifier

The Pillo Input Device's unique identifier assigned by the Pillo Bluetooth peripheral.

```csharp
public string identifier { get; }
```

#### Pillo Input Device State Player Index

Internally assigned player indexes. These indexes are assigned automatically and are used to identify the player that the device belongs to. If a device disconnects, the player index is reassigned.

```csharp
public int playerIndex { get; }
```

#### Pillo Input Device State Pressure

The Pillo Input Device's current pressure value.

```csharp
public int pressure { get; }
```

The pressure value is a value between `0` and `1024` where `0` is the lowest pressure and `1` is the highest pressure.

#### Pillo Input Device State Battery Level

The Pillo Input Device's current battery level.

```csharp
public int batteryLevel { get; }
```

The battery level is a value between `0` and `100` where `0` is the lowest battery level and `100` is the highest battery level.

#### Pillo Input Device State Charge State

The Pillo Input Device's current charge state.

```csharp
public PilloInputDeviceChargeState chargeState { get; }
```

- `-1` `UNKNOWN` - The charge state is unknown.
- `0` `PRE_CHARGE` - The Pillo is preparing for charging.
- `1` `FAST_CHARGE` - The Pillo is charging.
- `2` `CHARGE_DONE` - The Pillo is fully charged.
- `3` `SLEEP_MODE` - The Pillo is not charging and is in sleep mode.

### Pillo Input Device Methods

#### Power Off the Pillo Input Device

The `PowerOff` method can be used to power off the Pillo Hardware.

```csharp
public void PowerOff ();
```

#### Start Calibrating the Pillo Input Device

The `StartCalibrating` method can be used to start calibrating the Pillo Hardware.

```csharp
public void StartCalibration ();
```

<br/><br/><br/>
