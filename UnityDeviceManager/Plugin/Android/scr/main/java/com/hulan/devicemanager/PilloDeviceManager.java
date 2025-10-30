package com.hulan.devicemanager;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.BluetoothManager;
import android.bluetooth.BluetoothProfile;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanFilter;
import android.bluetooth.le.ScanResult;
import android.bluetooth.le.ScanSettings;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Build;
import android.os.Handler;
import android.os.Looper;
import android.os.ParcelUuid;
import android.util.Log;
import android.util.SparseArray;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Queue;
import java.util.UUID;

public class PilloDeviceManager {
    private static final String TAG = "PilloDeviceManager";
    
    private static final String DEVICEINFORMATION_SERVICE_UUID = "0000180A-0000-1000-8000-00805F9B34FB";
    private static final String DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID = "00002A24-0000-1000-8000-00805F9B34FB";
    private static final String DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID = "00002A26-0000-1000-8000-00805F9B34FB";
    private static final String DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID = "00002A27-0000-1000-8000-00805F9B34FB";
    private static final String BATTERY_SERVICE_UUID = "0000180F-0000-1000-8000-00805F9B34FB";
    private static final String BATTERY_LEVEL_CHARACTERISTIC_UUID = "00002A19-0000-1000-8000-00805F9B34FB";
    private static final String PRESSURE_SERVICE_UUID = "579BA43D-A351-463D-92C7-911EC1B54E35";
    private static final String PRESSURE_VALUE_CHARACTERISTIC_UUID = "1470CA75-5D7E-4E16-A70D-D1476E8D0C6F";
    private static final String CHARGE_SERVICE_UUID = "044402A3-F8B4-479A-B995-63E99ACB2735";
    private static final String CHARGE_STATE_CHARACTERISTIC_UUID = "22FEB891-0057-4A3E-AF5B-EC769849077C";
    private static final String COMMAND_SERVICE_UUID = "6ACCCABD-1728-4697-9B4A-BF25ECCA14AA";
    private static final String COMMAND_COMMAND_CHARACTERISTIC_UUID = "A9147E1F-E91F-4A02-B6E4-2869E0FE69BB";
    private static final String COMMAND_LED_CHARACTERISTIC_UUID = "7B3B969D-316A-450E-BDB9-6F1792270FA1";
    private static final String CALIBRATION_SERVICE_UUID = "7E238267-146F-461C-8615-39B358A428A5";
    private static final String CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID = "46F9AB5B-D01A-4353-9DB4-176C4F3200CF";
    private static final String HANDSHAKE_SERVICE_UUID = "35865C86-7B91-4834-B44A-8A66985D1375";
    private static final String HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID = "45C30C15-4815-4CDF-9ED3-9CC488492F4F";
    
    private static final int SCAN_DURATION_SECONDS = 2;
    private static final int SCAN_INTERVAL_SECONDS = 10;
    private static final int MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION = 2;
    private static final int BLUETOOTH_RETRY_DELAY_MS = 2000;
    private static final int MAX_BLUETOOTH_RETRIES = 5;
    private static final int PERMISSION_REQUEST_CODE = 1001;
    
    private static PilloDeviceManager instance;
    private Context context;
    private BluetoothAdapter bluetoothAdapter;
    private BluetoothLeScanner bluetoothLeScanner;
    private Handler mainHandler;
    private Handler scanHandler;
    
    // Connected devices
    private Map<String, BluetoothGatt> connectedDevices = new HashMap<>();
    private Map<String, BluetoothDevice> discoveredDevices = new HashMap<>();
    
    // Retry tracking
    private int bluetoothRetryCount = 0;
    private boolean isRetryingBluetooth = false;
    
    // Queue for serializing BLE operations (Android requires one operation at a time)
    private Queue<Runnable> bleOperationQueue = new LinkedList<>();
    private boolean isBleOperationInProgress = false;
    
    // Callbacks
    private static OnCentralDidInitialize onCentralDidInitialize;
    private static OnCentralDidFailToInitialize onCentralDidFailToInitialize;
    private static OnCentralDidStartScanning onCentralDidStartScanning;
    private static OnCentralDidStopScanning onCentralDidStopScanning;
    private static OnPeripheralDidConnect onPeripheralDidConnect;
    private static OnPeripheralDidDisconnect onPeripheralDidDisconnect;
    private static OnPeripheralDidFailToConnect onPeripheralDidFailToConnect;
    private static OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange;
    private static OnPeripheralPressureDidChange onPeripheralPressureDidChange;
    private static OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange;
    private static OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange;
    private static OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange;
    private static OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange;
    
    // Callback interfaces
    public interface OnCentralDidInitialize {
        void onCentralDidInitialize();
    }
    
    public interface OnCentralDidFailToInitialize {
        void onCentralDidFailToInitialize(String message);
    }
    
    public interface OnCentralDidStartScanning {
        void onCentralDidStartScanning();
    }
    
