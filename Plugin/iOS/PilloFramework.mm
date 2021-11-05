#import "PilloFramework.h"

// External C library exposed to Unity.
extern "C" {
  PilloFramework* pilloFramework = nil;

  void PilloFrameworkInitialize() {
    // TODO Make sure this wont be called twice
    pilloFramework = [PilloFramework new];
    [pilloFramework initialize];
  }
}

// Pillo Framework implementation
@implementation PilloFramework

// Definitions
#define PILLO_SERVICE_UUID @"579BA43D-A351-463D-92C7-911EC1B54E35"
#define PRESSURE_CHARACTERISTIC_UUID @"1470CA75-5D7E-4E16-A70D-D1476E8D0C6F"
#define CHARGE_CHARACTERISTIC_UUID @"22FEB891-0057-4A3E-AF5B-EC769849077C"
#define BATTERY_LEVEL_CHARACTERISTIC_UUID @"2A19"

- (void)initialize {
  self.centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil];
  [self invokeUnityCallback:@"OnDidInitialize"];
}

// Delegate Method invoked when the Center Manager's state did update.
- (void)centralManagerDidUpdateState:(nonnull CBCentralManager *)central {
  switch ([central state]) {
    case CBManagerStatePoweredOn:
      // When Bluetooth is available, we'll start scanning for Peripherals.
      [self.centralManager scanForPeripheralsWithServices:nil options:nil];
      break;
    default:
      // When Bluetooth is not available. We'll do nothing.
      [self invokeUnityCallback:@"OnBluetoothNotAvailable"];
      break;
  }
}

// Delegate Method invoked when the a Peripheral was discovered.
- (void)centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary<NSString *,id> *)advertisementData RSSI:(NSNumber *)RSSI {
  // Retrieve the peripheral name from the advertisement data. If the name
  // contains Pillo, we'll attempt to connect.
  if ([peripheral.name containsString:@"Pillo"]) {
    // We'll store the Peripheral in the Pillo Peripheral property. This is
    // required in order to use the delegates. Next set the Implementation as
    // the Peripheral's delegate. Then connect to the peripheral, when the
    // connection state changes an event will be raised.
    // TODO add support for multiple Pillo Peripherals.
    self.peripheral = peripheral;
    self.peripheral.delegate = self;
    [self.centralManager connectPeripheral:self.peripheral options:nil];
    // Stop scanning, we've found our Pillo.
    // TODO implement periodic scanning.
    [self.centralManager stopScan];
  }
}

// Delegate Method invoked when the Peripheral did connect.
- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
  // Once the Pillo Peripheral is connected, we'll start discovering Services.
  // We pass nil here to request all Services be discovered.
  [peripheral discoverServices:nil];
  [self invokeUnityCallback:@"OnConnectionSuccessful" parameter:peripheral.identifier.UUIDString];
}

// Delegate Method invoked when the Peripheral did fail to connect.
- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnConnectionFailed" parameter:peripheral.identifier.UUIDString];
}

// Delegate Method invoked when a Service is discovered.
- (void)peripheral:(CBPeripheral *)peripheral didDiscoverServices:(NSError *)error {
  // Loop all the available Services within the Peripheral.
  for (CBService *service in peripheral.services) {
    // We're going to discover the Characteristics for all Services.
    [peripheral discoverCharacteristics:nil forService:service];
  }
}

// Delegate Method invoked when a Characteristic for a Service is discovered.
- (void)peripheral:(CBPeripheral *)peripheral didDiscoverCharacteristicsForService:(CBService *)service error:(NSError *)error {
  for (CBCharacteristic *characteristic in service.characteristics) {
    // We'll loop all of the Service's Characteristics and enable Notifications.
    [self.peripheral setNotifyValue:YES forCharacteristic:characteristic];
  }
}

- (void)peripheral:(CBPeripheral *)peripheral didUpdateValueForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
  // Extract the data from the Characteristic's value property and convert it
  // into raw data based on the Characteristic's UUID.
  NSData *rawData = characteristic.value;
  NSString *characteristicUUIDString = characteristic.UUID.UUIDString;
  // When the Characteristic's UUID matches the battery level's Characteristic
  // UUID, we'll extract the value as its battery level which should contain
  // an interger from 0 to 100. We'll forward this using a Unity callback.
  if ([characteristicUUIDString isEqualToString:BATTERY_LEVEL_CHARACTERISTIC_UUID]) {
    uint32_t rawBatteryLevel = 0;
    [rawData getBytes:&rawBatteryLevel length:sizeof(rawBatteryLevel)];
    NSNumber *batteryLevel = [[NSNumber alloc]initWithUnsignedInt:rawBatteryLevel];
    [self invokeUnityCallback:@"OnBatteryLevelDidChange" parameter:[batteryLevel stringValue]];
  }
  // When the Characteristic's UUID matches the pressure's Characteristic UUID,
  // we'll extract the value as its pressure which should contain an interger
  // from 0 to 255. We'll forward this using a Unity callback.
  else if ([characteristicUUIDString isEqualToString:PRESSURE_CHARACTERISTIC_UUID]) {
    uint32_t rawPressure = 0;
    [rawData getBytes:&rawPressure length:sizeof(rawPressure)];
    NSNumber *pressure = [[NSNumber alloc]initWithUnsignedInt:rawPressure];
    [self invokeUnityCallback:@"OnPressureDidChange" parameter:[pressure stringValue]];
  }
}

// Delegate Method invoked when the Peripheral Manager's state did update.
- (void)peripheralManagerDidUpdateState:(nonnull CBPeripheralManager *)peripheral { }

// Invokes a callback event on the Unity Scene to a specific Game Object.
- (void)invokeUnityCallback:(NSString *)methodName {
  [self invokeUnityCallback:methodName parameter:@""];
}

// Invokes a callback event on the Unity Scene to a specific Game Object with a
// string parameter. This is the only type Unity accepts, to it might require
// a parse in order to be used.
- (void)invokeUnityCallback:(NSString *)methodName parameter:(NSString *)parameter {
  UnitySendMessage("~PilloFrameworkCallbackListener", [methodName UTF8String], [parameter UTF8String]);
  // NSLog(@"PILLO~ Invoking Unity Callback '%@' with param '%@'", methodName, parameter);
}

@end
