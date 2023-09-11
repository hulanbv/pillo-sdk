# Bluetooth Attribute Profile

The following Bluetooth Low Energy Generic Attribute Profile Definitions are based on version `0.4` of the `VT_HULPIL_20220308_BLE_GATT_PROFILES` protocol.

## Generic Attribute Services

The Generic Attribute Service is a general-purpose service for discovering attributes of a device that are not associated with a service.

### Battery Service

The Battery Service exposes the battery level of the Pillo.

| Type           | UUID   | Name            | Definition            | Description                                               |
| -------------- | ------ | --------------- | --------------------- | --------------------------------------------------------- |
| Service        | `180f` | Battery Service |                       |                                                           |
| Characteristic | `2a19` | Battery Level   | 1 Byte, Read & Notify | The Battery level contains a value between `0` and `100`. |

### Device Information Service

The Device Information Service exposes manufacturer and/or vendor information about a device.

| Type           | UUID   | Name                       | Definition   | Description                                                                |
| -------------- | ------ | -------------------------- | ------------ | -------------------------------------------------------------------------- |
| Service        | `180a` | Device Information Service |              |                                                                            |
| Characteristic | `2a29` | Manufacturer Name String   | String, Read | The Manufacturer Name String characteristic exposes the manufacturer name. |
| Characteristic | `2a24` | Model Number String        | String, Read | The Model Number String characteristic exposes the model number.           |
| Characteristic | `2a27` | Hardware Revision String   | String, Read | The Hardware Revision String characteristic exposes the hardware revision. |
| Characteristic | `2a26` | Firmware Revision String   | String, Read | The Firmware Revision String characteristic exposes the firmware revision. |

### Pressure Service

The Pressure Service exposes the pressure value of the Pillo.

| Type           | UUID                                   | Name                    | Definition                      | Description                                                           |
| -------------- | -------------------------------------- | ----------------------- | ------------------------------- | --------------------------------------------------------------------- |
| Service        | `579ba43d-a351-463d-92c7-911ec1b54e35` | Pressure Service        |                                 |                                                                       |
| Characteristic | `1470ca75-5d7e-4e16-a70d-d1476e8d0c6f` | Pressure Characteristic | 1 byte, Read & Notify (uint8_t) | The current pressure value containing a value between `0` and `1024`. |

### Charging Service

The Charging Service exposes the charging status of the Pillo.

| Type           | UUID                                   | Name                          | Definition                      | Description                                                                                                 |
| -------------- | -------------------------------------- | ----------------------------- | ------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| Service        | `044402a3-f8b4-479a-b995-63e99acb2735` | Charging Service              |                                 |                                                                                                             |
| Characteristic | `22feb891-0057-4a3e-af5b-ec769849077c` | Charging State Characteristic | 1 byte, Read & Notify (uint8_t) | `0` indicates pre-charge, `1` indicates fast charge, `2` indicates fully charged, `3` indicates sleep mode. |

### Calibration Service

> Available on Pillo firmware version 0.1.1 or later.

The Calibration Service exposes the calibration of the Pillo Hardware.

| Type           | UUID                                   | Name                             | Definition                     | Description                                  |
| -------------- | -------------------------------------- | -------------------------------- | ------------------------------ | -------------------------------------------- |
| Service        | `7e238267-146f-461c-8615-39b358a428a5` | Calibration Service              |                                |                                              |
| Characteristic | `60b89ebc-d1c2-45ed-8b30-aa3ebd6ded65` | Calibration Characteristic       | 1 byte, Read & Write (uint8_t) | The current calibration value.               |
| Characteristic | `46f9ab5b-d01a-4353-9db4-176c4f3200cf` | Start Calibration Characteristic | 1 byte, Write (uint8_t)        | Write any value, the calibration will start. |

### Handshake Service

> Available on Pillo firmware version 0.1.2 or later.

The Handshake Service exposes the handshake of the Pillo Hardware.

| Type           | UUID                                   | Name                     | Definition                       | Description                                                                                                            |
| -------------- | -------------------------------------- | ------------------------ | -------------------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| Serice         | `35865c86-7b91-4834-b44a-8a66985d1375` | Handshake Service        |                                  |                                                                                                                        |
| Characteristic | `45c30c15-4815-4cdf-9ed3-9cc488492f4f` | Handshake Characteristic | 8 bytes, Read & Write (uint64_t) | Read this value to get a number used for calculating the handshake value, write calculated value to perform handshake. |

### Command Service

> Available on Pillo firmware version 0.1.1 or later.

The Command Service exposes the command of the Pillo Hardware.

| Type           | UUID                                   | Name                   | Definition                     | Description                                                       |
| -------------- | -------------------------------------- | ---------------------- | ------------------------------ | ----------------------------------------------------------------- |
| Service        | `6acccabd-1728-4697-9b4a-bf25ecca14aa` | Command Service        |                                |                                                                   |
| Characteristic | `a9147e1f-e91f-4a02-b6e4-2869e0fe69bb` | Command Characteristic | 1-5 byte, Write (uint8_t)      | Write `0x01` to set max pressure value, write `0x0F` to power off |
| Characteristic | `7b3b969d-316a-450e-bdb9-6f1792270fa1` | LED characteristic     | 1 byte, Read & Write (uint8_t) | Write `0x01` to turn on, write `0x00` to turn off                 |

<br/><br/><br/>