    public interface OnCentralDidStopScanning {
        void onCentralDidStopScanning();
    }
    
    public interface OnPeripheralDidConnect {
        void onPeripheralDidConnect(String identifier);
    }
    
    public interface OnPeripheralDidDisconnect {
        void onPeripheralDidDisconnect(String identifier);
    }
    
    public interface OnPeripheralDidFailToConnect {
        void onPeripheralDidFailToConnect(String identifier);
    }
    
    public interface OnPeripheralBatteryLevelDidChange {
        void onPeripheralBatteryLevelDidChange(String identifier, int batteryLevel);
    }
    
    public interface OnPeripheralPressureDidChange {
        void onPeripheralPressureDidChange(String identifier, int pressure);
    }
    
    public interface OnPeripheralChargingStateDidChange {
        void onPeripheralChargingStateDidChange(String identifier, int chargingState);
    }
    
    public interface OnPeripheralFirmwareVersionDidChange {
        void onPeripheralFirmwareVersionDidChange(String identifier, String firmwareVersion);
    }
    
    public interface OnPeripheralHardwareVersionDidChange {
        void onPeripheralHardwareVersionDidChange(String identifier, String hardwareVersion);
    }
    
    public interface OnPeripheralModelNumberDidChange {
        void onPeripheralModelNumberDidChange(String identifier, String modelNumber);
    }
    
    private PilloDeviceManager(Context context) {
        this.context = context;
        this.mainHandler = new Handler(Looper.getMainLooper());
        this.scanHandler = new Handler(Looper.getMainLooper());
        
        BluetoothManager bluetoothManager = (BluetoothManager) context.getSystemService(Context.BLUETOOTH_SERVICE);
        if (bluetoothManager != null) {
            this.bluetoothAdapter = bluetoothManager.getAdapter();
            if (bluetoothAdapter != null) {
                this.bluetoothLeScanner = bluetoothAdapter.getBluetoothLeScanner();
            }
        }
    }
    
    public static synchronized PilloDeviceManager getInstance(Context context) {
        if (instance == null) {
            instance = new PilloDeviceManager(context);
        }
        return instance;
    }
    
    public void startService() {
        Log.d(TAG, "Starting Pillo Device Manager service (attempt " + (bluetoothRetryCount + 1) + ")");
        
        if (bluetoothAdapter == null) {
            Log.e(TAG, "Bluetooth adapter is null");
            if (onCentralDidFailToInitialize != null) {
                onCentralDidFailToInitialize.onCentralDidFailToInitialize("Bluetooth not available");
            }
            return;
        }
        
        // Check if we have the necessary permissions first
        String[] missingPermissions = getMissingPermissions();
        if (missingPermissions.length > 0) {
            Log.w(TAG, "Missing permissions, requesting them...");
            requestPermissions(missingPermissions);
            return;
        }
        
        // Check if Bluetooth is enabled
        if (!bluetoothAdapter.isEnabled()) {
            if (bluetoothRetryCount < MAX_BLUETOOTH_RETRIES && !isRetryingBluetooth) {
                Log.w(TAG, "Bluetooth is not enabled, requesting to enable it...");
                requestBluetoothEnable();
                return;
            } else {
                Log.e(TAG, "Bluetooth is not enabled and max retries reached");
                if (onCentralDidFailToInitialize != null) {
                    onCentralDidFailToInitialize.onCentralDidFailToInitialize("Bluetooth not enabled. Please enable Bluetooth in settings.");
                }
                return;
            }
        }
        
        // Reset retry count on successful start
        bluetoothRetryCount = 0;
        isRetryingBluetooth = false;
        
        Log.d(TAG, "Starting Pillo Device Manager service");
        if (onCentralDidInitialize != null) {
            onCentralDidInitialize.onCentralDidInitialize();
        }
        
        startScanning();
    }
    
    private void requestPermissions(String[] permissions) {
        if (context instanceof Activity) {
            Activity activity = (Activity) context;
            activity.requestPermissions(permissions, PERMISSION_REQUEST_CODE);
        } else {
            Log.e(TAG, "Context is not an Activity, cannot request permissions");
            if (onCentralDidFailToInitialize != null) {
                onCentralDidFailToInitialize.onCentralDidFailToInitialize("Cannot request permissions: context is not an Activity");
            }
        }
    }
    
    private void requestBluetoothEnable() {
        if (context instanceof Activity) {
            Activity activity = (Activity) context;
            Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            activity.startActivityForResult(enableBtIntent, PERMISSION_REQUEST_CODE + 1);
            isRetryingBluetooth = true;
            
            // Set up retry timer
            mainHandler.postDelayed(() -> {
                if (!bluetoothAdapter.isEnabled()) {
                    bluetoothRetryCount++;
                    Log.w(TAG, "Bluetooth still not enabled, retrying... (attempt " + bluetoothRetryCount + ")");
                    startService();
                }
            }, BLUETOOTH_RETRY_DELAY_MS);
        } else {
            Log.e(TAG, "Context is not an Activity, cannot request Bluetooth enable");
            if (onCentralDidFailToInitialize != null) {
                onCentralDidFailToInitialize.onCentralDidFailToInitialize("Cannot enable Bluetooth: context is not an Activity");
            }
        }
    }
    
