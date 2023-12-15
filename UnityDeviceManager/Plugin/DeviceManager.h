#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

#define DEVICEINFORMATION_SERVICE_UUID @"180A"
#define DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID @"2A24"
#define DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID @"2A26"
#define DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID @"2A27"
#define BATTERY_SERVICE_UUID @"180F"
#define BATTERY_LEVEL_CHARACTERISTIC_UUID @"2A19"
#define PRESSURE_SERVICE_UUID @"579BA43D-A351-463D-92C7-911EC1B54E35"
#define PRESSURE_VALUE_CHARACTERISTIC_UUID @"1470CA75-5D7E-4E16-A70D-D1476E8D0C6F"
#define CHARGE_SERVICE_UUID @"044402A3-F8B4-479A-B995-63E99ACB2735"
#define CHARGE_STATE_CHARACTERISTIC_UUID @"22FEB891-0057-4A3E-AF5B-EC769849077C"
#define COMMAND_SERVICE_UUID @"6ACCCABD-1728-4697-9B4A-BF25ECCA14AA"
#define COMMAND_COMMAND_CHARACTERISTIC_UUID @"A9147E1F-E91F-4A02-B6E4-2869E0FE69BB"
#define COMMAND_LED_CHARACTERISTIC_UUID @"7B3B969D-316A-450E-BDB9-6F1792270FA1"
#define CALIBRATION_SERVICE_UUID @"7E238267-146F-461C-8615-39B358A428A5"
#define CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID @"46F9AB5B-D01A-4353-9DB4-176C4F3200CF"
#define HANDSHAKE_SERVICE_UUID @"35865C86-7B91-4834-B44A-8A66985D1375"
#define HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID @"45C30C15-4815-4CDF-9ED3-9CC488492F4F"
#define SCAN_DURATION_SECONDS 2
#define SCAN_INTERVAL_SECONDS 10
#define MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION 2

typedef void (*OnCentralDidInitialize)(void);
typedef void (*OnCentralDidFailToInitialize)(const char *);
typedef void (*OnCentralDidStartScanning)(void);
typedef void (*OnCentralDidStopScanning)(void);
typedef void (*OnPeripheralDidConnect)(const char *);
typedef void (*OnPeripheralDidDisconnect)(const char *);
typedef void (*OnPeripheralDidFailToConnect)(const char *);
typedef void (*OnPeripheralBatteryLevelDidChange)(const char *, int);
typedef void (*OnPeripheralPressureDidChange)(const char *, int);
typedef void (*OnPeripheralChargingStateDidChange)(const char *, int);
typedef void (*OnPeripheralFirmwareVersionDidChange)(const char *, const char *);
typedef void (*OnPeripheralHardwareVersionDidChange)(const char *, const char *);
typedef void (*OnPeripheralModelNumberDidChange)(const char *, const char *);

@interface DeviceManager : NSObject <CBCentralManagerDelegate, CBPeripheralDelegate> {
    @public OnCentralDidInitialize onCentralDidInitialize;
    @public OnCentralDidFailToInitialize onCentralDidFailToInitialize;
    @public OnCentralDidStartScanning onCentralDidStartScanning;
    @public OnCentralDidStopScanning onCentralDidStopScanning;
    @public OnPeripheralDidConnect onPeripheralDidConnect;
    @public OnPeripheralDidDisconnect onPeripheralDidDisconnect;
    @public OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;
    @public OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;
    @public OnPeripheralPressureDidChange onPeripheralPressureDidChange;
    @public OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange;
    @public OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange;
    @public OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange;
    @public OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange;
}

- (void)cancelPeripheralConnection:(NSString *)identifier;
- (void)powerOffPeripheral:(NSString *)identifier;
- (void)forcePeripheralLedOff:(NSString *)identifier enabled:(BOOL)enabled;
- (void)calibratePeripheral:(NSString *)identifier;
- (void)tempFixForRacingConditionCrash_unityIsReady; // TODO: Resolve this issue in a better way

@end
