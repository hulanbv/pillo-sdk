//  PilloFramework.h
//  Created by Jeffrey Lanters on 1/11/2021.

// The header file of the Pillo Framework
#import "PilloFramework.h"

// External C Libraries exposed to Unity.
extern "C" {

    // Instance of the Pillo Framework implementation.
    PilloFramework *_pilloFramework = nil;
    
    // Logs a message to the Unity console.
    void _iOSBluetoothLELogString (NSString *message) {
        NSLog (@"%@", message);
    }
    
    void _iOSBluetoothLELog (char *message) {
        _iOSBluetoothLELogString ([NSString stringWithFormat:@"%s", message]);
    }
    
    void _iOSBluetoothLEInitialize (BOOL asCentral, BOOL asPeripheral) {
        _pilloFramework = [PilloFramework new];
        [_pilloFramework initialize:asCentral asPeripheral:asPeripheral];
    }
    
    void _iOSBluetoothLEDeInitialize () {
        if (_pilloFramework != nil) {
            [_pilloFramework deInitialize];
            [_pilloFramework release];
            _pilloFramework = nil;
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", "DeInitialized");
        }
    }
    
    void _iOSBluetoothLEPauseMessages (BOOL pause) {
        if (_pilloFramework != nil)
            [_pilloFramework pauseMessages:pause];
    }
    
    void _iOSBluetoothLEScanForPeripheralsWithServices (char *serviceUUIDsStringRaw, bool allowDuplicates, bool rssiOnly, bool clearPeripheralList, int recordType) {
        
        if (_pilloFramework != nil) {
            _pilloFramework._rssiOnly = rssiOnly;
            
            NSMutableArray *actualUUIDs = nil;
            
            if (serviceUUIDsStringRaw != nil) {
                NSString *serviceUUIDsString = [NSString stringWithFormat:@"%s", serviceUUIDsStringRaw];
                NSArray *serviceUUIDs = [serviceUUIDsString componentsSeparatedByString:@"|"];
                
                if (serviceUUIDs.count > 0) {
                    actualUUIDs = [[NSMutableArray alloc] init];
                    
                    for (NSString* sUUID in serviceUUIDs)
                        [actualUUIDs addObject:[CBUUID UUIDWithString:sUUID]];
                }
            }
            
            NSDictionary *options = nil;
            if (allowDuplicates)
                options = @{ CBCentralManagerScanOptionAllowDuplicatesKey: @YES };
            
            [_pilloFramework scanForPeripheralsWithServices:actualUUIDs options:options clearPeripheralList:clearPeripheralList recordType:recordType];
        }
    }

    void _iOSBluetoothLEStopScan () {
        if (_pilloFramework != nil)
            [_pilloFramework stopScan];
    }
    
    void _iOSBluetoothLERetrieveListOfPeripheralsWithServices (char *serviceUUIDsStringRaw) {
        if (_pilloFramework != nil) {
            NSMutableArray *actualUUIDs = nil;
            
            if (serviceUUIDsStringRaw != nil) {
                NSString *serviceUUIDsString = [NSString stringWithFormat:@"%s", serviceUUIDsStringRaw];
                NSArray *serviceUUIDs = [serviceUUIDsString componentsSeparatedByString:@"|"];
                
                if (serviceUUIDs.count > 0) {
                    actualUUIDs = [[NSMutableArray alloc] init];
                    
                    for (NSString* sUUID in serviceUUIDs)
                        [actualUUIDs addObject:[CBUUID UUIDWithString:sUUID]];
                }
            }
            
            [_pilloFramework retrieveListOfPeripheralsWithServices:actualUUIDs];
        }
    }
    
    void _iOSBluetoothLEConnectToPeripheral (char *name) {
        if (_pilloFramework && name != nil)
            [_pilloFramework connectToPeripheral:[NSString stringWithFormat:@"%s", name]];
    }
    
    void _iOSBluetoothLEDisconnectPeripheral (char *name) {
        if (_pilloFramework && name != nil)
            [_pilloFramework disconnectPeripheral:[NSString stringWithFormat:@"%s", name]];
    }
    
    void _iOSBluetoothLEReadCharacteristic (char *name, char *service, char *characteristic) {
        if (_pilloFramework && name != nil && service != nil && characteristic != nil)
            [_pilloFramework readCharacteristic:[NSString stringWithFormat:@"%s", name] service:[NSString stringWithFormat:@"%s", service] characteristic:[NSString stringWithFormat:@"%s", characteristic]];
    }
    
    void _iOSBluetoothLEWriteCharacteristic (char *name, char *service, char *characteristic, unsigned char *data, int length, BOOL withResponse) {
        if (_pilloFramework && name != nil && service != nil && characteristic != nil && data != nil && length > 0)
            [_pilloFramework writeCharacteristic:[NSString stringWithFormat:@"%s", name] service:[NSString stringWithFormat:@"%s", service] characteristic:[NSString stringWithFormat:@"%s", characteristic] data:[NSData dataWithBytes:data length:length] withResponse:withResponse];
    }
    
    void _iOSBluetoothLESubscribeCharacteristic (char *name, char *service, char *characteristic) {
        if (_pilloFramework && name != nil && service != nil && characteristic != nil)
            [_pilloFramework subscribeCharacteristic:[NSString stringWithFormat:@"%s", name] service:[NSString stringWithFormat:@"%s", service] characteristic:[NSString stringWithFormat:@"%s", characteristic]];
    }
    
    void _iOSBluetoothLEUnSubscribeCharacteristic (char *name, char *service, char *characteristic) {
        if (_pilloFramework && name != nil && service != nil && characteristic != nil)
            [_pilloFramework unsubscribeCharacteristic:[NSString stringWithFormat:@"%s", name] service:[NSString stringWithFormat:@"%s", service] characteristic:[NSString stringWithFormat:@"%s", characteristic]];
    }
    
    void _iOSBluetoothLEDisconnectAll () {
        if (_pilloFramework != nil)
            [_pilloFramework disconnectAll];
    }
}

