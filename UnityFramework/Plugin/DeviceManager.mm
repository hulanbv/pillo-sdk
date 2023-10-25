#import "DeviceManager.h"

extern "C" {
  DeviceManager* deviceManager = nil;

  void Instantiate(OnCentralDidInitialize onCentralDidInitialize, OnCentralDidFailToInitialize onCentralDidFailToInitialize, OnCentralDidStartScanning onCentralDidStartScanning, OnCentralDidStopScanning onCentralDidStopScanning, OnPeripheralDidConnect onPeripheralDidConnect, OnPeripheralDidDisconnect onPeripheralDidDisconnect, OnPeripheralDidFailToConnect onPeripheralDidFailToConnect, OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange, OnPeripheralPressureDidChange onPeripheralPressureDidChange, OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange, OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange, OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange, OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
    if (deviceManager != nil) {
      return;
    }
    deviceManager = [DeviceManager new];
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

  void ForcePeripheralLedOff(const char* identifier, bool enabled) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager forcePeripheralLedOff:[NSString stringWithUTF8String:identifier] enabled:enabled];
    }
  }

  void StartPeripheralCalibration(const char* identifier) {
    if (deviceManager != nil && identifier != nil) {
      [deviceManager calibratePeripheral:[NSString stringWithUTF8String:identifier]];
    }
  }
}
