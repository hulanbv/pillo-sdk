# Pillo SDK Package

This repository contains the Unity Package for the Pillo SDK.

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your manifest.json file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity. To make sure you're using a stable version, use a tag at the end of the Git URL in order to install a specific release.

```json
{
  "dependencies": {
    "nl.hulan.pillo-sdk.framework": "git+https://github.com/hulanbv/pillo-sdk-package.git?path=/Framework#v1.0.0"
  }
}
```

## Documentation

The Pillo SDK provides a High-Level API for interacting with the Pillo Hardware. The Framework is available on the following platforms:

- AppleTV running tvOS 9.0 or later
- Apple iPhone running iOS 5.0 or later
- Apple iPad running iPadOS 5.0 or later

### Getting Started using the Input System

Get started by installing this package in your Unity project and Switching the build platform to Apple TV. Then importing the Pillo SDK Input System Namespace. Importing this namespace will expose the `PilloInput` class which contains all the methods, properties delegates of the Pillo SDK Input System.

```csharp
using Hulan.PilloSDK.InputSystem;
// PilloInput is now available
```

There is no need to initialize the Pillo SDK Input System. You can start using the Pillo SDK Input System right away by adding listeners to the Input System's delegates, reading it's properties and using the methods.

```csharp
using UnityEngine;
using Hulan.PilloSDK.InputSystem;

public class PilloTestComponent : MonoBehaviour {
  public void Start () {
    PilloInput.onPilloInputDeviceDidConnect += this.OnPilloInputDeviceDidConnect;
    PilloInput.onPilloInputDeviceStateDidChange += this.OnPilloInputDeviceStateDidChange;
  }
  public void OnDestroy () {
    PilloInput.onPilloInputDeviceDidConnect -= this.OnPilloInputDeviceDidConnect;
    PilloInput.onPilloInputDeviceStateDidChange -= this.OnPilloInputDeviceStateDidChange;
  }
  public void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice) {
    // Say hello to 'pilloInputDevice.identifier', player 'pilloInputDevice.playerIndex'!
  }
  public void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice) {
    // Feeling a bit heavy, 'pilloInputDevice.pressure'!
  }
}
```

### Pillo Input Delegates

**OnCentralDidInitialize `Version 2.0.0`**

Delegate invoked when the Framework has been initialized.

```csharp
public delegate void OnCentralDidInitialize ();
```

**OnCentralDidFailToInitialize `Version 2.0.0`**

Delegate invoked when the Framework has failed to initialize.

```csharp
public delegate void OnCentralDidFailToInitialize (string message);
```

**OnPilloInputDeviceDidConnect `Version 2.0.0`**

Delegate invoked when a Pillo Input Device has been connected.

```csharp
public delegate void OnPilloInputDeviceDidConnect (PilloInputDevice pilloInputDevice);
```

**OnPilloInputDeviceDidDisconnect `Version 2.0.0`**

Delegate invoked when a Pillo Input Device has been disconnected.

```csharp
public delegate void OnPilloInputDeviceDidDisconnect (PilloInputDevice pilloInputDevice);
```

**OnPilloInputDeviceDidFailToConnect `Version 2.0.0`**

Delegate invoked when a Pillo Input Device has failed to connect.

```csharp
public delegate void OnPilloInputDeviceDidFailToConnect ();
```

**OnPilloInputDeviceStateDidChange `Version 2.0.0`**

Delegate invoked when the Pillo Input Device's state did change.

```csharp
public delegate void OnPilloInputDeviceStateDidChange (PilloInputDevice pilloInputDevice);
```

### Pillo Input Properties

**pilloInputDevices `Version 2.0.0` and pilloInputDevicesCount `Version 2.0.0`**

Use the `pilloInputDevices` and `pilloInputDevicesCount` respectively to get a list of connected Pillo Input Devices.

```csharp
for (var i = 0; i < PilloInput.pilloInputDevicesCount; i++) {
  var pilloInputDevice = PilloInput.pilloInputDevices[i];
  // Do something with 'pilloInputDevice'
}
```

### Pillo Input Methods

**ReassignPilloInputDevicePlayerIndexes `Version 2.1.0`**

Resets all of the Pillo Input Device's player indexes back to their original values.

```csharp
public static void ResetPilloInputDevicePlayerIndexes ();
```

### Pillo Input Device Properties

**modelNumber `Version 2.2.0`**

The model number of the Pillo Peripheral.

```csharp
public string modelNumber;
```