@implementation PilloFramework

@synthesize _peripherals;
@synthesize _rssiOnly;

- (void)initialize:(BOOL)asCentral asPeripheral:(BOOL)asPeripheral {
    _mtu = 20;
    _isPaused = FALSE;
    _isInitializing = TRUE;
    _centralManager = nil;
    
    if (asCentral)
        _centralManager = [[CBCentralManager alloc] initWithDelegate:self queue:nil];
    
    _peripherals = [[NSMutableDictionary alloc] init];
}

- (void)deInitialize {
    if (_backgroundMessages != nil) {
        for (UnityMessage *message in _backgroundMessages) {
            if (message != nil) {
                [message deInitialize];
                [message release];
            }
        }
        
        [_backgroundMessages release];
        _backgroundMessages = nil;
    }
    
    if (_centralManager != nil)
        [self stopScan];
    
    [_peripherals removeAllObjects];
}

- (void)pauseMessages:(BOOL)isPaused {
    if (isPaused != _isPaused) {
        
        if (_backgroundMessages == nil)
            _backgroundMessages = [[NSMutableArray alloc] init];
        
        _isPaused = isPaused;
        
        // if we are not paused now since we know we changed state
        // that means we were paused so we need to pump the saved
        // messages to Unity
        if (isPaused) {
            if (_backgroundMessages != nil) {
                for (UnityMessage *message in _backgroundMessages) {
                    if (message != nil) {
                        [message sendUnityMessage];
                        [message deInitialize];
                        [message release];
                    }
                }
                
                [_backgroundMessages removeAllObjects];
            }
        }
    }
}

// central delegate implementation
- (void)scanForPeripheralsWithServices:(NSArray *)serviceUUIDs options:(NSDictionary *)options clearPeripheralList:(BOOL)clearPeripheralList recordType:(int)recordType {
    if (_centralManager != nil) {
        recordType = recordType;
        if (clearPeripheralList && _peripherals != nil)
            [_peripherals removeAllObjects];
        
        [_centralManager scanForPeripheralsWithServices:serviceUUIDs options:options];
    }
}

- (void) stopScan {
    if (_centralManager != nil)
        [_centralManager stopScan];
}

- (void)retrieveListOfPeripheralsWithServices:(NSArray *)serviceUUIDs {
    if (_centralManager != nil) {
        if (_peripherals != nil)
            [_peripherals removeAllObjects];
        
        NSArray * list = [_centralManager retrieveConnectedPeripheralsWithServices:serviceUUIDs];
        if (list != nil) {
            for (int i = 0; i < list.count; ++i) {
                CBPeripheral *peripheral = [list objectAtIndex:i];
                if (peripheral != nil) {
                    NSString *identifier = [[peripheral identifier] UUIDString];
                    NSString *name = [peripheral name];
                    NSString *message = [NSString stringWithFormat:@"RetrievedConnectedPeripheral~%@~%@", identifier, name];
                    UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
                    
                    [_peripherals setObject:peripheral forKey:identifier];
                }
            }
        }
    }
}

