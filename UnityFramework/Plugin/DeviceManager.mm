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
#define COMMAND_SERVICE_UUID @"6ACCCABD-1728-4697-9B4A-BF25ECCA14AA"
#define COMMAND_COMMAND_CHARACTERISTIC_UUID @"A9147E1F-E91F-4A02-B6E4-2869E0FE69BB"
#define CALIBRATION_SERVICE_UUID @"7E238267-146F-461C-8615-39B358A428A5"
#define CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID @"46F9AB5B-D01A-4353-9DB4-176C4F3200CF"
#define HANDSHAKE_SERVICE_UUID @"35865C86-7B91-4834-B44A-8A66985D1375"
#define HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID @"45C30C15-4815-4CDF-9ED3-9CC488492F4F"
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
      [self invokeUnityCallback:@"OnCentralDidStopScanning"];
    }
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self startScanningForPeripheralsRoutine];
    });
  } else if ([self.peripherals count] < MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
    [self.centralManager scanForPeripheralsWithServices:nil options:[NSDictionary dictionaryWithObjectsAndKeys:@YES, CBCentralManagerScanOptionAllowDuplicatesKey, nil]];
    [self invokeUnityCallback:@"OnCentralDidStartScanning"];
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      if ([self.peripherals count] > 0) {
        [self.centralManager stopScan];
      [self invokeUnityCallback:@"OnCentralDidStopScanning"];
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
    [CBUUID UUIDWithString:CHARGE_SERVICE_UUID],
    [CBUUID UUIDWithString:COMMAND_SERVICE_UUID],
    [CBUUID UUIDWithString:CALIBRATION_SERVICE_UUID],
    [CBUUID UUIDWithString:HANDSHAKE_SERVICE_UUID]
  ]];
}

- (void)centralManager:(CBCentralManager *)central didDisconnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidDisconnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [self.centralManager cancelPeripheralConnection:peripheral]; // TODO: Is this necessary?
  if ([self.peripherals containsObject:peripheral]) {
    [self.peripherals removeObjectAtIndex:[self.peripherals indexOfObject:peripheral]];
  }
}

- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidFailToConnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [self.centralManager cancelPeripheralConnection:peripheral]; // TODO: Is this necessary?
  if ([self.peripherals containsObject:peripheral]) {
    [self.peripherals removeObjectAtIndex:[self.peripherals indexOfObject:peripheral]];
  }
}

- (void)peripheral:(CBPeripheral *)peripheral didDiscoverServices:(NSError *)error {
  for (CBService *service in peripheral.services) {
    if ([service.UUID.UUIDString isEqualToString:BATTERY_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:BATTERY_LEVEL_CHARACTERISTIC_UUID]] forService:service];
    } else if ([service.UUID.UUIDString isEqualToString:PRESSURE_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:PRESSURE_VALUE_CHARACTERISTIC_UUID]] forService:service];
    } else if ([service.UUID.UUIDString isEqualToString:CHARGE_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:CHARGE_STATE_CHARACTERISTIC_UUID]] forService:service];
    } else if ([service.UUID.UUIDString isEqualToString:COMMAND_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:COMMAND_COMMAND_CHARACTERISTIC_UUID]] forService:service];
    } else if ([service.UUID.UUIDString isEqualToString:CALIBRATION_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID]] forService:service];
    } else if ([service.UUID.UUIDString isEqualToString:DEVICEINFORMATION_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID], [CBUUID UUIDWithString:DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID], [CBUUID UUIDWithString:DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID]] forService:service];
    } else if ([service.UUID.UUIDString isEqualToString:HANDSHAKE_SERVICE_UUID]) {
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID]] forService:service];
    }
  }
}

