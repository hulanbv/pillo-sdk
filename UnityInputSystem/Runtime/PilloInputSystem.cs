using Hulan.PilloSDK.InputSystem.Core;
using Hulan.PilloSDK.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem {
  /// <summary>
  /// The Pillo Input class exposes a set of methods and delegates to interact 
  /// with the Pillo Peripherals, Services and Characteristics.
  /// </summary>
  [System.Obsolete("The Pillo Input System is obsolete and will no longer be updated, use the Pillo Framework to write a custom implementation instead.", false)]
  public static class PilloInputSystem {
    /// <summary>
    /// Delegate invoked when a Pillo Input Device has been connected.
    /// </summary>
    public static PilloInputSystemDelegate.OnPilloInputDeviceDidConnect onPilloInputDeviceDidConnect;

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has been disconnected.
    /// </summary>
    public static PilloInputSystemDelegate.OnPilloInputDeviceDidDisconnect onPilloInputDeviceDidDisconnect;

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has failed to connect.
    /// </summary>
    public static PilloInputSystemDelegate.OnPilloInputDeviceDidFailToConnect onPilloInputDeviceDidFailToConnect;

    /// <summary>
    /// Delegate invoked when the Pillo Input Device's state did changed.
    /// </summary>
    public static PilloInputSystemDelegate.OnPilloInputDeviceStateDidChange onPilloInputDeviceStateDidChange;

    /// <summary>
    /// A list of connected Pillo Input Devices.
    /// </summary>
    public static readonly List<PilloInputDevice> pilloInputDevices = new List<PilloInputDevice>();

    /// <summary>
    /// The number of connected Pillo Input Devices.
    /// </summary>
    public static int pilloInputDeviceCount {
      get {
        // The number of Pillo Input Devices is determined by the number of
        // Pillo Input Device objects in the pilloInputDevices list.
        return PilloInputSystem.pilloInputDevices.Count;
      }
    }

    /// <summary>
    /// Gets the Pillo Input Device with the specified identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the Pillo Input Device.</param>
    /// <returns>The Pillo Input Device with the specified identifier.</returns>
    private static PilloInputDevice FindPilloInputDevice(string identifier) {
      foreach (var pilloInputDevice in PilloInputSystem.pilloInputDevices) {
        if (pilloInputDevice.identifier == identifier) {
          return pilloInputDevice;
        }
      }
      return null;
    }

    /// <summary>
    /// Assigns a player index to all unassigned Pillo Input Devices.
    /// </summary>
    private static void AssignPilloInputDevicePlayerIndexes() {
      var unassignedPilloInputDevice = PilloInputSystem.pilloInputDevices.FindAll(pilloInputDevice => {
        return pilloInputDevice.playerIndex == -1;
      });
      foreach (var pilloInputDevice in unassignedPilloInputDevice) {
        pilloInputDevice.playerIndex = PilloInputSystem.GetNextPilloInputDevicePlayerIndexes();
      }
    }

    /// <summary>
    /// Gets the next available Pillo Input Device Player Index.
    /// </summary>
    /// <returns>The next available Pillo Input Device Player Index</returns>
    private static int GetNextPilloInputDevicePlayerIndexes() {
      var playerIndex = 0;
      while (PilloInputSystem.pilloInputDevices.Any(pilloInputDevice => {
        return pilloInputDevice.playerIndex == playerIndex;
      })) {
        playerIndex++;
      }
      return playerIndex;
    }

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// binds the Pillo Framework's delegates to the Pillo Input System's
    /// methods.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad() {
      PilloFramework.onPeripheralDidConnect += PilloInputSystem.OnPeripheralDidConnect;
      PilloFramework.onPeripheralDidDisconnect += PilloInputSystem.OnPeripheralDidDisconnect;
      PilloFramework.onPeripheralDidFailToConnect += PilloInputSystem.OnPeripheralDidFailToConnect;
      PilloFramework.onPeripheralBatteryLevelDidChange += PilloInputSystem.OnPeripheralBatteryLevelDidChange;
      PilloFramework.onPeripheralPressureDidChange += PilloInputSystem.OnPeripheralPressureDidChange;
      PilloFramework.onPeripheralChargeStateDidChange += PilloInputSystem.OnPeripheralChargeStateDidChange;
      PilloFramework.onPeripheralFirmwareVersionDidChange += PilloInputSystem.OnPeripheralFirmwareVersionDidChange;
      PilloFramework.onPeripheralHardwareVersionDidChange += PilloInputSystem.OnPeripheralHardwareVersionDidChange;
      PilloFramework.onPeripheralModelNumberDidChange += PilloInputSystem.OnPeripheralModelNumberDidChange;
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral has been 
    /// connected.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    private static void OnPeripheralDidConnect(string identifier) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice == null) {
        pilloInputDevice = new PilloInputDevice(identifier);
        PilloInputSystem.pilloInputDevices.Add(pilloInputDevice);
      }
      PilloInputSystem.AssignPilloInputDevicePlayerIndexes();
      if (PilloInputSystem.onPilloInputDeviceDidConnect != null) {
        PilloInputSystem.onPilloInputDeviceDidConnect(pilloInputDevice);
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral has been
    /// disconnected.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    private static void OnPeripheralDidDisconnect(string identifier) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        PilloInputSystem.pilloInputDevices.Remove(pilloInputDevice);
        PilloInputSystem.AssignPilloInputDevicePlayerIndexes();
        if (PilloInputSystem.onPilloInputDeviceDidDisconnect != null) {
          PilloInputSystem.onPilloInputDeviceDidDisconnect(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral has failed to
    /// connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    private static void OnPeripheralDidFailToConnect(string identifier) {
      PilloInputSystem.onPilloInputDeviceDidFailToConnect?.Invoke();
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral's Batter Level
    /// has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The new battery level of the Peripheral.</param>
    private static void OnPeripheralBatteryLevelDidChange(string identifier, int batteryLevel) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.batteryLevel = batteryLevel;
        if (PilloInputSystem.onPilloInputDeviceStateDidChange != null) {
          PilloInputSystem.onPilloInputDeviceStateDidChange(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral's Pressure
    /// has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="pressure">The new pressure of the Peripheral.</param>
    private static void OnPeripheralPressureDidChange(string identifier, int pressure) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.pressure = pressure;
        if (PilloInputSystem.onPilloInputDeviceStateDidChange != null) {
          PilloInputSystem.onPilloInputDeviceStateDidChange(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral's Charge State
    /// has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargeState">The new charge state of the Peripheral.</param>
    private static void OnPeripheralChargeStateDidChange(string identifier, PeripheralChargeState chargeState) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.chargeState = (PilloInputDeviceChargeState)chargeState;
        if (PilloInputSystem.onPilloInputDeviceStateDidChange != null) {
          PilloInputSystem.onPilloInputDeviceStateDidChange(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral's Firmware
    /// Version has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="firmwareVersion">The new firmware version of the Peripheral.</param>
    private static void OnPeripheralFirmwareVersionDidChange(string identifier, string firmwareVersion) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.firmwareVersion = firmwareVersion;
        if (PilloInputSystem.onPilloInputDeviceStateDidChange != null) {
          PilloInputSystem.onPilloInputDeviceStateDidChange(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral's Hardware
    /// Version has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="hardwareVersion">The new hardware version of the Peripheral.</param>
    private static void OnPeripheralHardwareVersionDidChange(string identifier, string hardwareVersion) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.hardwareVersion = hardwareVersion;
        if (PilloInputSystem.onPilloInputDeviceStateDidChange != null) {
          PilloInputSystem.onPilloInputDeviceStateDidChange(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Method invoked by the Pillo Framework when a Peripheral's Model Number
    /// has changed.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="modelNumber">The new model number of the Peripheral.</param>
    private static void OnPeripheralModelNumberDidChange(string identifier, string modelNumber) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice(identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.modelNumber = modelNumber;
        if (PilloInputSystem.onPilloInputDeviceStateDidChange != null) {
          PilloInputSystem.onPilloInputDeviceStateDidChange(pilloInputDevice);
        }
      }
    }

    /// <summary>
    /// Resets all of the Pillo Input Device's player indexes to their default
    /// values.
    /// </summary>
    public static void ResetPilloInputDevicePlayerIndexes() {
      for (var i = 0; i < PilloInputSystem.pilloInputDevices.Count; i++) {
        PilloInputSystem.pilloInputDevices[i].playerIndex = i;
      }
    }
  }
}