- (void)connectToPeripheral:(NSString *)name {
    if (_peripherals != nil && name != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil)
            [_centralManager connectPeripheral:peripheral options:nil];
    }
}

- (void)disconnectAll {
    if (_peripherals != nil && [_peripherals count] > 0) {
        NSArray* keys = [_peripherals allKeys];
        for(NSString* key in keys) {
            CBPeripheral *peripheral = [_peripherals objectForKey:key];
            if (peripheral != nil)
                [_centralManager cancelPeripheralConnection:peripheral];
        }
    }
}

- (void)disconnectPeripheral:(NSString *)name {
    if (_peripherals != nil && name != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil) {
            [_centralManager cancelPeripheralConnection:peripheral];
        }
    }
}

- (CBCharacteristic *)getCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString {
    CBCharacteristic *returnCharacteristic = nil;
    
    if (name != nil && serviceString != nil && characteristicString != nil && _peripherals != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil) {
            CBUUID *serviceUUID = [CBUUID UUIDWithString:serviceString];
            CBUUID *characteristicUUID = [CBUUID UUIDWithString:characteristicString];
            
            for (CBService *service in peripheral.services) {
                if ([service.UUID isEqual:serviceUUID]) {
                    for (CBCharacteristic *characteristic in service.characteristics) {
                        if ([characteristic.UUID isEqual:characteristicUUID]) {
                            returnCharacteristic = characteristic;
                        }
                    }
                }
            }
        }
    }
    
    return returnCharacteristic;
}

- (void)readCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString {
    if (name != nil && serviceString != nil && characteristicString != nil && _peripherals != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil) {
            CBCharacteristic *characteristic = [_pilloFramework getCharacteristic:name service:serviceString characteristic:characteristicString];
            if (characteristic != nil)
                [peripheral readValueForCharacteristic:characteristic];
        }
    }
}

- (void)writeCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString data:(NSData *)data withResponse:(BOOL)withResponse {
    if (name != nil && serviceString != nil && characteristicString != nil && _peripherals != nil && data != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil) {
            CBCharacteristic *characteristic = [_pilloFramework getCharacteristic:name service:serviceString characteristic:characteristicString];
            if (characteristic != nil) {
                CBCharacteristicWriteType type = CBCharacteristicWriteWithoutResponse;
                if (withResponse)
                    type = CBCharacteristicWriteWithResponse;
                
                [self writeCharactersticBytes:peripheral characteristic:characteristic data:data withResponse:type];
            }
        }
    }
}

- (void)subscribeCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString {
    if (name != nil && serviceString != nil && characteristicString != nil && _peripherals != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil) {
            CBCharacteristic *characteristic = [_pilloFramework getCharacteristic:name service:serviceString characteristic:characteristicString];
            if (characteristic != nil)
                [peripheral setNotifyValue:YES forCharacteristic:characteristic];
        }
    }
}

- (void)unsubscribeCharacteristic:(NSString *)name service:(NSString *)serviceString characteristic:(NSString *)characteristicString {
    if (name != nil && serviceString != nil && characteristicString != nil && _peripherals != nil) {
        CBPeripheral *peripheral = [_peripherals objectForKey:name];
        if (peripheral != nil) {
            CBCharacteristic *characteristic = [_pilloFramework getCharacteristic:name service:serviceString characteristic:characteristicString];
            if (characteristic != nil)
                [peripheral setNotifyValue:NO forCharacteristic:characteristic];
        }
    }
}

- (void)centralManagerDidUpdateState:(CBCentralManager *)central {
    switch (central.state) {
        case CBCentralManagerStateUnsupported: {
            NSLog(@"Central State: Unsupported");
            NSString *message = [NSString stringWithFormat:@"Error~Bluetooth LE Not Supported"];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        } break;
            
        case CBCentralManagerStateUnauthorized: {
            NSLog(@"Central State: Unauthorized");
            NSString *message = [NSString stringWithFormat:@"Error~Bluetooth LE Not Authorized"];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        } break;
            
        case CBCentralManagerStatePoweredOff: {
            NSLog(@"Central State: Powered Off");
            
            NSString *message = [NSString stringWithFormat:@"Error~Bluetooth LE Powered Off"];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        } break;
            
        case CBCentralManagerStatePoweredOn: {
            NSLog(@"Central State: Powered On");
            if (_isInitializing)
                UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", "Initialized");
            _isInitializing = FALSE;
        } break;
            
        case CBCentralManagerStateUnknown: {
            NSLog(@"Central State: Unknown");
            NSString *message = [NSString stringWithFormat:@"Error~Bluetooth LE Unknown"];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        } break;
            
        default: { }
    }
}