- (void)peripheral:(CBPeripheral *)peripheral didDiscoverCharacteristicsForService:(CBService *)service error:(NSError *)error {
  NSString *serviceUUID = service.UUID.UUIDString;
  if ([serviceUUID isEqualToString:BATTERY_SERVICE_UUID]) {
    for (CBCharacteristic *characteristic in service.characteristics) {
      if ([characteristic.UUID.UUIDString isEqualToString:BATTERY_LEVEL_CHARACTERISTIC_UUID]) {
        [peripheral setNotifyValue:true forCharacteristic:characteristic];
      }
    }
  } else if ([serviceUUID isEqualToString:PRESSURE_SERVICE_UUID]) {
    for (CBCharacteristic *characteristic in service.characteristics) {
      if ([characteristic.UUID.UUIDString isEqualToString:PRESSURE_VALUE_CHARACTERISTIC_UUID]) {
        [peripheral setNotifyValue:true forCharacteristic:characteristic];
      }
    }
  } else if ([serviceUUID isEqualToString:CHARGE_SERVICE_UUID]) {
    for (CBCharacteristic *characteristic in service.characteristics) {
      if ([characteristic.UUID.UUIDString isEqualToString:CHARGE_STATE_CHARACTERISTIC_UUID]) {
        [peripheral setNotifyValue:true forCharacteristic:characteristic];
      }
    }
  } else if ([serviceUUID isEqualToString:DEVICEINFORMATION_SERVICE_UUID]) {
    for (CBCharacteristic *characteristic in service.characteristics) {
      if ([characteristic.UUID.UUIDString isEqualToString:DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID]) {
        [peripheral readValueForCharacteristic:characteristic];
      } else if ([characteristic.UUID.UUIDString isEqualToString:DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID]) {
        [peripheral readValueForCharacteristic:characteristic];
      } else if ([characteristic.UUID.UUIDString isEqualToString:DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID]) {
        [peripheral readValueForCharacteristic:characteristic];
      }
    }
  } else if ([serviceUUID isEqualToString:HANDSHAKE_SERVICE_UUID]) {
    for (CBCharacteristic *characteristic in service.characteristics) {
      if ([characteristic.UUID.UUIDString isEqualToString:HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID]) {
        [peripheral readValueForCharacteristic:characteristic];
      }
    }
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
  } else if ([serviceUUID isEqualToString:BATTERY_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:BATTERY_LEVEL_CHARACTERISTIC_UUID]) {
      uint32_t batteryLevel = 0;
      [rawData getBytes:&batteryLevel length:sizeof(batteryLevel)];
      [self invokeUnityCallback:@"OnPeripheralBatteryLevelDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"batteryLevel": @(batteryLevel)
      }];
    }
  } else if ([serviceUUID isEqualToString:CHARGE_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:CHARGE_STATE_CHARACTERISTIC_UUID]) {
      uint32_t chargeState = 0;
      [rawData getBytes:&chargeState length:sizeof(chargeState)];
      [self invokeUnityCallback:@"OnPeripheralChargeStateDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"chargeState": @(chargeState)
      }];
    }
  } else if ([serviceUUID isEqualToString:DEVICEINFORMATION_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID]) {
      NSString *firmwareVersion = [[NSString alloc] initWithData:rawData encoding:NSUTF8StringEncoding];
      [self invokeUnityCallback:@"OnPeripheralFirmwareVersionDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"firmwareVersion": firmwareVersion
      }];
    } else if ([characteristicUUID isEqualToString:DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID]) {
      NSString *hardwareVersion = [[NSString alloc] initWithData:rawData encoding:NSUTF8StringEncoding];
      [self invokeUnityCallback:@"OnPeripheralHardwareVersionDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"hardwareVersion": hardwareVersion
      }];
    } else if ([characteristicUUID isEqualToString:DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID]) {
      NSString *modelNumber = [[NSString alloc] initWithData:rawData encoding:NSUTF8StringEncoding];
      [self invokeUnityCallback:@"OnPeripheralModelNumberDidChange" payload:@{
        @"identifier": peripheral.identifier.UUIDString,
        @"modelNumber": modelNumber
      }];
    }
  } else if ([serviceUUID isEqualToString:HANDSHAKE_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID]) {
      uint64_t handshake = 0;
      [rawData getBytes:&handshake length:sizeof(handshake)];
      double value1 = (double)handshake * 0.8129863214;
      double value2 = value1 / ((handshake % 10) + 1);
      double value3 = value2 + (handshake * 0.1870136786);
      uint64_t result = round(value3);
      NSData *value = [NSData dataWithBytes:&result length:sizeof(result)];
      [self writeValueToPeripheral:peripheral.identifier.UUIDString serviceUUID:HANDSHAKE_SERVICE_UUID characteristicUUID:HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID value:value];
    }
  }
}

- (void)peripheralManagerDidUpdateState:(nonnull CBPeripheralManager *)peripheral {
  // TODO: Is this something we need to send to Unity?
}

- (void)cancelPeripheralConnection:(NSString *)identifier {
  for (CBPeripheral *peripheral in self.peripherals) {
    if ([peripheral.identifier.UUIDString isEqualToString:identifier]) {
      [self.centralManager cancelPeripheralConnection:peripheral];
      break;
    }
  }
}

- (void)powerOffPeripheral:(NSString *)identifier {
  NSData *value = [NSData dataWithBytes:(uint8_t[]){ 0x0F } length:1];
  [self writeValueToPeripheral:identifier serviceUUID:COMMAND_SERVICE_UUID characteristicUUID:COMMAND_COMMAND_CHARACTERISTIC_UUID value:value];
}

- (void)calibratePeripheral:(NSString *)identifier {
  NSData *value = [NSData dataWithBytes:(uint8_t[]){ 0x0F } length:1];
  [self writeValueToPeripheral:identifier serviceUUID:CALIBRATION_SERVICE_UUID characteristicUUID:CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID value:value];
}

- (void)writeValueToPeripheral:(NSString *)identifier serviceUUID:(NSString *)serviceUUID characteristicUUID:(NSString *)characteristicUUID value:(NSData *)value {
  NSLog(@"~Writing value to peripheral: %@, service: %@, characteristic: %@, data: %@", identifier, serviceUUID, characteristicUUID, value);
  for (CBPeripheral *peripheral in self.peripherals) {
    if ([peripheral.identifier.UUIDString isEqualToString:identifier]) {
      NSLog(@"~Found peripheral with UUID %@", identifier);
      for (CBService *service in peripheral.services) {
        if ([service.UUID.UUIDString isEqualToString:serviceUUID]) {
          NSLog(@"~Found service with UUID %@", serviceUUID);
          for (CBCharacteristic *characteristic in service.characteristics) {
            if ([characteristic.UUID.UUIDString isEqualToString:characteristicUUID]) {
              NSLog(@"~Found characteristic with UUID %@", characteristicUUID);
              [peripheral writeValue:value forCharacteristic:characteristic type:CBCharacteristicWriteWithResponse];
            }
          }
        }
      }
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
    if (deviceManager != nil && identifier != nil) {
      [deviceManager cancelPeripheralConnection:[NSString stringWithUTF8String:identifier]];
    }
  }

  void _DeviceManagerPowerOffPeripheral(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager powerOffPeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }

  void _DeviceManagerStartPeripheralCalibration(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager calibratePeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }
}
