#import "DeviceManager.h"

// Pillo Framework Device Manager Implementation File
// Author: Jeffrey Lanters at Hulan
@implementation DeviceManager

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
#define SCAN_DURATION_SECONDS 2
#define SCAN_INTERVAL_SECONDS 10
#define MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION 2

- (void)initialize {
  self.centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil];
  self.peripherals = [NSMutableArray<CBPeripheral *> arrayWithCapacity:MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION];
  [self startScanningForPeripheralsRoutine];
}

- (void)startScanningForPeripheralsRoutine {
  if ([self.centralManager isScanning] == true) {
    if ([self.peripherals count] > 0) {
      [self.centralManager stopScan];
    }
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self startScanningForPeripheralsRoutine];
    });
  } else if ([self.peripherals count] < MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
    [self.centralManager scanForPeripheralsWithServices:nil options:[NSDictionary dictionaryWithObjectsAndKeys:@YES, CBCentralManagerScanOptionAllowDuplicatesKey, nil]];
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      if ([self.peripherals count] > 0) {
        [self.centralManager stopScan];
      }
      dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_INTERVAL_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
        [self startScanningForPeripheralsRoutine];
      });
    });
  } else {
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_INTERVAL_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self startScanningForPeripheralsRoutine];
    });
  }
}

- (void)centralManagerDidUpdateState:(nonnull CBCentralManager *)central {
  switch ([central state]) {
    case CBManagerStatePoweredOn:
      [self invokeUnityCallback:@"OnCentralDidInitialize"];
      break;
    default:
      [self invokeUnityCallback:@"OnCentralDidFailToInitialize" payload:@{
        @"message": @"Unable to initialize CoreBluetooth Central Manager"
      }];
      break;
  }
}

- (void)centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary<NSString *,id> *)advertisementData RSSI:(NSNumber *)RSSI {
  if (![self.peripherals containsObject:peripheral] && [peripheral.name containsString:@"Pillo"]) {
    peripheral.delegate = self;
    [self.peripherals addObject:peripheral];
    [self.centralManager connectPeripheral:peripheral options:nil];
    if ([self.peripherals count] >= MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
      [self.centralManager stopScan];
    }
  }
}

- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
  [self invokeUnityCallback:@"OnPeripheralDidConnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [peripheral discoverServices:@[
    [CBUUID UUIDWithString:DEVICEINFORMATION_SERVICE_UUID],
    [CBUUID UUIDWithString:BATTERY_SERVICE_UUID],
    [CBUUID UUIDWithString:PRESSURE_SERVICE_UUID],
    [CBUUID UUIDWithString:CHARGE_SERVICE_UUID]
  ]];
}

- (void)centralManager:(CBCentralManager *)central didDisconnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidDisconnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [self.centralManager cancelPeripheralConnection:peripheral];
  [self.peripherals removeObjectAtIndex:[self.peripherals indexOfObject:peripheral]];
}

- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidFailToConnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [self.centralManager cancelPeripheralConnection:peripheral];
  [self.peripherals removeObjectAtIndex:[self.peripherals indexOfObject:peripheral]];
}

- (void)peripheral:(CBPeripheral *)peripheral didDiscoverServices:(NSError *)error {
  for (CBService *service in peripheral.services) {
    [peripheral discoverCharacteristics:nil forService:service];
  }
}

- (void)peripheral:(CBPeripheral *)peripheral didDiscoverCharacteristicsForService:(CBService *)service error:(NSError *)error {
  for (CBCharacteristic *characteristic in service.characteristics) {
    [peripheral setNotifyValue:true forCharacteristic:characteristic];
  }
}

- (void)peripheral:(CBPeripheral *)peripheral didUpdateValueForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
  NSData *rawData = characteristic.value;
  NSString *serviceUUID = characteristic.service.UUID.UUIDString;
  NSString *characteristicUUID = characteristic.UUID.UUIDString;
  if ([serviceUUID isEqualToString:PRESSURE_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:PRESSURE_VALUE_CHARACTERISTIC_UUID]) {
      uint32_t pressure = 0;
      [rawData getBytes:&pressure length:sizeof(pressure)];
      [self invokeUnityCallback:@"OnPeripheralPressureDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"pressure": @(pressure)
      }];
    }
  }
  else if ([serviceUUID isEqualToString:BATTERY_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:BATTERY_LEVEL_CHARACTERISTIC_UUID]) {
      uint32_t batteryLevel = 0;
      [rawData getBytes:&batteryLevel length:sizeof(batteryLevel)];
      [self invokeUnityCallback:@"OnPeripheralBatteryLevelDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"batteryLevel": @(batteryLevel)
      }];
    }
  }
  else if ([serviceUUID isEqualToString:CHARGE_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:CHARGE_STATE_CHARACTERISTIC_UUID]) {
      uint32_t chargeState = 0;
      [rawData getBytes:&chargeState length:sizeof(chargeState)];
      [self invokeUnityCallback:@"OnPeripheralChargeStateDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"chargeState": @(chargeState)
      }];
    }
  }
}

- (void)peripheralManagerDidUpdateState:(nonnull CBPeripheralManager *)peripheral { }

- (void)cancelPeripheralConnection:(NSString *)identifier {
  for (CBPeripheral *peripheral in self.peripherals) {
    if ([peripheral.identifier.UUIDString isEqualToString:identifier]) {
      [self.centralManager cancelPeripheralConnection:peripheral];
      break;
    }
  }
}

- (void)invokeUnityCallback:(NSString *)methodName {
  UnitySendMessage("~DeviceManagerCallbackListener", [methodName UTF8String], [@"" UTF8String]);
}

- (void)invokeUnityCallback:(NSString *)methodName payload:(NSDictionary *)payload {
  NSError *error;
  NSData *payloadJsonData = [NSJSONSerialization dataWithJSONObject:payload options:kNilOptions error:&error];
  NSString *payloadJson = [[NSString alloc]initWithData:payloadJsonData encoding:NSUTF8StringEncoding];
  UnitySendMessage("~DeviceManagerCallbackListener", [methodName UTF8String], [payloadJson UTF8String]);
}

@end

extern "C" {
  DeviceManager* deviceManager = nil;

  void _DeviceManagerInstantiate() {
    if (deviceManager == nil) {
      deviceManager = [DeviceManager new];
      [deviceManager initialize];
    }
  }

  void _DeviceManagerCancelPeripheralConnection(const char* identifier) {
    if (deviceManager != nil) {
      [deviceManager cancelPeripheralConnection:[NSString stringWithUTF8String:identifier]];
    }
  }
}