- (void)centralManager:(CBCentralManager *)central didRetrievePeripherals:(NSArray *)peripherals { }

- (void)centralManager:(CBCentralManager *)central didRetrieveConnectedPeripherals:(NSArray *)peripherals { }

- (void)centralManager:(CBCentralManager *)central didFailToConnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
    if (error) {
        NSString *message = [NSString stringWithFormat:@"Error~%@", error.description];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    }
}

- (void)centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary *)advertisementData RSSI:(NSNumber *)RSSI {
    if (_peripherals != nil && peripheral != nil) {
        NSString *name = [advertisementData objectForKey:CBAdvertisementDataLocalNameKey];
        if (name == nil)
            name = peripheral.name;
        if (name == nil)
            name = @"No Name";
        
        if (name != nil) {
            NSString *identifier = nil;
            
            NSString *foundPeripheral = [self findPeripheralName:peripheral];
            if (foundPeripheral == nil) {
                identifier = [[peripheral identifier] UUIDString];
            } else {
                identifier = foundPeripheral;
            }
            
            NSString *message = nil;
            
            if (advertisementData != nil && [advertisementData objectForKey:CBAdvertisementDataManufacturerDataKey] != nil) {
                NSData* bytes = [advertisementData objectForKey:CBAdvertisementDataManufacturerDataKey];
                message = [NSString stringWithFormat:@"DiscoveredPeripheral~%@~%@~%@~%@", identifier, name, RSSI, [PilloFramework base64StringFromData:bytes length:bytes.length]];
            } else if (RSSI != 0 && _rssiOnly) {
                message = [NSString stringWithFormat:@"DiscoveredPeripheral~%@~%@~%@~", identifier, name, RSSI];
            } else {
                message = [NSString stringWithFormat:@"DiscoveredPeripheral~%@~%@", identifier, name];
            }
            
            if (message != nil)
                UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
            
            [_peripherals setObject:peripheral forKey:identifier];
        }
    }
}

- (void)centralManager:(CBCentralManager *)central didDisconnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error {
    if (_peripherals != nil) {
        NSString *foundPeripheral = [self findPeripheralName:peripheral];
        if (foundPeripheral != nil) {
            NSString *message = [NSString stringWithFormat:@"DisconnectedPeripheral~%@", foundPeripheral];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
        }
    }
}

- (void)centralManager:(CBCentralManager *)central didConnectPeripheral:(CBPeripheral *)peripheral {
    NSString *foundPeripheral = [self findPeripheralName:peripheral];
    if (foundPeripheral != nil) {
        NSString *message = [NSString stringWithFormat:@"ConnectedPeripheral~%@", foundPeripheral];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
        peripheral.delegate = self;
        [peripheral discoverServices:nil];
    }
}

- (CBPeripheral *) findPeripheralInList:(CBPeripheral*)peripheral {
    CBPeripheral *foundPeripheral = nil;
    
    NSEnumerator *enumerator = [_peripherals keyEnumerator];
    id key;
    while ((key = [enumerator nextObject])) {
        CBPeripheral *tempPeripheral = [_peripherals objectForKey:key];
        if ([tempPeripheral isEqual:peripheral]) {
            foundPeripheral = tempPeripheral;
            break;
        }
    }
    
    return foundPeripheral;
}

- (NSString *) findPeripheralName:(CBPeripheral*)peripheral {
    NSString *foundPeripheral = nil;
    NSEnumerator *enumerator = [_peripherals keyEnumerator];
    id key;
    while ((key = [enumerator nextObject])) {
        CBPeripheral *tempPeripheral = [_peripherals objectForKey:key];
        if ([tempPeripheral isEqual:peripheral]) {
            foundPeripheral = key;
            break;
        }
    }
    
    return foundPeripheral;
}

