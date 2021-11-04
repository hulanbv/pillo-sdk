#import "PilloFramework.h"

// External C library exposed to Unity.
extern "C" {
  PilloFramework* pilloFramework = nil;

  void PilloInitialize() {
    pilloFramework = [PilloFramework new];
    [pilloFramework initialize];
  }
}

// Pillo Framework implementation
@implementation PilloFramework

- (void)initialize {
  self.centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil options:nil];
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
      // TODO Invoke a Unity event.
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
    self.pilloPeripheral = peripheral;
    self.pilloPeripheral.delegate = self;
    [self.centralManager connectPeripheral:self.pilloPeripheral options:nil];
    // Stop scanning, we've found our Pillo.
    // TODO implement periodic scanning.
    [self.centralManager stopScan];
  }
}

// Delegate Method invoked when the Peripheral did connect.
- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
  NSLog(@"PILLO~ Connected to the %@ (%@)", peripheral.name, peripheral.identifier.UUIDString);
}

// Delegate Method invoked when the Peripheral did fail to connect.
- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
  NSLog(@"PILLO~ Error while connecting %@ (%@)", peripheral.name, peripheral.identifier.UUIDString);
}

// Delegate Method invoked when the Peripheral Manager's state did update.
- (void)peripheralManagerDidUpdateState:(nonnull CBPeripheralManager *)peripheral { }

@end
