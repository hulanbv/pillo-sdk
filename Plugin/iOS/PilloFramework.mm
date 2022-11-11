#import "PilloFramework.h"

// Pillo Framework Implementation File
// Author: Jeffrey Lanters at Hulan

// External C library exposed to Unity.
extern "C" {
  PilloFramework* pilloFramework = nil;

  void PilloFrameworkInitialize() {
    if (pilloFramework == nil) {
      // If the pillo framework has not been initialized before. A new instance will be created here. Then the initialize method will be invoked.
      pilloFramework = [PilloFramework new];
      [pilloFramework initialize];
    }
  }
}

// Pillo Framework implementation.
@implementation PilloFramework

// Configuration.
#define PILLO_SERVICE_UUID @"579BA43D-A351-463D-92C7-911EC1B54E35"
#define PRESSURE_CHARACTERISTIC_UUID @"1470CA75-5D7E-4E16-A70D-D1476E8D0C6F"
#define CHARGE_STATE_CHARACTERISTIC_UUID @"22FEB891-0057-4A3E-AF5B-EC769849077C"
#define BATTERY_LEVEL_CHARACTERISTIC_UUID @"2A19"
#define SCAN_DURATION_SECONDS 2
#define SCAN_INTERVAL_SECONDS 10
#define MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION 2

- (void)initialize {
  // Instansatiate the central manager with a delegate to this class while also instansatiating the array which will hold the peripherals and the known peripheral UUIDs.
  self.centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil];
  self.peripherals = [NSMutableArray<CBPeripheral *> arrayWithCapacity:MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION];
  // Start the scan for peripherals routine.
  [self startScanningForPeripheralsRoutine];
}

// Starts scanning for peripherals for a set duration.
-(void)startScanningForPeripheralsRoutine {
  // If the central manager was already scanning,
  if ([self.centralManager isScanning] == true) {
    // If at least one peripheral is connected,
    if ([self.peripherals count] > 0) {
      // Scanning is stopped.
      [self.centralManager stopScan];
    }
    // A routine cycle will be scheduled.
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self startScanningForPeripheralsRoutine];
    });
  }
  // If the central manager was not scanning, and the maximum simulataneously allowed peripheral connections is not exceeded,
  else if ([self.peripherals count] < MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
    // Scanning for peripherals will start, the allow duplicate keys option is passed along in order to allow the Central Manager to discover Perhiperals that have been disconnected before. This is an expensive operation and should only be executed periodically.
    [self.centralManager scanForPeripheralsWithServices:nil options:[NSDictionary dictionaryWithObjectsAndKeys:@YES, CBCentralManagerScanOptionAllowDuplicatesKey, nil]];
    // After a set amount of time,
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_DURATION_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      // If at least one peripheral is connected,
      if ([self.peripherals count] > 0) {
        // Scanning is stopped.
        [self.centralManager stopScan];
      }
      // A routine cycle will be scheduled.
      dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_INTERVAL_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
        [self startScanningForPeripheralsRoutine];
      });
    });
  }
  // If the maximum simulaneously allowed peripheral connections is exceeded.
  else {
    // A routine cycle will be scheduled.
    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (SCAN_INTERVAL_SECONDS * NSEC_PER_SEC)), dispatch_get_main_queue(), ^(void) {
      [self startScanningForPeripheralsRoutine];
    });
  }
}

// Delegate Method invoked when the Center Manager's state did update.
- (void)centralManagerDidUpdateState:(nonnull CBCentralManager *)central {
  // The initialization state of the Central Manager is communicated to Unity.
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

// Delegate Method invoked when the a Peripheral was discovered.
- (void)centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary<NSString *,id> *)advertisementData RSSI:(NSNumber *)RSSI {
  // Retrieve the peripheral name from the advertisement data. If the name contains Pillo, we'll attempt to connect.
  if (![self.peripherals containsObject:peripheral] && [peripheral.name containsString:@"Pillo"]) {
    // This class instance will be set as its delegate, this it can invoke the corresponding delegates.
    peripheral.delegate = self;
    // The use of delegates require the Pillo Peripherals to be stored somewhere, it will be stored within the Peripherals array.
    [self.peripherals addObject:peripheral];
    // The central will attempt to connect to the Pillo Peripheral.
    [self.centralManager connectPeripheral:peripheral options:nil];
    // If this number of simulataneously connected peripherals is exceeded, we'll stop scanning immediately.
    if ([self.peripherals count] >= MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
      [self.centralManager stopScan];
    }
  }
}

// Delegate Method invoked when the Peripheral did connect.
- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
  [self invokeUnityCallback:@"OnPeripheralDidConnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  // Once the Pillo Peripheral is connected, we'll start discovering Services, we will pass nil here to request all Services be discovered.
  [peripheral discoverServices:nil];
}

