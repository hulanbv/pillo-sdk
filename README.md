# Pillo Software Development Kit Mono Repository

The Pillo SDK Mono Repository contains various packages for interacting with the Pillo Hardware via Unity on various Platforms.

# Pillo SDK Framework

The Pillo SDK Framework provides the core functionality for interacting with the Pillo Hardware.

## Compatibility

The Pillo SDK Framework is compatible with the following platforms:

- Unity Engine for AppleTV running tvOS 9.0 or later
- Unity Engine for Apple iPhone running iOS 5.0 or later
- Unity Engine for Apple iPad running iPadOS 5.0 or later

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory.

```json
"nl.hulan.pillo-sdk.framework": "https://github.com/hulanbv/pillo-sdk-package.git?path=/Framework"
```

# Generic Attribute Profile Definitions

The following Bluetooth Low Energy Generic Attribute Profile Definitions are based on version `0.4` of the `VT_HULPIL_20220308_BLE_GATT_PROFILES` protocol.

## Generic Attribute Services

### Battery GATT

| Type           | UUID   | Name            | Definition            | Description                                                          |
| -------------- | ------ | --------------- | --------------------- | -------------------------------------------------------------------- |
| Service        | `180f` | Battery Service |                       | The Battery Service exposes the battery level of the Pillo Hardware. |
| Characteristic | `2a19` | Battery Level   | 1 Byte, Read & Notify | The Battery Level characteristic exposes the battery level.          |

The battery level is represented as a value between `0` and `100` where `0` is the lowest battery level and `100` is the highest battery level.

### Device Information GATT

| Type           | UUID   | Name                       | Definition   | Description                                                                 |
| -------------- | ------ | -------------------------- | ------------ | --------------------------------------------------------------------------- |
| Service        | `180a` | Device Information Service |              | The Device Information Service exposes the device information of the Pillo. |
| Characteristic | `2a29` | Manufacturer Name String   | String, Read | The Manufacturer Name String characteristic exposes the manufacturer name.  |
| Characteristic | `2a24` | Model Number String        | String, Read | The Model Number String characteristic exposes the model number.            |
| Characteristic | `2a27` | Hardware Revision String   | String, Read | The Hardware Revision String characteristic exposes the hardware revision.  |
| Characteristic | `2a26` | Firmware Revision String   | String, Read | The Firmware Revision String characteristic exposes the firmware revision.  |

### Pressure GATT

| Type           | UUID                                   | Name                    | Definition            | Description                                                      |
| -------------- | -------------------------------------- | ----------------------- | --------------------- | ---------------------------------------------------------------- |
| Service        | `579ba43d-a351-463d-92c7-911ec1b54e35` | Pressure Service        |                       | The Pressure Service exposes the pressure of the Pillo Hardware. |
| Characteristic | `1470ca75-5d7e-4e16-a70d-d1476e8d0c6f` | Pressure Characteristic | 1 byte, Read & Notify | The Pressure Characteristic exposes the pressure.                |

The pressure is represented a value between `0` and `255` where `0` is the lowest pressure and `255` is the highest pressure.

### Charging GATT

| Type           | UUID                                   | Name                          | Definition            | Description                                                    |
| -------------- | -------------------------------------- | ----------------------------- | --------------------- | -------------------------------------------------------------- |
| Service        | `579ba43d-a351-463d-92c7-911ec1b54e35` | Charging Service              |                       | The Charging Service exposes the charging status of the Pillo. |
| Characteristic | `22feb891-0057-4a3e-af5b-ec769849077c` | Charging State Characteristic | 1 byte, Read & Notify | The Charging Characteristic exposes the charging status.       |

The charging state is represented by the following values:

- `0`: The device is in pre-charge state
- `1`: The device is fast charging
- `2`: The device is fully charged
- `3`: The device is in sleep mode

### Command GATT

| Type           | UUID                                   | Name                   | Definition      | Description                                                    |
| -------------- | -------------------------------------- | ---------------------- | --------------- | -------------------------------------------------------------- |
| Service        | `6acccabd-1728-4697-9b4a-bf25ecca14aa` | Command Service        |                 | The Command Service exposes the command of the Pillo Hardware. |
| Characteristic | `a9147e1f-e91f-4a02-b6e4-2869e0fe69bb` | Command Characteristic | 1-5 byte, Write | The Command Characteristic exposes the command.                |

The command characteristic expects the following values:

- `0x01`: Set Max Pressure Value
- `0x0F`: Power Off

### Calibration GATT

| Type           | UUID                                   | Name                             | Definition           | Description                                                                   |
| -------------- | -------------------------------------- | -------------------------------- | -------------------- | ----------------------------------------------------------------------------- |
| Service        | `7e238267-146f-461c-8615-39b358a428a5` | Calibration Service              |                      | The Calibration Service exposes the calibration of the Pillo.                 |
| Characteristic | `60b89ebc-d1c2-45ed-8b30-aa3ebd6ded65` | Calibration Value Characteristic | 1 byte, Read & Write | The Calibration Value Characteristic exposes the calibration.                 |
| Characteristic | `46f9ab5b-d01a-4353-9db4-176c4f3200cf` | Start Calibration Characteristic | 1 byte, Write        | When any value is written to this characteristic, the calibration will start. |

# Development

## Working on the Unity Packages

To start development on the Unity packages of the Pillo SDK, clone the entirety of this repository in your Unity project's `Assets` directory as if it were a normal asset. This will allow you to make changes to the packages and test them in your Unity project without having them to be installed via the Unity Package Manager.

## Working on the Native Libraries

To start development on the native libraries of the Pillo SDK, clone the entirety of this repository in any directory on your computer. Open this XCode project which can be found in the `Plugin` directory within the Pillo SDK Framework.

To test the native libraries in an actual Unity environment, a build must be made using the first step. This will generate an XCode project which can be opened and run on a physical device or simulator.
