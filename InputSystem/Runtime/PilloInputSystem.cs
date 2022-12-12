using Hulan.PilloSDK.InputSystem.Core;
using Hulan.PilloSDK.Framework;
using System.Collections.Generic;
using UnityEngine;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.InputSystem {
  /// <summary>
  /// The Pillo Input class exposes a set of methods and delegates to interact 
  /// with the Pillo Peripherals, Services and Characteristics.
  /// </summary>
  public static class PilloInputSystem {
    /// <summary>
    /// Delegate invoked when a anything has changed.
    /// </summary>
    public static PilloInputSystemDelegate.OnChange onChange;

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
    public static readonly List<PilloInputDevice> pilloInputDevices = new List<PilloInputDevice> ();

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
    private static PilloInputDevice FindPilloInputDevice (string identifier) {
      foreach (var pilloInputDevice in PilloInputSystem.pilloInputDevices) {
        if (pilloInputDevice.identifier == identifier) {
          return pilloInputDevice;
        }
      }
      return null;
    }

    private static void ReassignPilloInputDevicePlayerIndexes () {
      // TODO -- Improve logic implementation.
      foreach (var pilloInputDevice in PilloInputSystem.pilloInputDevices) {
        if (pilloInputDevice.playerIndex == -1) {
          pilloInputDevice.playerIndex = PilloInputSystem.GetNextPilloInputDevicePlayerIndexes ();
        }
      }
    }

    private static int GetNextPilloInputDevicePlayerIndexes () {
      // TODO -- Improve logic implementation.
      var playerIndex = 0;
      while (true) {
        var playerIndexIsInUse = false;
        foreach (var pilloInputDevice in PilloInputSystem.pilloInputDevices) {
          if (pilloInputDevice.playerIndex == playerIndex) {
            playerIndexIsInUse = true;
          }
        }
        if (playerIndexIsInUse == false) {
          return playerIndex;
        }
        playerIndex++;
      }
    }

    /// <summary>
    /// Invoked when the Runtime Application initializes and is loaded. This
    /// binds the Pillo Framework's delegates to the Pillo Input System's
    /// methods.
    /// </summary>
    [RuntimeInitializeOnLoadMethod]
    private static void RuntimeInitializeOnLoad () {
      PilloFramework.onPeripheralDidConnect += PilloInputSystem.OnPeripheralDidConnect;
      PilloFramework.onPeripheralDidDisconnect += PilloInputSystem.OnPeripheralDidDisconnect;
      PilloFramework.onPeripheralDidFailToConnect += PilloInputSystem.OnPeripheralDidFailToConnect;
      PilloFramework.onPeripheralBatteryLevelDidChange += PilloInputSystem.OnPeripheralBatteryLevelDidChange;
      PilloFramework.onPeripheralPressureDidChange += PilloInputSystem.OnPeripheralPressureDidChange;
      PilloFramework.onPeripheralChargeStateDidChange += PilloInputSystem.OnPeripheralChargeStateDidChange;
    }

    private static void OnPeripheralDidConnect (string identifier) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice (identifier);
      if (pilloInputDevice == null) {
        pilloInputDevice = new PilloInputDevice (identifier);
        PilloInputSystem.pilloInputDevices.Add (pilloInputDevice);
      }
      PilloInputSystem.ReassignPilloInputDevicePlayerIndexes ();
      PilloInputSystem.onPilloInputDeviceDidConnect?.Invoke (pilloInputDevice);
      PilloInputSystem.onChange?.Invoke ();
    }

    private static void OnPeripheralDidDisconnect (string identifier) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        PilloInputSystem.pilloInputDevices.Remove (pilloInputDevice);
        PilloInputSystem.ReassignPilloInputDevicePlayerIndexes ();
        PilloInputSystem.onPilloInputDeviceDidDisconnect?.Invoke (pilloInputDevice);
        PilloInputSystem.onChange?.Invoke ();
      }
    }

    private static void OnPeripheralDidFailToConnect (string identifier) {
      PilloInputSystem.onPilloInputDeviceDidFailToConnect?.Invoke ();
      PilloInputSystem.onChange?.Invoke ();
    }

    private static void OnPeripheralBatteryLevelDidChange (string identifier, int batteryLevel) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.batteryLevel = batteryLevel;
        PilloInputSystem.onPilloInputDeviceStateDidChange?.Invoke (pilloInputDevice);
        PilloInputSystem.onChange?.Invoke ();
      }
    }

    private static void OnPeripheralPressureDidChange (string identifier, int pressure) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.pressure = pressure;
        PilloInputSystem.onPilloInputDeviceStateDidChange?.Invoke (pilloInputDevice);
        PilloInputSystem.onChange?.Invoke ();
      }
    }

    private static void OnPeripheralChargeStateDidChange (string identifier, PeripheralChargeState chargeState) {
      var pilloInputDevice = PilloInputSystem.FindPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.chargeState = (PilloInputDeviceChargeState)chargeState;
        PilloInputSystem.onPilloInputDeviceStateDidChange?.Invoke (pilloInputDevice);
        PilloInputSystem.onChange?.Invoke ();
      }
    }

    /// <summary>
    /// Resets all of the Pillo Input Device's player indexes to their default
    /// values.
    /// </summary>
    public static void ResetPilloInputDevicePlayerIndexes () {
      for (var i = 0; i < PilloInputSystem.pilloInputDevices.Count; i++) {
        PilloInputSystem.pilloInputDevices[i].playerIndex = i;
      }
    }
  }
}