    // Method to handle permission results (should be called from Unity)
    public void onPermissionResult(int requestCode, String[] permissions, int[] grantResults) {
        if (requestCode == PERMISSION_REQUEST_CODE) {
            boolean allPermissionsGranted = true;
            for (int result : grantResults) {
                if (result != PackageManager.PERMISSION_GRANTED) {
                    allPermissionsGranted = false;
                    break;
                }
            }
            
            if (allPermissionsGranted) {
                Log.d(TAG, "All permissions granted, retrying startService");
                startService();
            } else {
                Log.e(TAG, "Some permissions were denied");
                if (onCentralDidFailToInitialize != null) {
                    onCentralDidFailToInitialize.onCentralDidFailToInitialize("Required permissions were denied. Please grant permissions in app settings.");
                }
            }
        }
    }
    
    // Method to handle Bluetooth enable result (should be called from Unity)
    public void onBluetoothEnableResult(boolean enabled) {
        isRetryingBluetooth = false;
        if (enabled) {
            Log.d(TAG, "Bluetooth enabled, retrying startService");
            startService();
        } else {
            Log.e(TAG, "Bluetooth enable was denied");
            if (onCentralDidFailToInitialize != null) {
                onCentralDidFailToInitialize.onCentralDidFailToInitialize("Bluetooth enable was denied. Please enable Bluetooth in settings.");
            }
        }
    }
    
    private String[] getMissingPermissions() {
        List<String> requiredPermissions = new ArrayList<>();
        List<String> missingPermissions = new ArrayList<>();
        
        // Always required permissions
        requiredPermissions.add("android.permission.BLUETOOTH");
        requiredPermissions.add("android.permission.BLUETOOTH_ADMIN");
        
        // Android 12+ (API 31+) requires new BLE permissions
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            requiredPermissions.add("android.permission.BLUETOOTH_CONNECT");
            requiredPermissions.add("android.permission.BLUETOOTH_SCAN");
            
            // Note: We use usesPermissionFlags="neverForLocation" for BLUETOOTH_SCAN
            // so no location permissions are required
        } else {
            // For Android 11 and below, we still need location permission for Bluetooth scanning
            requiredPermissions.add("android.permission.ACCESS_FINE_LOCATION");
            requiredPermissions.add("android.permission.ACCESS_COARSE_LOCATION");
        }
        
        // Check each permission
        for (String permission : requiredPermissions) {
            try {
                if (context.checkSelfPermission(permission) != android.content.pm.PackageManager.PERMISSION_GRANTED) {
                    missingPermissions.add(permission);
                    Log.w(TAG, "Missing permission: " + permission);
                }
            } catch (Exception e) {
                // Some permissions might not exist on older Android versions
                Log.w(TAG, "Could not check permission " + permission + ": " + e.getMessage());
            }
        }
        
