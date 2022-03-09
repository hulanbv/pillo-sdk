#import "PilloFramework.h"

// Pillo Framework Implementation File
// Author: Jeffrey Lanters at Hulan

// External C library exposed to Unity.
extern "C" {
  PilloFramework* pilloFramework = nil;

  void PilloFrameworkInitialize() {
    if (pilloFramework == nil) {
      // If the pillo framework has not been initialized before. A new instance
      // will be created here. Then the initialize method will be invoked.
      pilloFramework = [PilloFramework new];
      [pilloFramework initialize];
    }
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
  // Instansatiate the central manager with a delegate to this class while also
  // instansatiating the array which will hold the peripherals.
  self.centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil];
  self.peripherals = [NSMutableArray<CBPeripheral *> new];
}

// Delegate Method invoked when the Center Manager's state did update.
- (void)centralManagerDidUpdateState:(nonnull CBCentralManager *)central {
  switch ([central state]) {
    case CBManagerStatePoweredOn:
      // When Bluetooth is available, we'll start scanning for Peripherals.
      [self.centralManager scanForPeripheralsWithServices:nil options:nil];
      [self invokeUnityCallback:@"OnCentralDidInitialize"];
      break;
    default:
      // When Bluetooth is not available, we'll just thrown a catch.
      [self invokeUnityCallback:@"OnCentralDidFailToInitialize" payload:@"Bluetooth is not available."];
      break;
  }
}

// Delegate Method invoked when the a Peripheral was discovered.
- (void)centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary<NSString *,id> *)advertisementData RSSI:(NSNumber *)RSSI {
  // Retrieve the peripheral name from the advertisement data. If the name
  // contains Pillo, we'll attempt to connect.
  // TODO -- check the UUID instead.
  if ([peripheral.name containsString:@"Pillo"]) {
    // The Pillo Peripheral will be stored within the Pillo Peripherals array.
    // The index in the array will be determained by the current number of
    // Pillo Peripheralss currently in the array.
    NSUInteger peripheralIndex = [self.peripherals count];
    [self.peripherals insertObject:peripheral atIndex:peripheralIndex];
    // When the Pillo Peripherals is stored in the array, this class will be set
    // as it's delegate. The use of delegates require the Pillo Peripherals to
    // be stored somewhere, this is why they are kept in the array.
    self.peripherals[peripheralIndex].delegate = self;
    // The central will attempt to connect to the Pillo Peripheral.
    [self.centralManager connectPeripheral:self.peripherals[peripheralIndex] options:nil];
  }
}

// Delegate Method invoked when the Peripheral did connect.
- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
  // Once the Pillo Peripheral is connected, we'll start discovering Services.
  // We pass nil here to request all Services be discovered.
  [peripheral discoverServices:nil];
  [self invokeUnityCallback:@"OnPeripheralDidConnect" payload:peripheral.identifier.UUIDString];
  // When two (or more) Pillo Peripherals are connected, scanning is stopped
  // since not more Pillo Peripherals can be connected at once. When one of
  // these Peripherals will disconnect, scanning will be resumed.
  // TODO -- This is temporary disabled, sometimes scanning stops to soon resulting in only one connected Pillo Peripheral.
  // if ([self.peripherals count] >= 2) {
  //   [self.centralManager stopScan];
  // }
}

// Delegate Method invoked when the Peripheral did disconnect.
- (void)centralManager:(CBCentralManager *)central didDisconnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidDisconnect" payload:peripheral.identifier.UUIDString];
  // TODO -- remove from the array of Pillo Peripherals
  // TODO -- start scanning again when there are less than 2 connected Pillo Peripherals
  // HACK -- this propably doesnt work since peripheral is a new object????
  NSUInteger peripheralIndex = [self.peripherals indexOfObject:peripheral];
  [self.peripherals removeObjectAtIndex:peripheralIndex];
}

// Delegate Method invoked when the Peripheral did fail to connect.
- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidFailToConnect"];
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
    [peripheral setNotifyValue:YES forCharacteristic:characteristic];
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
    // TODO -- directly send this data as a string instead of parsing it around!
    uint32_t rawBatteryLevel = 0;
    [rawData getBytes:&rawBatteryLevel length:sizeof(rawBatteryLevel)];
    NSNumber *batteryLevel = [[NSNumber alloc]initWithUnsignedInt:rawBatteryLevel];
    NSString *payload = [NSString stringWithFormat:@"%@~%@", peripheral.identifier.UUIDString, batteryLevel];
    [self invokeUnityCallback:@"OnPeripheralBatteryLevelDidChange" payload:payload];
  }
  // When the Characteristic's UUID matches the pressure's Characteristic UUID,
  // we'll extract the value as its pressure which should contain an interger
  // from 0 to 255. We'll forward this using a Unity callback.
  else if ([characteristicUUIDString isEqualToString:PRESSURE_CHARACTERISTIC_UUID]) {
    // TODO -- directly send this data as a string instead of parsing it around!
    uint32_t rawPressure = 0;
    [rawData getBytes:&rawPressure length:sizeof(rawPressure)];
    NSNumber *pressure = [[NSNumber alloc]initWithUnsignedInt:rawPressure];
    NSString *payload = [NSString stringWithFormat:@"%@~%@", peripheral.identifier.UUIDString, pressure];
    [self invokeUnityCallback:@"OnPeripheralPressureDidChange" payload:payload];
  }
}

// Delegate Method invoked when the Peripheral Manager's state did update.
- (void)peripheralManagerDidUpdateState:(nonnull CBPeripheralManager *)peripheral { }

// Invokes a callback event on the Unity Scene to a specific Game Object.
- (void)invokeUnityCallback:(NSString *)methodName {
  [self invokeUnityCallback:methodName payload:@""];
}

// Invokes a callback event on the Unity Scene to a specific Game Object with a
// string payload. This is the only type Unity accepts, to it might require
// a parse in order to be used.
- (void)invokeUnityCallback:(NSString *)methodName payload:(NSString *)payload {
  UnitySendMessage("~PilloFrameworkCallbackListener", [methodName UTF8String], [payload UTF8String]);
}

@end
