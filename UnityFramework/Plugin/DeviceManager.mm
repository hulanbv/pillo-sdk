#import "DeviceManager.h"

extern "C" {
  DeviceManager* deviceManager = nil;

  void DeviceManagerInstantiate() {
    if (deviceManager == nil) {
      deviceManager = [DeviceManager new];
      [deviceManager initialize];
    }
  }

  void DeviceManagerCancelPeripheralConnection(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager cancelPeripheralConnection:[NSString stringWithUTF8String:identifier]];
    }
  }

  void DeviceManagerPowerOffPeripheral(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager powerOffPeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }

  void DeviceManagerForceLedOff(const char* identifier, bool enabled) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager forceLedOff:[NSString stringWithUTF8String:identifier] enabled:enabled];
    }
  }

  void DeviceManagerStartPeripheralCalibration(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager calibratePeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }
}