// central peripheral delegate implementation
- (void)peripheral:(CBPeripheral *)peripheral didDiscoverServices:(NSError *)error {
    if (error) {
        NSString *message = [NSString stringWithFormat:@"Error~%@", error.description];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    } else {
        NSString *foundPeripheral = [self findPeripheralName:peripheral];
        if (foundPeripheral != nil) {
            for (CBService *service in peripheral.services) {
                NSString *message = [NSString stringWithFormat:@"DiscoveredService~%@~%@", foundPeripheral, [service UUID]];
                UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
                [peripheral discoverCharacteristics:nil forService:service];
            }
        }
    }
}

- (void)peripheral:(CBPeripheral *)peripheral didDiscoverCharacteristicsForService:(CBService *)service error:(NSError *)error {
    if (error) {
        NSString *message = [NSString stringWithFormat:@"Error~%@", error.description];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    } else {
        NSString *foundPeripheral = [self findPeripheralName:peripheral];
        if (foundPeripheral != nil) {
            for (CBCharacteristic *characteristic in service.characteristics) {
                NSString *message = [NSString stringWithFormat:@"DiscoveredCharacteristic~%@~%@~%@", foundPeripheral, [service UUID], [characteristic UUID]];
                UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
            }
        }
    }
}

- (void)peripheral:(CBPeripheral *)peripheral didUpdateValueForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
    if (error) {
        NSString *message = [NSString stringWithFormat:@"Error~%@", error.description];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    } else {
        NSString *foundPeripheral = [self findPeripheralName:peripheral];
        if (foundPeripheral != nil) {
            if (characteristic.value != nil) {
                NSString *message = [NSString stringWithFormat:@"DidUpdateValueForCharacteristic~%@~%@~%@", foundPeripheral, [characteristic UUID], [PilloFramework base64StringFromData:characteristic.value length:characteristic.value.length]];
                UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String]);
            }
        }
    }
}

- (void)writeCharactersticBytesReset {
    _writeCharacteristicBytes = nil;
    _writeCharacteristicLength = 0;
    _writeCharacteristicPosition = 0;
    _writeCharacteristicBytesToWrite = 0;
    _writeCharacteristicWithResponse = CBCharacteristicWriteWithResponse;
    _writeCharacteristicRetries = 3;
}

- (void)writeCharactersticBytes:(CBPeripheral *)peripheral characteristic:(CBCharacteristic *)characteristic data:(NSData *)data withResponse:(CBCharacteristicWriteType)withResponse {
    if (_writeCharacteristicBytes == nil && (_mtu == 0 || data.length > _mtu)) {
        _writeCharacteristicLength = data.length;
        _writeCharacteristicBytes = (unsigned char*)malloc(_writeCharacteristicLength);
        memcpy(_writeCharacteristicBytes, [data bytes], _writeCharacteristicLength);
        _writeCharacteristicPosition = 0;
        _writeCharacteristicWithResponse = withResponse;
        if (_mtu == 0) {
            _writeCharacteristicBytesToWrite = _writeCharacteristicLength;
        } else {
            _writeCharacteristicBytesToWrite = _mtu;
        }
    }

    NSLog(@"write characteristic block");

    if (_writeCharacteristicBytes != nil) {
        NSMutableData *newData = [NSMutableData dataWithCapacity:_writeCharacteristicBytesToWrite];
        [newData appendBytes:&_writeCharacteristicBytes[_writeCharacteristicPosition] length:_writeCharacteristicBytesToWrite];
        data = newData;
        NSLog(@"data: %@", data);
    }
    
    NSLog(@"writing %ld bytes, %ld with response", data.length, withResponse);
    [peripheral writeValue:data forCharacteristic:characteristic type:withResponse];
    
    if (withResponse == CBCharacteristicWriteWithoutResponse) {
        double delayInSeconds = 0.01;
        dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, delayInSeconds * NSEC_PER_SEC);
        dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
            [self writeNextPacket:peripheral characteristic:characteristic];
        });
    }
}

- (void)writeNextPacket:(CBPeripheral *)peripheral characteristic:(CBCharacteristic *)characteristic {
    if (_writeCharacteristicLength > _writeCharacteristicPosition + _writeCharacteristicBytesToWrite) {
        _writeCharacteristicPosition += _writeCharacteristicBytesToWrite;
        if (_writeCharacteristicPosition + _writeCharacteristicBytesToWrite > _writeCharacteristicLength)
            _writeCharacteristicBytesToWrite = _writeCharacteristicLength - _writeCharacteristicPosition;

        NSMutableData *data = [NSMutableData dataWithCapacity:_writeCharacteristicLength];
        [data appendBytes:_writeCharacteristicBytes length:_writeCharacteristicLength];
        [self writeCharactersticBytes:peripheral characteristic:characteristic data:data withResponse:_writeCharacteristicWithResponse];
    } else {
        [self writeCharactersticBytesReset];
        NSString *message = [NSString stringWithFormat:@"DidWriteCharacteristic~%@", characteristic.UUID];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    }
}

