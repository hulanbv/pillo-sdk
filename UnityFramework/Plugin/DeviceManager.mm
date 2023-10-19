#import "DeviceManager.h"

extern "C" {
  DeviceManager* deviceManager = nil;

  void Instantiate(OnCentralDidInitialize onCentralDidInitialize, OnCentralDidFailToInitialize onCentralDidFailToInitialize, OnCentralDidStartScanning onCentralDidStartScanning, OnCentralDidStopScanning onCentralDidStopScanning, OnPeripheralDidConnect onPeripheralDidConnect, OnPeripheralDidDisconnect onPeripheralDidDisconnect, OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange onPeripheralPressureDidChange, OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
    if (deviceManager == nil) {
      deviceManager = [DeviceManager new];
      [deviceManager initialize:onCentralDidInitialize onCentralDidFailToInitialize:onCentralDidFailToInitialize onCentralDidStartScanning:onCentralDidStartScanning onCentralDidStopScanning:onCentralDidStopScanning onPeripheralDidConnect:onPeripheralDidConnect onPeripheralDidDisconnect:onPeripheralDidDisconnect onPeripheralDidFailToConnect:onPeripheralDidFailToConnect onPeripheralBatteryLevelDidChange:onPeripheralBatteryLevelDidChange onPeripheralPressureDidChange:onPeripheralPressureDidChange onPeripheralChargingStateDidChange:onPeripheralChargingStateDidChange onPeripheralFirmwareVersionDidChange:onPeripheralFirmwareVersionDidChange onPeripheralHardwareVersionDidChange:onPeripheralHardwareVersionDidChange onPeripheralModelNumberDidChange:onPeripheralModelNumberDidChange];
    }
  }

  void CancelPeripheralConnection(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager cancelPeripheralConnection:[NSString stringWithUTF8String:identifier]];
    }
  }

  void PowerOffPeripheral(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager powerOffPeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }

  void ForceLedOff(const char* identifier, bool enabled) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager forceLedOff:[NSString stringWithUTF8String:identifier] enabled:enabled];
    }
  }

  void StartPeripheralCalibration(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager calibratePeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }
}
