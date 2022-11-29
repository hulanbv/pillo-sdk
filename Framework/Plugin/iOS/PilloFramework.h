#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

// Pillo Framework Header File
// Author: Jeffrey Lanters at Hulan

// Interface for the PilloFramework Objective-C implementation.
@interface PilloFramework : NSObject <CBCentralManagerDelegate, CBPeripheralManagerDelegate, CBPeripheralDelegate> {}

// Instance Properties.
@property (nonatomic, strong) CBCentralManager *centralManager;
@property (nonatomic, strong) NSMutableArray<CBPeripheral *> *peripherals;

// Instance Methods.
- (void)initialize;
- (void)invokeUnityCallback:(NSString *)methodName;
- (void)invokeUnityCallback:(NSString *)methodName payload:(NSDictionary *)payload;

@end