- (void)peripheral:(CBPeripheral *)peripheral didWriteValueForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
    if (error) {
        NSString *message = [NSString stringWithFormat:@"Error~%@", error.description];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    } else {
        NSLog(@"%ld bytes written, %ld position, %ld length, %ld with response", _writeCharacteristicBytesToWrite, _writeCharacteristicPosition, _writeCharacteristicLength, _writeCharacteristicWithResponse);
        
        if (_writeCharacteristicBytesToWrite > 0) {
            [self writeNextPacket:peripheral characteristic:characteristic];
        } else {
            NSString *message = [NSString stringWithFormat:@"DidWriteCharacteristic~%@", characteristic.UUID];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        }
    }
}

- (void)peripheral:(CBPeripheral *)peripheral didUpdateNotificationStateForCharacteristic:(CBCharacteristic *)characteristic error:(NSError *)error {
    if (error) {
        NSString *message = [NSString stringWithFormat:@"Error~%@", error.description];
        UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
    } else {
        NSString *foundPeripheral = [self findPeripheralName:peripheral];
        if (foundPeripheral != nil) {
            NSString *message = [NSString stringWithFormat:@"DidUpdateNotificationStateForCharacteristic~%@~%@", foundPeripheral, characteristic.UUID];
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        }
    }
}


- (void)sendUnityMessage:(BOOL)isString message:(NSString *)message {
    if (_isPaused) {
        if (_backgroundMessages != nil) {
            UnityMessage *unitymessage = [[UnityMessage alloc] init];
            if (unitymessage != nil) {
                [unitymessage initialize:isString message:message];
                [_backgroundMessages addObject:unitymessage];
            }
        }
    } else {
        if (isString) {
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [message UTF8String] );
        } else {
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothData", [message UTF8String] );
        }
    }
}

static char base64EncodingTable[64] = {
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
    'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
    'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
    'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
};

+ (NSString *) base64StringFromData: (NSData *)data length: (int)length {
    unsigned long ixtext, lentext;
    long ctremaining;
    unsigned char input[3], output[4];
    short i, charsonline = 0, ctcopy;
    const unsigned char *raw;
    NSMutableString *result;
    
    lentext = [data length];
    if (lentext < 1)
        return @"";
    result = [NSMutableString stringWithCapacity: lentext];
    raw = (const unsigned char *)[data bytes];
    ixtext = 0;
    
    while (true) {
        ctremaining = lentext - ixtext;
        if (ctremaining <= 0)
            break;
        for (i = 0; i < 3; i++) {
            unsigned long ix = ixtext + i;
            if (ix < lentext)
                input[i] = raw[ix];
            else
                input[i] = 0;
        }
        output[0] = (input[0] & 0xFC) >> 2;
        output[1] = ((input[0] & 0x03) << 4) | ((input[1] & 0xF0) >> 4);
        output[2] = ((input[1] & 0x0F) << 2) | ((input[2] & 0xC0) >> 6);
        output[3] = input[2] & 0x3F;
        ctcopy = 4;
        switch (ctremaining) {
            case 1:
                ctcopy = 2;
                break;
            case 2:
                ctcopy = 3;
                break;
        }
        
        for (i = 0; i < ctcopy; i++)
            [result appendString: [NSString stringWithFormat: @"%c", base64EncodingTable[output[i]]]];
        
        for (i = ctcopy; i < 4; i++)
            [result appendString: @"="];
        
        ixtext += 3;
        charsonline += 4;
        
        if ((length > 0) && (charsonline >= length))
            charsonline = 0;
    }
    return result;
}

#pragma mark Internal

@end

@implementation UnityMessage

- (void)initialize:(BOOL)isString message:(NSString *)message {
    _isString = isString;
    _message = [message copy];
}

- (void)deInitialize {
    if (_message != nil)
        [_message release];
    _message = nil;
}

- (void)sendUnityMessage {
    if (_message != nil) {
        if (_isString) {
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothMessage", [_message UTF8String] );
        } else {
            UnitySendMessage ("BluetoothLEReceiver", "OnBluetoothData", [_message UTF8String] );\
        }
    }
}

@end
