package com.hulan.devicemanager;

import android.content.Context;
import com.unity3d.player.UnityPlayer;

public class PilloDeviceManagerBridge {
    private static final String TAG = "PilloDeviceManagerBridge";
    
    // These methods are no longer needed - callbacks are handled directly via AndroidJavaProxy
    
    // JNI methods called from Unity
    public static void setDelegates() {
        Context context = UnityPlayer.currentActivity;
    }
    
    public static void startService() {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.startService(context);
    }
    
    public static void stopService() {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.stopService(context);
    }
    
    public static void cancelPeripheralConnection(String identifier) {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.cancelPeripheralConnection(context, identifier);
    }
    
    public static void powerOffPeripheral(String identifier) {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.powerOffPeripheral(context, identifier);
    }
    
    public static void forcePeripheralLedOff(String identifier, boolean enabled) {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.forcePeripheralLedOff(context, identifier, enabled);
    }
    
    public static void calibratePeripheral(String identifier) {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.calibratePeripheral(context, identifier);
    }
    
    // Debug method to help troubleshoot scanning issues
    public static void logDiscoveredDevices() {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.logDiscoveredDevices(context);
    }
    
    // Method to handle permission results from Unity
    public static void onPermissionResult(int requestCode, String[] permissions, int[] grantResults) {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.getInstance(context).onPermissionResult(requestCode, permissions, grantResults);
    }
    
    // Method to handle Bluetooth enable result from Unity
    public static void onBluetoothEnableResult(boolean enabled) {
        Context context = UnityPlayer.currentActivity;
        PilloDeviceManager.getInstance(context).onBluetoothEnableResult(enabled);
    }
}
