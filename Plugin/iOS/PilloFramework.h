#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

// Interface for the PilloFramework Objective-C implementation.
 @interface PilloFramework : NSObject <CBCentralManagerDelegate, CBPeripheralManagerDelegate, CBPeripheralDelegate> { }

// Instance Properties.
@property (nonatomic, strong) CBCentralManager *centralManager;
@property (nonatomic, strong) CBPeripheral *peripheral;

// Instance Methods.
- (void)initialize;
- (void)invokeUnityCallback:(NSString *)methodName;
- (void)invokeUnityCallback:(NSString *)methodName parameter:(NSString *)parameter;

@end
