#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

// Pillo Framework Device Manager Header File
// Author: Jeffrey Lanters at Hulan
@interface DeviceManager : NSObject <CBCentralManagerDelegate, CBPeripheralManagerDelegate, CBPeripheralDelegate> {}

@property (nonatomic, strong) CBCentralManager *centralManager;
@property (nonatomic, strong) NSMutableArray<CBPeripheral *> *peripherals;

- (void)initialize;
- (void)invokeUnityCallback:(NSString *)methodName;
- (void)invokeUnityCallback:(NSString *)methodName payload:(NSDictionary *)payload;

@end