        // If we're on Android 12+ and both location permissions are missing, only report one
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            boolean hasFineLocation = !missingPermissions.contains("android.permission.ACCESS_FINE_LOCATION");
            boolean hasCoarseLocation = !missingPermissions.contains("android.permission.ACCESS_COARSE_LOCATION");
            if (!hasFineLocation && missingPermissions.contains("android.permission.ACCESS_COARSE_LOCATION")) {
                missingPermissions.remove("android.permission.ACCESS_COARSE_LOCATION");
            }
        }
        
        return missingPermissions.toArray(new String[0]);
    }
    
    public void stopService() {
        stopScanning();
        
        // Disconnect all connected devices
        for (BluetoothGatt gatt : connectedDevices.values()) {
            gatt.disconnect();
            gatt.close();
        }
        connectedDevices.clear();
        discoveredDevices.clear();
    }
    
    private void startScanning() {
        if (bluetoothLeScanner == null) {
            Log.e(TAG, "BluetoothLeScanner is null");
            return;
        }
        
        if (onCentralDidStartScanning != null) {
            onCentralDidStartScanning.onCentralDidStartScanning();
        }
        
        // Create scan settings for better compatibility
        ScanSettings.Builder settingsBuilder = new ScanSettings.Builder();
        settingsBuilder.setScanMode(ScanSettings.SCAN_MODE_LOW_LATENCY);
        settingsBuilder.setCallbackType(ScanSettings.CALLBACK_TYPE_ALL_MATCHES);
        settingsBuilder.setMatchMode(ScanSettings.MATCH_MODE_AGGRESSIVE);
        settingsBuilder.setNumOfMatches(ScanSettings.MATCH_NUM_ONE_ADVERTISEMENT);
        settingsBuilder.setReportDelay(0L);
        
        ScanSettings scanSettings = settingsBuilder.build();
        
        // Start scanning for BLE devices with settings
        bluetoothLeScanner.startScan(null, scanSettings, scanCallback);
        
        // Stop scanning after duration
        scanHandler.postDelayed(this::stopScanning, SCAN_DURATION_SECONDS * 1000);
    }
    
    private void stopScanning() {
        if (bluetoothLeScanner != null) {
            bluetoothLeScanner.stopScan(scanCallback);
        }
        
        if (onCentralDidStopScanning != null) {
            onCentralDidStopScanning.onCentralDidStopScanning();
        }
        
        // Schedule next scan
        scanHandler.postDelayed(this::startScanning, SCAN_INTERVAL_SECONDS * 1000);
    }
    
    private final ScanCallback scanCallback = new ScanCallback() {
        @Override
        public void onScanResult(int callbackType, ScanResult result) {
            BluetoothDevice device = result.getDevice();
            String address = device.getAddress();
            String deviceName = device.getName();
            
            if (isPilloDevice(device, result)) {
                if (!discoveredDevices.containsKey(address) && connectedDevices.size() < MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
                    Log.d(TAG, "Pillo device discovered: " + deviceName + " (" + address + ")");
                    discoveredDevices.put(address, device);
                    
                    Log.d(TAG, "Attempting to connect to Pillo device: " + deviceName);
                    connectToDevice(device);
                    
                    // Stop scanning if we've reached max connections (like iOS)
                    if (connectedDevices.size() >= MAX_SIMULTANEOUS_PERIPHERAL_CONNECTION) {
                        Log.d(TAG, "Max connections reached, stopping scan");
                        stopScanning();
                    }
                } else if (discoveredDevices.containsKey(address)) {
                    Log.d(TAG, "Device already discovered, ignoring: " + deviceName);
                } else {
                    Log.d(TAG, "Max connections reached, ignoring: " + deviceName);
                }
            }
        }
        
        @Override
        public void onScanFailed(int errorCode) {
            Log.e(TAG, "Scan failed with error: " + errorCode);
            if (onCentralDidFailToInitialize != null) {
                onCentralDidFailToInitialize.onCentralDidFailToInitialize("BLE scan failed with error: " + errorCode);
            }
        }
    };
    
    private boolean isPilloDevice(BluetoothDevice device, ScanResult result) {
        String deviceName = device.getName();
        
        // Simple name-based detection like iOS: only connect if name contains "Pillo"
        if (deviceName != null && deviceName.toLowerCase().contains("pillo")) {
            Log.d(TAG, "Pillo device found by name: " + deviceName);
            return true;
        }
        
        return false;
    }
    
    private void connectToDevice(BluetoothDevice device) {
        String address = device.getAddress();
        
        if (connectedDevices.containsKey(address)) {
            Log.d(TAG, "Already connected to: " + address);
            return; // Already connected
        }
        
        Log.d(TAG, "Connecting to device: " + device.getName() + " (" + address + ")");
        BluetoothGatt gatt = device.connectGatt(context, false, gattCallback);
        if (gatt != null) {
            connectedDevices.put(address, gatt);
        } else {
            Log.e(TAG, "Failed to create GATT connection for: " + address);
            if (onPeripheralDidFailToConnect != null) {
                onPeripheralDidFailToConnect.onPeripheralDidFailToConnect(address);
            }
        }
    }
    
    private final BluetoothGattCallback gattCallback = new BluetoothGattCallback() {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            String address = gatt.getDevice().getAddress();
            String deviceName = gatt.getDevice().getName();
            
            if (status == BluetoothGatt.GATT_SUCCESS) {
                if (newState == BluetoothProfile.STATE_CONNECTED) {
                    Log.d(TAG, "Connected to device: " + address + " (name: " + deviceName + ")");
                    connectedDevices.put(address, gatt);
                    gatt.discoverServices();
                    
                    if (onPeripheralDidConnect != null) {
                        onPeripheralDidConnect.onPeripheralDidConnect(address);
                    }
                } else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
                    Log.d(TAG, "Disconnected from device: " + address + " (name: " + deviceName + ")");
                    connectedDevices.remove(address);
                    discoveredDevices.remove(address);
                    try { gatt.close(); } catch (Exception ignored) {}
                    
                    if (onPeripheralDidDisconnect != null) {
                        onPeripheralDidDisconnect.onPeripheralDidDisconnect(address);
                    }
                }
            } else {
                Log.e(TAG, "GATT connection failed for " + address + " with status: " + status);
                connectedDevices.remove(address);
                discoveredDevices.remove(address);
                try { gatt.close(); } catch (Exception ignored) {}
                
                if (onPeripheralDidFailToConnect != null) {
                    onPeripheralDidFailToConnect.onPeripheralDidFailToConnect(address);
                }
            }
        }
        
        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.d(TAG, "Services discovered for device: " + gatt.getDevice().getAddress());
                // Log all discovered services
                for (BluetoothGattService service : gatt.getServices()) {
                    Log.d(TAG, "Discovered service: " + service.getUuid().toString().toUpperCase());
                    // Check if this is the pressure service
                    if (service.getUuid().toString().toUpperCase().contains("579BA43D-A351-463D-92C7-911EC1B54E35")) {
                        Log.d(TAG, "*** PRESSURE SERVICE FOUND! ***");
                        // Log characteristics in this service
                        for (BluetoothGattCharacteristic characteristic : service.getCharacteristics()) {
                            Log.d(TAG, "  Characteristic: " + characteristic.getUuid().toString().toUpperCase());
                        }
                    }
                }
                enableNotifications(gatt);
            } else {
                Log.e(TAG, "Service discovery failed with status: " + status);
            }
        }
        
        @Override
        public void onCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.d(TAG, "Characteristic read success: " + characteristic.getUuid().toString().toUpperCase());
                handleCharacteristicRead(gatt, characteristic);
            } else {
                Log.w(TAG, "Characteristic read failed: " + characteristic.getUuid().toString().toUpperCase() + " status: " + status);
            }
            // Mark operation complete and process next
            isBleOperationInProgress = false;
            processNextBleOperation();
        }
        
        @Override
        public void onCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.d(TAG, "Characteristic write success: " + characteristic.getUuid().toString().toUpperCase());
            } else {
                Log.w(TAG, "Characteristic write failed: " + characteristic.getUuid().toString().toUpperCase() + " status: " + status);
            }
            // Mark operation complete and process next
            isBleOperationInProgress = false;
            processNextBleOperation();
        }
        
        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            Log.d(TAG, "Characteristic changed (notification): " + characteristic.getUuid().toString().toUpperCase());
            handleCharacteristicChanged(gatt, characteristic);
        }
        
        @Override
        public void onDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.d(TAG, "Descriptor write success for: " + descriptor.getCharacteristic().getUuid().toString().toUpperCase());
            } else {
                Log.e(TAG, "Descriptor write failed for: " + descriptor.getCharacteristic().getUuid().toString().toUpperCase() + " status: " + status);
            }
            // Mark operation complete and process next
            isBleOperationInProgress = false;
            processNextBleOperation();
        }
        
        // onCharacteristicsDiscovered does not exist in Android's BluetoothGattCallback; removed
    };
    
    private void enableNotifications(BluetoothGatt gatt) {
        // CRITICAL: Handshake must happen FIRST - device won't send pressure data until handshake is complete
        queueBleOperation(() -> handleHandshakeService(gatt));
        
        // Queue each device info read separately
        queueBleOperation(() -> readFirmwareVersion(gatt));
        queueBleOperation(() -> readHardwareVersion(gatt));
        queueBleOperation(() -> readModelNumber(gatt));
        
        // Start processing the queue
        processNextBleOperation();
    }
    
    private void queueBleOperation(Runnable operation) {
        bleOperationQueue.add(operation);
        Log.d(TAG, "Queued BLE operation, queue size: " + bleOperationQueue.size());
    }
    
    private void processNextBleOperation() {
        if (isBleOperationInProgress) {
            Log.d(TAG, "BLE operation already in progress, waiting...");
            return;
        }
        
        Runnable operation = bleOperationQueue.poll();
        if (operation != null) {
            isBleOperationInProgress = true;
            Log.d(TAG, "Processing next BLE operation, remaining: " + bleOperationQueue.size());
            operation.run();
            // Note: isBleOperationInProgress will be set to false in onDescriptorWrite callback
        } else {
            Log.d(TAG, "BLE operation queue empty");
        }
    }
    
    private void enableNotificationsForService(BluetoothGatt gatt, String serviceUuid, String characteristicUuid) {
        BluetoothGattService service = gatt.getService(UUID.fromString(serviceUuid));
        if (service != null) {
            Log.d(TAG, "Found service: " + serviceUuid);
            BluetoothGattCharacteristic characteristic = service.getCharacteristic(UUID.fromString(characteristicUuid));
            if (characteristic != null) {
                Log.d(TAG, "Found characteristic: " + characteristicUuid + " for service: " + serviceUuid);
                
                // Enable local notifications
                gatt.setCharacteristicNotification(characteristic, true);
                
                // CRITICAL: Write to CCCD to enable notifications on the server side (like iOS setNotifyValue does)
                BluetoothGattDescriptor descriptor = characteristic.getDescriptor(
                    UUID.fromString("00002902-0000-1000-8000-00805f9b34fb") // Standard CCCD UUID
                );
                if (descriptor != null) {
                    descriptor.setValue(BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE);
                    gatt.writeDescriptor(descriptor);
                    Log.d(TAG, "Wrote CCCD for notifications: " + characteristicUuid);
                } else {
                    Log.w(TAG, "CCCD not found for characteristic: " + characteristicUuid);
                }
                
                Log.d(TAG, "Enabled notifications for: " + characteristicUuid);
            } else {
                Log.w(TAG, "Characteristic not found: " + characteristicUuid + " for service: " + serviceUuid);
            }
        } else {
            Log.w(TAG, "Service not found: " + serviceUuid);
        }
    }
    
    private void readFirmwareVersion(BluetoothGatt gatt) {
        BluetoothGattService service = gatt.getService(UUID.fromString(DEVICEINFORMATION_SERVICE_UUID));
        if (service != null) {
            BluetoothGattCharacteristic firmwareChar = service.getCharacteristic(UUID.fromString(DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID));
            if (firmwareChar != null) {
                gatt.readCharacteristic(firmwareChar);
            }
        }
    }
    
    private void readHardwareVersion(BluetoothGatt gatt) {
        BluetoothGattService service = gatt.getService(UUID.fromString(DEVICEINFORMATION_SERVICE_UUID));
        if (service != null) {
            BluetoothGattCharacteristic hardwareChar = service.getCharacteristic(UUID.fromString(DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID));
            if (hardwareChar != null) {
                gatt.readCharacteristic(hardwareChar);
            }
        }
    }
    
    private void readModelNumber(BluetoothGatt gatt) {
        BluetoothGattService service = gatt.getService(UUID.fromString(DEVICEINFORMATION_SERVICE_UUID));
        if (service != null) {
            BluetoothGattCharacteristic modelChar = service.getCharacteristic(UUID.fromString(DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID));
            if (modelChar != null) {
                gatt.readCharacteristic(modelChar);
            }
        }
    }
    
    private void handleHandshakeService(BluetoothGatt gatt) {
        BluetoothGattService service = gatt.getService(UUID.fromString(HANDSHAKE_SERVICE_UUID));
        if (service != null) {
            Log.d(TAG, "Handshake service found, looking for characteristic...");
            BluetoothGattCharacteristic characteristic = service.getCharacteristic(UUID.fromString(HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID));
            if (characteristic != null) {
                Log.d(TAG, "Handshake characteristic found, reading value...");
                // Read the handshake value first
                gatt.readCharacteristic(characteristic);
            } else {
                Log.w(TAG, "Handshake characteristic not found!");
            }
        } else {
            Log.w(TAG, "Handshake service not found!");
        }
    }
    
    private void handleCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
        String address = gatt.getDevice().getAddress();
        String characteristicUuid = characteristic.getUuid().toString().toUpperCase();
        
        byte[] value = characteristic.getValue();
        if (value == null || value.length == 0) {
            return;
        }
        
        switch (characteristicUuid) {
            case BATTERY_LEVEL_CHARACTERISTIC_UUID:
                if (onPeripheralBatteryLevelDidChange != null) {
                    // Parse as uint32_t (4 bytes) like iOS
                    int batteryLevel = 0;
                    for (int i = 0; i < Math.min(4, value.length); i++) {
                        batteryLevel |= ((value[i] & 0xFF) << (i * 8));
                    }
                    onPeripheralBatteryLevelDidChange.onPeripheralBatteryLevelDidChange(address, batteryLevel);
                }
                break;
            case PRESSURE_VALUE_CHARACTERISTIC_UUID:
                Log.d(TAG, "Received pressure data for " + address + ", bytes: " + java.util.Arrays.toString(value));
                if (onPeripheralPressureDidChange != null) {
                    // Parse as uint32_t (4 bytes) like iOS
                    int pressure = 0;
                    for (int i = 0; i < Math.min(4, value.length); i++) {
                        pressure |= ((value[i] & 0xFF) << (i * 8));
                    }
                    Log.d(TAG, "Parsed pressure value: " + pressure);
                    Log.d(TAG, "Calling Unity pressure callback for device: " + address + " with value: " + pressure);
                    onPeripheralPressureDidChange.onPeripheralPressureDidChange(address, pressure);
                    Log.d(TAG, "Unity pressure callback completed");
                } else {
                    Log.w(TAG, "Pressure callback is NULL - Unity delegate not set!");
                }
                break;
            case CHARGE_STATE_CHARACTERISTIC_UUID:
                if (onPeripheralChargingStateDidChange != null) {
                    // Parse as uint32_t (4 bytes) like iOS
                    int chargingState = 0;
                    for (int i = 0; i < Math.min(4, value.length); i++) {
                        chargingState |= ((value[i] & 0xFF) << (i * 8));
                    }
                    onPeripheralChargingStateDidChange.onPeripheralChargingStateDidChange(address, chargingState);
                }
                break;
            case DEVICEINFORMATION_FIRMWAREVERSION_CHARACTERISTIC_UUID:
                if (onPeripheralFirmwareVersionDidChange != null) {
                    String firmwareVersion = new String(value).trim();
                    onPeripheralFirmwareVersionDidChange.onPeripheralFirmwareVersionDidChange(address, firmwareVersion);
                }
                break;
            case DEVICEINFORMATION_HARDWAREVERSION_CHARACTERISTIC_UUID:
                if (onPeripheralHardwareVersionDidChange != null) {
                    String hardwareVersion = new String(value).trim();
                    onPeripheralHardwareVersionDidChange.onPeripheralHardwareVersionDidChange(address, hardwareVersion);
                }
                break;
            case DEVICEINFORMATION_MODELNUMBER_CHARACTERISTIC_UUID:
                if (onPeripheralModelNumberDidChange != null) {
                    String modelNumber = new String(value).trim();
                    onPeripheralModelNumberDidChange.onPeripheralModelNumberDidChange(address, modelNumber);
                }
                break;
            case HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID:
                // Process handshake - this is critical for communication
                processHandshake(gatt, value);
                break;
        }
    }
    
    private void processHandshake(BluetoothGatt gatt, byte[] value) {
        if (value.length < 8) {
            Log.e(TAG, "Invalid handshake data length: " + value.length);
            return;
        }
        
        // Convert bytes to long (same as iOS uint64_t)
        long handshake = 0;
        for (int i = 0; i < 8; i++) {
            handshake |= ((long) (value[i] & 0xFF)) << (i * 8);
        }
        
        // Perform the same calculation as iOS
        double value1 = (double) handshake * 0.8129863214;
        double value2 = value1 / ((handshake % 10) + 1);
        double value3 = value2 + (handshake * 0.1870136786);
        long result = Math.round(value3);
        
        // Convert result back to bytes
        byte[] response = new byte[8];
        for (int i = 0; i < 8; i++) {
            response[i] = (byte) ((result >> (i * 8)) & 0xFF);
        }
        
        // Write the response back to the handshake characteristic
        writeValueToCharacteristic(gatt, HANDSHAKE_SERVICE_UUID, HANDSHAKE_HANDSHAKE_CHARACTERISTIC_UUID, response);
        
        // Enable ALL notifications after handshake completion (like iOS does)
        Log.d(TAG, "Handshake completed, enabling all notifications for device: " + gatt.getDevice().getAddress());
        queueBleOperation(() -> {
            Log.d(TAG, "Enabling battery notifications after handshake completion");
            enableNotificationsForService(gatt, BATTERY_SERVICE_UUID, BATTERY_LEVEL_CHARACTERISTIC_UUID);
        });
        queueBleOperation(() -> {
            Log.d(TAG, "Enabling pressure notifications after handshake completion");
            enableNotificationsForService(gatt, PRESSURE_SERVICE_UUID, PRESSURE_VALUE_CHARACTERISTIC_UUID);
        });
        queueBleOperation(() -> {
            Log.d(TAG, "Enabling charge notifications after handshake completion");
            enableNotificationsForService(gatt, CHARGE_SERVICE_UUID, CHARGE_STATE_CHARACTERISTIC_UUID);
        });
    }
    
    private void handleCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
        // Handle characteristic changes (notifications)
        handleCharacteristicRead(gatt, characteristic);
    }
    
    public void cancelPeripheralConnection(String identifier) {
        BluetoothGatt gatt = connectedDevices.get(identifier);
        if (gatt != null) {
            gatt.disconnect();
        }
    }
    
    public void powerOffPeripheral(String identifier) {
        BluetoothGatt gatt = connectedDevices.get(identifier);
        if (gatt != null) {
            // Send power off command via command characteristic
            BluetoothGattService service = gatt.getService(UUID.fromString(COMMAND_SERVICE_UUID));
            if (service != null) {
                BluetoothGattCharacteristic characteristic = service.getCharacteristic(UUID.fromString(COMMAND_COMMAND_CHARACTERISTIC_UUID));
                if (characteristic != null) {
                    // Add your power off command here
                    byte[] command = {0x01}; // Example command
                    characteristic.setValue(command);
                    gatt.writeCharacteristic(characteristic);
                }
            }
        }
    }
    
    public void forcePeripheralLedOff(String identifier, boolean enabled) {
        BluetoothGatt gatt = connectedDevices.get(identifier);
        if (gatt != null) {
            BluetoothGattService service = gatt.getService(UUID.fromString(COMMAND_SERVICE_UUID));
            if (service != null) {
                BluetoothGattCharacteristic characteristic = service.getCharacteristic(UUID.fromString(COMMAND_LED_CHARACTERISTIC_UUID));
                if (characteristic != null) {
                    byte[] command = {enabled ? (byte)0x01 : (byte)0x00};
                    characteristic.setValue(command);
                    gatt.writeCharacteristic(characteristic);
                }
            }
        }
    }
    
    public void calibratePeripheral(String identifier) {
        BluetoothGatt gatt = connectedDevices.get(identifier);
        if (gatt != null) {
            BluetoothGattService service = gatt.getService(UUID.fromString(CALIBRATION_SERVICE_UUID));
            if (service != null) {
                BluetoothGattCharacteristic characteristic = service.getCharacteristic(UUID.fromString(CALIBRATION_STARTCALIBRATION_CHARACTERISTIC_UUID));
                if (characteristic != null) {
                    byte[] command = {0x01}; // Start calibration command
                    characteristic.setValue(command);
                    gatt.writeCharacteristic(characteristic);
                }
            }
        }
    }
    
    private void writeValueToCharacteristic(BluetoothGatt gatt, String serviceUuid, String characteristicUuid, byte[] value) {
        BluetoothGattService service = gatt.getService(UUID.fromString(serviceUuid));
        if (service != null) {
            BluetoothGattCharacteristic characteristic = service.getCharacteristic(UUID.fromString(characteristicUuid));
            if (characteristic != null) {
                characteristic.setValue(value);
                gatt.writeCharacteristic(characteristic);
                Log.d(TAG, "Wrote value to characteristic: " + characteristicUuid);
            } else {
                Log.e(TAG, "Characteristic not found: " + characteristicUuid);
            }
        } else {
            Log.e(TAG, "Service not found: " + serviceUuid);
        }
    }
    
    // Static methods for Unity JNI calls
    public static void setDelegates(
            OnCentralDidInitialize onCentralDidInitialize,
            OnCentralDidFailToInitialize onCentralDidFailToInitialize,
            OnCentralDidStartScanning onCentralDidStartScanning,
            OnCentralDidStopScanning onCentralDidStopScanning,
            OnPeripheralDidConnect onPeripheralDidConnect,
            OnPeripheralDidDisconnect onPeripheralDidDisconnect,
            OnPeripheralDidFailToConnect onPeripheralDidFailToConnect,
            OnPeripheralBatteryLevelDidChange onPeripheralBatteryLevelDidChange,
            OnPeripheralPressureDidChange onPeripheralPressureDidChange,
            OnPeripheralChargingStateDidChange onPeripheralChargingStateDidChange,
            OnPeripheralFirmwareVersionDidChange onPeripheralFirmwareVersionDidChange,
            OnPeripheralHardwareVersionDidChange onPeripheralHardwareVersionDidChange,
            OnPeripheralModelNumberDidChange onPeripheralModelNumberDidChange) {
        
        PilloDeviceManager.onCentralDidInitialize = onCentralDidInitialize;
        PilloDeviceManager.onCentralDidFailToInitialize = onCentralDidFailToInitialize;
        PilloDeviceManager.onCentralDidStartScanning = onCentralDidStartScanning;
        PilloDeviceManager.onCentralDidStopScanning = onCentralDidStopScanning;
        PilloDeviceManager.onPeripheralDidConnect = onPeripheralDidConnect;
        PilloDeviceManager.onPeripheralDidDisconnect = onPeripheralDidDisconnect;
        PilloDeviceManager.onPeripheralDidFailToConnect = onPeripheralDidFailToConnect;
        PilloDeviceManager.onPeripheralBatteryLevelDidChange = onPeripheralBatteryLevelDidChange;
        PilloDeviceManager.onPeripheralPressureDidChange = onPeripheralPressureDidChange;
        PilloDeviceManager.onPeripheralChargingStateDidChange = onPeripheralChargingStateDidChange;
        PilloDeviceManager.onPeripheralFirmwareVersionDidChange = onPeripheralFirmwareVersionDidChange;
        PilloDeviceManager.onPeripheralHardwareVersionDidChange = onPeripheralHardwareVersionDidChange;
        PilloDeviceManager.onPeripheralModelNumberDidChange = onPeripheralModelNumberDidChange;
    }
    
    public static void startService(Context context) {
        getInstance(context).startService();
    }
    
    public static void stopService(Context context) {
        getInstance(context).stopService();
    }
    
    public static void cancelPeripheralConnection(Context context, String identifier) {
        getInstance(context).cancelPeripheralConnection(identifier);
    }
    
    public static void powerOffPeripheral(Context context, String identifier) {
        getInstance(context).powerOffPeripheral(identifier);
    }
    
    public static void forcePeripheralLedOff(Context context, String identifier, boolean enabled) {
        getInstance(context).forcePeripheralLedOff(identifier, enabled);
    }
    
    public static void calibratePeripheral(Context context, String identifier) {
        getInstance(context).calibratePeripheral(identifier);
    }
    
    // Debug method to help troubleshoot scanning issues
    public static void logDiscoveredDevices(Context context) {
        PilloDeviceManager instance = getInstance(context);
        Log.d(TAG, "=== Discovered Devices ===");
        for (Map.Entry<String, BluetoothDevice> entry : instance.discoveredDevices.entrySet()) {
            BluetoothDevice device = entry.getValue();
            Log.d(TAG, "Device: " + device.getName() + " (" + device.getAddress() + ")");
        }
        Log.d(TAG, "Total discovered: " + instance.discoveredDevices.size());
        Log.d(TAG, "Total connected: " + instance.connectedDevices.size());
    }
}