**firmwareVersion `Version 2.2.0`**

The firmware version of the Pillo Peripheral.

```csharp
public string firmwareVersion;
```

**hardwareVersion `Version 2.2.0`**

The hardware version of the Pillo Peripheral.

```csharp
public string hardwareVersion;
```

### Pillo Input Device Methods

**PowerOff `Version 2.1.0`**

Turns off the Pillo Input Device.

```csharp
public void PowerOff ();
```

**SetMaximumPressure `Version 2.1.0`**

Sets the maximum pressure value of the Pillo Input Device.

```csharp
public void SetMaximumPressure (int maximumPressureValue);
```

### Pillo Input Device State Properties

**modelNumber `Version 2.0.0`**

The Pillo Input Device's unique identifier assigned by the Pillo Bluetooth peripheral.

```csharp
public string identifier;
```

**playerIndex `Version 2.1.0`**

Internally assigned player indexes. These indexes are assigned automatically and are used to identify the player that the device belongs to. If a device disconnects, the player index is reassigned.

```csharp
public int playerIndex;
```

**pressure `Version 2.0.0`**

The Pillo Input Device's pressure level.

```csharp
public int pressure;
```

**batteryLevel `Version 2.0.0`**

The Pillo Input Device's battery level.

```csharp
public int batteryLevel;
```

**chargeState `Version 2.0.0`**

The Pillo Input Device's charge state.

```csharp
public PilloInputDeviceChargeState chargeState;
```

## Development

To contribute to the Pillo SDK package, create a new Unity Project and close this repository inside of the packages folder as following. This clones the Package as a development package inside of your Unity project. It will appear as a normal packages within the Unity Editor, but is editable. When any of the files within the Assembly Definition are changed, it will be recompiled and reimported.

```sh
$ cd MyUnityProject/Packages
$ git clone https://github.com/hulanbv/pillo-sdk-package pillo-sdk-package
```

#### Termology

The new Pillos communicate over BLE, this stands for Bluetooth Low Energy. The "Wokkel" developed by Vention provides an implementation which exposes various Services and Characteristics over this protocol.

In Bluetooth-speak, the device that has the data is referred to as the Peripheral, and the device that wants the data contained in the Peripheral is known as the Central. In case of a Pillo app, this means that typically the iPhone, iPad or AppleTV app we are developing will be the Central, interacting with one or more Peripherals to glean information that can then be processed, analyzed, or stored in some meaningful fashion.

An app using the Pillo SDK will act as a Central, we will want to know how to get the data we’re interested in out of the Peripheral. This is where Services and Characteristics come into play. You can think of Characteristics somewhat as being like properties of your device which and be read from and written to, and a Service is a collection of characteristics. A Peripheral can (and usually does) contain multiple services that we inspect to determine what characteristics are available with which to interact.

Before you can read from or write to values on the device, you need to Discover it. As it turns out, if a Pillo is turned on and is in range, it periodically sends out a little signal that lets interested parties know that it’s alive and kicking. This process is known as Advertising, and the time between signals is known as the “advertising interval.”

The Pillo SDK listens for those advertisements, and it will specify exactly which Services it is interested in. When going through its discovery phase, the SDK will find Pillos in the area that support the desired Services. This is very important, because the number of Bluetooth LE devices in existence, and which could potentially be in our vicinity, is constantly increasing. Therefore, it is helpful to let the framework weed out the devices that are not broadcasting the types of services we are interested in.

On iOS and TVOS the Core Bluetooth framework provides the classes needed for your apps to communicate with Bluetooth-equipped low energy (LE) and Basic Rate / Enhanced Data Rate (BR/EDR) wireless technology.

#### Structure

The Pillo SDK consists of the following namespaces:

- **The Pillo Framework** which contains two layers. The Plugin, which is responsible for communicating with the Pillo hardware. And the Runtime, which is responsible for sending and receiving messages from the Plugin which toghether makes a bridge to form all communication between the Pillo hardware and the Pillo SDK.
- **The Pillo Input System** which contains an abstraction layer for the Pillo hardware. It exposes the Pillo hardware as a set of properties and methods that can be used to interact with the hardware. The Pillo Input System uses the Framework to achieve this.

_More Namespaces may be added in future releases._

## Creating Releases

Mark releases by pushing version tags.

```sh
$ npm version [<newversion> | major | minor | patch]
$ git push origin <tag_name>
```

## Distribution

The repository is distributed automatically via Git.
