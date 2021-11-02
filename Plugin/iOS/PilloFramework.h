// PilloFramework.h
// Created by Jeffrey Lanters on 1/11/2021.

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

// Interface for the PilloFramework implementation
@interface PilloFramework : NSObject <CBCentralManagerDelegate, CBPeripheralManagerDelegate, CBPeripheralDelegate> {
    CBCentralManager *_centralManager;
    NSMutableDictionary *_peripherals;
    NSMutableArray *_backgroundMessages;
    BOOL _isPaused;
    BOOL _alreadyNotified;
    BOOL _isInitializing;
    BOOL _rssiOnly;
    int  _recordType;
    long _mtu;
    unsigned char *_writeCharacteristicBytes;
    long _writeCharacteristicLength;
    long _writeCharacteristicPosition;
    long _writeCharacteristicBytesToWrite;
    CBCharacteristicWriteType _writeCharacteristicWithResponse;
    int _writeCharacteristicRetries;
}

// Properties
@property (atomic, strong) NSMutableDictionary *_peripherals;
@property (atomic) BOOL _rssiOnly;

// Methods
- (void)initialize:(BOOL)asCentral asPeripheral:(BOOL)asPeripheral;
- (void)deInitialize;
- (void)scanForPeripheralsWithServices:(NSArray *)serviceUUIDs options:(NSDictionary *)options clearPeripheralList:(BOOL)clearPeripheralList recordType:(int)recordType;
- (void)stopScan;
- (void)retrieveListOfPeripheralsWithServices:(NSArray *)serviceUUIDs;
- (void)connectToPeripheral:(NSString *)name;
- (void)disconnectPeripheral:(NSString *)name;
- (CBCharacteristic *)getCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString;
- (void)readCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString;
- (void)writeCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString data:(NSData *)data withResponse:(BOOL)withResponse;
- (void)subscribeCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString;
- (void)unsubscribeCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString;
- (void)writeCharactersticBytesReset;
- (void)writeCharactersticBytes:(CBPeripheral *)peripheral characteristic:(CBCharacteristic *)characteristic data:(NSData *)data withResponse:(CBCharacteristicWriteType)withResponse;
- (void)writeNextPacket:(CBPeripheral *)peripheral characteristic:(CBCharacteristic *)characteristic;
- (void)pauseMessages:(BOOL)isPaused;
- (void)sendUnityMessage:(BOOL)isString message:(NSString *)message;

// Static methods
+ (NSString *) base64StringFromData:(NSData *)data length:(int)length;

// End of the PilloFramework implementation interface
@end

// Interface for the UnityMessage implementation
@interface UnityMessage : NSObject {
    BOOL _isString;
    NSString *_message;
}

// Methods
- (void)initialize:(BOOL)isString message:(NSString *)message;
- (void)deInitialize;
- (void)sendUnityMessage;

// End of the UnityMessage implementation interface
@end
