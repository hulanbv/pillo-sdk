# Pillo SDK Unity Device Manager

The Pillo SDK Unity Device Manager provides the core functionality for interacting with the Pillo Hardware in Unity. The `PilloDeviceManager` is a static utility class in the **Hulan.PilloSDK** that acts as a bridge between Unity and the native Pillo plugin. It is responsible for registering native callbacks and exposing methods for managing and interacting with Pillo devices during runtime.

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your manifest.json file located within your project's Packages directory, or add the GIT URL to the Unity Package Manager directly.

```json
"nl.hulan.pillo-sdk.device-manager": "https://github.com/hulanbv/pillo-sdk.git?path=/UnityDeviceManager"
```

## Compatibility

The Pillo SDK Device Manager is compatible with the following platforms:

- Unity Engine for AppleTV running tvOS 9.0 or later
- Unity Engine for Apple iPhone running iOS 5.0 or later
- Unity Engine for Apple iPad running iPadOS 5.0 or later

## Requirements

- Unity Engine (`UnityEngine`)
- Native plugin provided via `Hulan.PilloSDK.DeviceManager.Core`
- Compatible with IL2CPP or AOT platforms (due to `[MonoPInvokeCallback]` usage)

## Initialization

The class is initialized automatically using Unity’s runtime hook via:

```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
```

This setup ensures that all native callbacks are registered before your scene loads. No manual instantiation or setup is required.

---

## Public API Overview

### Peripheral Connection Control

```csharp
public static void CancelPeripheralConnection(string identifier)
```

Cancels the connection attempt or active session with a peripheral device.

```csharp
public static void PowerOffPeripheral(string identifier)
```

Turns off the connected peripheral device entirely.

---

### Peripheral Hardware Behavior

```csharp
public static void ForcePeripheralLedOff(string identifier, bool enabled)
```

Forces the LED of the device to be turned off (or re-enabled).

```csharp
public static void StartPeripheralCalibration(string identifier)
```

Begins a pressure sensor calibration for the given peripheral.

---

## Event Delegates

To respond to device and system-level events, you can assign handlers to any of the following static delegates. These events are triggered by the native plugin via AOT-compatible methods.

### Central Manager Events

| Delegate                                       | Description                                                          |
| ---------------------------------------------- | -------------------------------------------------------------------- |
| `onCentralDidInitialize`                       | Called when the central device manager has initialized successfully. |
| `onCentralDidFailToInitialize(string message)` | Called when initialization fails, providing an error message.        |
| `onCentralDidStartScanning`                    | Triggered when device scanning begins.                               |
| `onCentralDidStopScanning`                     | Triggered when scanning ends.                                        |

### Peripheral Lifecycle Events

| Delegate                                          | Description                                 |
| ------------------------------------------------- | ------------------------------------------- |
| `onPeripheralDidConnect(string identifier)`       | Invoked when a peripheral has connected.    |
| `onPeripheralDidDisconnect(string identifier)`    | Invoked when a peripheral has disconnected. |
| `onPeripheralDidFailToConnect(string identifier)` | Invoked when a connection attempt fails.    |

### Peripheral State & Sensor Events

| Delegate                                                                     | Description                                                |
| ---------------------------------------------------------------------------- | ---------------------------------------------------------- |
| `onPeripheralBatteryLevelDidChange(string identifier, int level)`            | Triggered when the peripheral's battery level updates.     |
| `onPeripheralPressureDidChange(string identifier, int pressure)`             | Triggered when the pressure sensor reading changes.        |
| `onPeripheralChargingStateDidChange(string identifier, ChargingState state)` | Called when the device's charging state changes.           |
| `onPeripheralFirmwareVersionDidChange(string identifier, string version)`    | Called when the firmware version is updated or reported.   |
| `onPeripheralHardwareVersionDidChange(string identifier, string version)`    | Called when the hardware version is available or updated.  |
| `onPeripheralModelNumberDidChange(string identifier, string model)`          | Called when the model number changes or becomes available. |

### Usage Example

```csharp
PilloDeviceManager.onPeripheralDidConnect = (id) => Debug.Log($"Connected to device {id}");
PilloDeviceManager.onPeripheralPressureDidChange = (id, pressure) => Debug.Log($"Pressure: {pressure}");
```
