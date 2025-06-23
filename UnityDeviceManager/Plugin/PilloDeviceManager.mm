//
//  PilloDeviceManager.mm
//  Pillo SDK Device Manager
//  Plugin
//
//  Created by Jeffrey Lanters on 14/08/2024.
//

#import "PilloDeviceManager.h"

#define PILLO_EXPORT_SYMBOL extern "C" __attribute__((visibility("default")))

PilloDeviceManager *pilloDeviceManager = [[PilloDeviceManager alloc] init];

PILLO_EXPORT_SYMBOL void PilloDeviceManagerSetDelegates(OnCentralDidInitialize onCentralDidInitialize, OnCentralDidFailToInitialize onCentralDidFailToInitialize, OnCentralDidStartScanning onCentralDidStartScanning, OnCentralDidStopScanning onCentralDidStopScanning, OnPeripheralDidConnect onPeripheralDidConnect, OnPeripheralDidDisconnect onPeripheralDidDisconnect, OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange onPeripheralPressureDidChange, OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
  pilloDeviceManager->onCentralDidInitialize = onCentralDidInitialize;
  pilloDeviceManager->onCentralDidFailToInitialize = onCentralDidFailToInitialize;
  pilloDeviceManager->onCentralDidStartScanning = onCentralDidStartScanning;
  pilloDeviceManager->onCentralDidStopScanning = onCentralDidStopScanning;
  pilloDeviceManager->onPeripheralDidConnect = onPeripheralDidConnect;
  pilloDeviceManager->onPeripheralDidDisconnect = onPeripheralDidDisconnect;
  pilloDeviceManager->onPeripheralDidFailToConnect = onPeripheralDidFailToConnect;
  pilloDeviceManager->onPeripheralBatteryLevelDidChange = onPeripheralBatteryLevelDidChange;
  pilloDeviceManager->onPeripheralPressureDidChange = onPeripheralPressureDidChange;
  pilloDeviceManager->onPeripheralChargingStateDidChange = onPeripheralChargingStateDidChange;
  pilloDeviceManager->onPeripheralFirmwareVersionDidChange = onPeripheralFirmwareVersionDidChange;
  pilloDeviceManager->onPeripheralHardwareVersionDidChange = onPeripheralHardwareVersionDidChange;
  pilloDeviceManager->onPeripheralModelNumberDidChange = onPeripheralModelNumberDidChange;
}

PILLO_EXPORT_SYMBOL void PilloDeviceManagerStartService() {
  if (pilloDeviceManager == nil) {
    return;
  }
  
  [pilloDeviceManager startService];
}

PILLO_EXPORT_SYMBOL void PilloDeviceManagerCancelPeripheralConnection(const char* identifier) {
  if (pilloDeviceManager == nil || identifier == nil) {
    return;
  }
  
  [pilloDeviceManager cancelPeripheralConnection:[NSString stringWithUTF8String:identifier]];
}

PILLO_EXPORT_SYMBOL void PilloDeviceManagerPowerOffPeripheral(const char* identifier) {
  if (pilloDeviceManager == nil || identifier == nil) {
    return;
  }
  
  [pilloDeviceManager powerOffPeripheral:[NSString stringWithUTF8String:identifier]];
}

PILLO_EXPORT_SYMBOL void PilloDeviceManagerForcePeripheralLedOff(const char* identifier, bool enabled) {
  if (pilloDeviceManager == nil || identifier == nil) {
    return;
  }
  
  [pilloDeviceManager forcePeripheralLedOff:[NSString stringWithUTF8String:identifier] enabled:enabled];
}

PILLO_EXPORT_SYMBOL void PilloDeviceManagerStartPeripheralCalibration(const char* identifier) {
  if (pilloDeviceManager == nil || identifier == nil) {
    return;
  }
  
  [pilloDeviceManager calibratePeripheral:[NSString stringWithUTF8String:identifier]];
}
