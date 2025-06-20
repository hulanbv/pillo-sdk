#import "DeviceManager.h"

#define EXPORT_SYMBOL extern "C" __attribute__((visibility("default")))

DeviceManager *deviceManager = [[DeviceManager alloc] init];

EXPORT_SYMBOL void SetDelegates(OnCentralDidInitialize onCentralDidInitialize, OnCentralDidFailToInitialize onCentralDidFailToInitialize, OnCentralDidStartScanning onCentralDidStartScanning, OnCentralDidStopScanning onCentralDidStopScanning, OnPeripheralDidConnect onPeripheralDidConnect, OnPeripheralDidDisconnect onPeripheralDidDisconnect, OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange onPeripheralPressureDidChange, OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
  deviceManager->onCentralDidInitialize = onCentralDidInitialize;
  deviceManager->onCentralDidFailToInitialize = onCentralDidFailToInitialize;
  deviceManager->onCentralDidStartScanning = onCentralDidStartScanning;
  deviceManager->onCentralDidStopScanning = onCentralDidStopScanning;
  deviceManager->onPeripheralDidConnect = onPeripheralDidConnect;
  deviceManager->onPeripheralDidDisconnect = onPeripheralDidDisconnect;
  deviceManager->onPeripheralDidFailToConnect = onPeripheralDidFailToConnect;
  deviceManager->onPeripheralBatteryLevelDidChange = onPeripheralBatteryLevelDidChange;
  deviceManager->onPeripheralPressureDidChange = onPeripheralPressureDidChange;
  deviceManager->onPeripheralChargingStateDidChange = onPeripheralChargingStateDidChange;
  deviceManager->onPeripheralFirmwareVersionDidChange = onPeripheralFirmwareVersionDidChange;
  deviceManager->onPeripheralHardwareVersionDidChange = onPeripheralHardwareVersionDidChange;
  deviceManager->onPeripheralModelNumberDidChange = onPeripheralModelNumberDidChange;
  [deviceManager didFinalize];
}

EXPORT_SYMBOL void CancelPeripheralConnection(const char* identifier) {
  if (deviceManager == nil || identifier == nil) {
    return;
  }
  
  [deviceManager cancelPeripheralConnection:[NSString stringWithUTF8String:identifier]];
}

EXPORT_SYMBOL void PowerOffPeripheral(const char* identifier) {
  if (deviceManager == nil || identifier == nil) {
    return;
  }
  
  [deviceManager powerOffPeripheral:[NSString stringWithUTF8String:identifier]];
}

EXPORT_SYMBOL void ForcePeripheralLedOff(const char* identifier, bool enabled) {
  if (deviceManager == nil || identifier == nil) {
    return;
  }
  
  [deviceManager forcePeripheralLedOff:[NSString stringWithUTF8String:identifier] enabled:enabled];
}

EXPORT_SYMBOL void StartPeripheralCalibration(const char* identifier) {
  if (deviceManager == nil || identifier == nil) {
    return;
  }
  
  [deviceManager calibratePeripheral:[NSString stringWithUTF8String:identifier]];
}
