namespace Hulan.PilloSDK.DeviceManager.Core {
  /// <summary>
  /// Containing the delegate definitions for the Pillo Device Manager.
  /// </summary>
  public class Delegates {
    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    public delegate void OnCentralDidInitialize();

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    public delegate void OnCentralDidFailToInitialize(string message);

    /// <summary>
    /// Delegate will be invoked when the Central has started scanning.
    /// </summary>
    public delegate void OnCentralDidStartScanning();

    /// <summary>
    /// Delegate will be invoked when the Central has stopped scanning.
    /// </summary>
    public delegate void OnCentralDidStopScanning();

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    public delegate void OnPeripheralDidConnect(string identifier);

    /// <summary>
    /// Delegate will be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    public delegate void OnPeripheralDidDisconnect(string identifier);

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    public delegate void OnPeripheralDidFailToConnect(string identifier);

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did 
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The battery level of the Peripheral.</param>
    public delegate void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel);

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel"> The pressure of the Peripheral.</param>
    public delegate void OnPeripheralPressureDidChange(string identifier, int pressure);

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did 
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargingState">The charge state of the Peripheral.</param>
    public delegate void OnPeripheralChargingStateDidChange(string identifier, ChargingState chargingState);

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge firmware version 
    /// did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="firmwareVersion">The firmware version of the Peripheral.</param>
    public delegate void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion);

    /// <summary>
    /// Delegate will be invoked when the Peripheral's hardware version did
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="hardwareVersion">The hardware version of the Peripheral.</param>
    public delegate void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion);

    /// <summary>
    /// Delegate will be invoked when the Peripheral's model number did
    /// change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="modelNumber">The model number of the Peripheral.</param>
    public delegate void OnPeripheralModelNumberDidChange(string identifier, string modelNumber);
  }
}