#import "DeviceManager.h"

@implementation DeviceManager {
  CBCentralManager *centralManager;
  NSMutableArray<CBPeripheral *> *peripherals;
}

- (instancetype)init {
  self = [super init];
  if (self) {
//    centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil]; // TODO: Resolve this issue in a better way
    peripherals = [NSMutableArray<CBPeripheral *> arrayWithCapacity:MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION];
  }
  return self;
}


- (void)peripheralsScanningRoutine {
  if ([centralManager isScanning]) {
    if ([peripherals count] > 0) {
      [centralManager stopScan];
      onCentralDidStopScanning();
    }
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self peripheralsScanningRoutine];
    });
  } else if ([peripherals count] < MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
    [centralManager scanForPeripheralsWithServices:nil options:[NSDictionary dictionaryWithObjectsAndKeys:@YES, CBCentralManagerScanOptionAllowDuplicatesKey, nil]];
    onCentralDidStartScanning();
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      if ([self->peripherals count] > 0) {
        [self->centralManager stopScan];
        self->onCentralDidStopScanning();
      }
      dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_INTERVAL_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
        [self peripheralsScanningRoutine];
      });
    });
  } else {
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_INTERVAL_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self peripheralsScanningRoutine];
    });
  }
}

- (void)centralManagerDidUpdateState:(nonnull CBCentralManager *)central {
  switch ([central state]) {
    case CBManagerStatePoweredOn:
      onCentralDidInitialize();
      [self peripheralsScanningRoutine];
      break;
    default:
      onCentralDidFailToInitialize("Unable to initialize CoreBluetooth Central Manager");
      break;
  }
}

- (void)centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary<NSString *,id> *)advertisementData RSSI:(NSNumber *)RSSI {
  if (![peripherals containsObject:peripheral] && [peripheral.name containsString:@"Pillo"]) {
    peripheral.delegate = self;
    [peripherals addObject:peripheral];
    [centralManager connectPeripheral:peripheral options:nil];
    if ([peripherals count] >= MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
      [centralManager stopScan];
    }
  }
}

- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
  onPeripheralDidConnect([peripheral.identifier.UUIDString UTF8String]);
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
  onPeripheralDidDisconnect([peripheral.identifier.UUIDString UTF8String]);
  [centralManager cancelPeripheralConnection:peripheral]; // TODO: Is this necessary?
  if ([peripherals containsObject:peripheral]) {
    [peripherals removeObjectAtIndex:[peripherals indexOfObject:peripheral]];
  }
}

- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  onPeripheralDidFailToConnect([peripheral.identifier.UUIDString UTF8String]);
  [centralManager cancelPeripheralConnection:peripheral]; // TODO: Is this necessary?
  if ([peripherals containsObject:peripheral]) {
    [peripherals removeObjectAtIndex:[peripherals indexOfObject:peripheral]];
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
      [peripheral discoverCharacteristics:@[[CBUUID UUIDWithString:COMMAND_LED_CHARACTERISTIC_UUID]] forService:service];
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
      onPeripheralPressureDidChange([peripheral.identifier.UUIDString UTF8String], pressure);
    }
  } else if ([serviceUUID isEqualToString:BATTERY_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:BATTERY_LEVEL_CHARACTERISTIC_UUID]) {
      uint32_t batteryLevel = 0;
      [rawData getBytes:&batteryLevel length:sizeof(batteryLevel)];
      onPeripheralBatteryLevelDidChange([peripheral.identifier.UUIDString UTF8String], batteryLevel);
    }
  } else if ([serviceUUID isEqualToString:CHARGE_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:CHARGE_STATE_CHARACTERISTIC_UUID]) {
      uint32_t chargeState = 0;
      [rawData getBytes:&chargeState length:sizeof(chargeState)];
      onPeripheralChargingStateDidChange([peripheral.identifier.UUIDString UTF8String], chargeState);
    }
  } else if ([serviceUUID isEqualToString:DEVICEINFORMATION_SERVICE_UUID]) {
    if ([characteristicUUID isEqualToString:DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID]) {
      NSString *firmwareVersion = [[NSString alloc] initWithData:rawData encoding:NSUTF8StringEncoding];
      onPeripheralFirmwareVersionDidChange([peripheral.identifier.UUIDString UTF8String], [firmwareVersion UTF8String]);
    } else if ([characteristicUUID isEqualToString:DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID]) {
      NSString *hardwareVersion = [[NSString alloc] initWithData:rawData encoding:NSUTF8StringEncoding];
      onPeripheralHardwareVersionDidChange([peripheral.identifier.UUIDString UTF8String], [hardwareVersion UTF8String]);
    } else if ([characteristicUUID isEqualToString:DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID]) {
      NSString *modelNumber = [[NSString alloc] initWithData:rawData encoding:NSUTF8StringEncoding];
      onPeripheralModelNumberDidChange([peripheral.identifier.UUIDString UTF8String], [modelNumber UTF8String]);
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

- (void)cancelPeripheralConnection:(NSString *)identifier {
  for (CBPeripheral *peripheral in peripherals) {
    if ([peripheral.identifier.UUIDString isEqualToString:identifier]) {
      [centralManager cancelPeripheralConnection:peripheral];
      return;
    }
  }
}

- (void)powerOffPeripheral:(NSString *)identifier {
  NSData *value = [NSData dataWithBytes:(uint8_t[]){ 0x0F } length:1];
  
  [self writeValueToPeripheral:identifier serviceUUID:COMMAND_SERVICE_UUID characteristicUUID:COMMAND_COMMAND_CHARACTERISTIC_UUID value:value];
}

- (void)forcePeripheralLedOff:(NSString *)identifier enabled:(BOOL)enabled {
  uint8_t byteValue = enabled ? 0x01 : 0x00;
  NSData *value = [NSData dataWithBytes:&byteValue length:1];
  
  [self writeValueToPeripheral:identifier serviceUUID:COMMAND_SERVICE_UUID characteristicUUID:COMMAND_LED_CHARACTERISTIC_UUID value:value];
}

- (void)calibratePeripheral:(NSString *)identifier {
  NSData *value = [NSData dataWithBytes:(uint8_t[]){ 0x0F } length:1];
  
  [self writeValueToPeripheral:identifier serviceUUID:CALIBRATION_SERVICE_UUID characteristicUUID:CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID value:value];
}

- (void)writeValueToPeripheral:(NSString *)identifier serviceUUID:(NSString *)serviceUUID characteristicUUID:(NSString *)characteristicUUID value:(NSData *)value {
  for (CBPeripheral *peripheral in peripherals) {
    if (![peripheral.identifier.UUIDString isEqualToString:identifier]) {
      continue;
    }
    
    for (CBService *service in peripheral.services) {
      if (![service.UUID.UUIDString isEqualToString:serviceUUID]) {
        continue;
      }
      
      CBCharacteristic *targetCharacteristic = nil;
      
      for (CBCharacteristic *characteristic in service.characteristics) {
        if ([characteristic.UUID.UUIDString isEqualToString:characteristicUUID]) {
          targetCharacteristic = characteristic;
          break;
        }
      }
      
      if (targetCharacteristic) {
        [peripheral writeValue:value forCharacteristic:targetCharacteristic type:CBCharacteristicWriteWithResponse];
        return;
      }
    }
  }
}

- (void)tempFixForRacingConditionCrash_unityIsReady {
    centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil]; // TODO: Resolve this issue in a better way
}

@end