// Delegate Method invoked when the Peripheral did disconnect.
- (void)centralManager:(CBCentralManager *)central didDisconnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidDisconnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [self.centralManager cancelPeripheralConnection:peripheral];
  // The Peripheral will be removed from the array of Peripherals.
  [self.peripherals removeObjectAtIndex:[self.peripherals indexOfObject:peripheral]];
}

// Delegate Method invoked when the Peripheral did fail to connect.
- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  [self invokeUnityCallback:@"OnPeripheralDidFailToConnect" payload:@{
    @"identifier": peripheral.identifier.UUIDString
  }];
  [self.centralManager cancelPeripheralConnection:peripheral];
  // The Peripheral will be removed from the array of Peripherals.
  [self.peripherals removeObjectAtIndex:[self.peripherals indexOfObject:peripheral]];
}

// Delegate Method invoked when a Service is discovered.
- (void)peripheral:(CBPeripheral *)peripheral didDiscoverServices:(NSError *)error {
  // Loop all the available Services within the Peripheral while asking them to discover its characteristics.
  for (CBService *service in peripheral.services) {
    [peripheral discoverCharacteristics:nil forService:service];
  }
}

// Delegate Method invoked when a Characteristic for a Service is discovered.
- (void)peripheral:(CBPeripheral *)peripheral didDiscoverCharacteristicsForService:(CBService *)service error:(NSError *)error {
  for (CBCharacteristic *characteristic in service.characteristics) {
    // We'll loop all of the Service's Characteristics in order to enable Notifications.
    [peripheral setNotifyValue:true forCharacteristic:characteristic];
  }
}

- (void)peripheral:(CBPeripheral *)peripheral didUpdateValueForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
  // Extract the data from the Characteristic's value property and convert it into raw data based on the Characteristic's UUID.
  NSData *rawData = characteristic.value;
  NSString *characteristicUUIDString = characteristic.UUID.UUIDString;
  // When the Characteristic's UUID matches the pressure's Characteristic UUID, we'll extract the value as its pressure which should contain an interger from 0 to 255. We'll forward this using a Unity callback.
  if ([characteristicUUIDString isEqualToString:PRESSURE_CHARACTERISTIC_UUID]) {
    uint32_t pressure = 0;
    [rawData getBytes:&pressure length:sizeof(pressure)];
    [self invokeUnityCallback:@"OnPeripheralPressureDidChange" payload:@{
      @"identifier": peripheral.identifier.UUIDString,
      @"pressure": @(pressure)
    }];
  }
  // When the Characteristic's UUID matches the battery level's Characteristic UUID, we'll extract the value as its battery level which should contain an interger from 0 to 100. We'll forward this using a Unity callback.
  else if ([characteristicUUIDString isEqualToString:BATTERY_LEVEL_CHARACTERISTIC_UUID]) {
    uint32_t batteryLevel = 0;
    [rawData getBytes:&batteryLevel length:sizeof(batteryLevel)];
    [self invokeUnityCallback:@"OnPeripheralBatteryLevelDidChange" payload:@{
      @"identifier": peripheral.identifier.UUIDString,
      @"batteryLevel": @(batteryLevel)
    }];
  }
  // When the Chracteristic's UUID matches the charge state's Characteristic UUID, we'll extract the value as its charge state. This value will be casted within Unity to an equal value from an enum.
  else if ([characteristicUUIDString isEqualToString:CHARGE_STATE_CHARACTERISTIC_UUID]) {
    uint32_t chargeState = 0;
    [rawData getBytes:&chargeState length:sizeof(chargeState)];
    [self invokeUnityCallback:@"OnPeripheralChargeStateDidChange" payload:@{
      @"identifier": peripheral.identifier.UUIDString,
      @"chargeState": @(chargeState)
    }];
  }
}

// Delegate Method invoked when the Peripheral Manager's state did update.
- (void)peripheralManagerDidUpdateState:(nonnull CBPeripheralManager *)peripheral { }

// Invokes a callback event on the Unity Scene to a specific Game Object.
- (void)invokeUnityCallback:(NSString *)methodName {
  UnitySendMessage("~PilloFrameworkCallbackListener", [methodName UTF8String], [@"" UTF8String]);
}

// Invokes a callback event on the Unity Scene to a specific Game Object with a string payload. This is the only type Unity accepts, to it might require a parse in order to be used.
- (void)invokeUnityCallback:(NSString *)methodName payload:(NSDictionary *)payload {
  NSError *error;
  NSData *payloadJsonData = [NSJSONSerialization dataWithJSONObject:payload options:kNilOptions error:&error];
  NSString *payloadJson = [[NSString alloc]initWithData:payloadJsonData encoding:NSUTF8StringEncoding];
  UnitySendMessage("~PilloFrameworkCallbackListener", [methodName UTF8String], [payloadJson UTF8String]);
}

@end
