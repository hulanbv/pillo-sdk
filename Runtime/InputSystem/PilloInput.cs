using System.Runtime.InteropServices;
using Hulan.PilloSDK.Core;
using UnityEngine;
using System.Collections.Generic;

// Unity Engine Pillo SDK Input System
// Author: Jeffrey Lanters at Hulan

namespace Hulan.PilloSDK.InputSystem {

  /// <summary>
  /// The Pillo Input class exposes a set of methods and delegates to interact 
  /// with the Pillo Peripherals, Services and Characteristics.
  /// </summary>
  public static class PilloInput {

    /// <summary>
    /// Delegate invoked when the Central has been initialized.
    /// </summary>
    public static PilloInputDelegate.OnCentralDidInitialize onCentralDidInitialize;

    /// <summary>
    /// Delegate invoked when the Central has failed to initialize.
    /// </summary>
    public static PilloInputDelegate.OnCentralDidFailToInitialize onCentralDidFailToInitialize;

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has been connected.
    /// </summary>
    public static PilloInputDelegate.OnPilloInputDeviceDidConnect onPilloInputDeviceDidConnect;

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has been disconnected.
    /// </summary>
    public static PilloInputDelegate.OnPilloInputDeviceDidDisconnect onPilloInputDeviceDidDisconnect;

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has failed to connect.
    /// </summary>
    public static PilloInputDelegate.OnPilloInputDeviceDidFailToConnect onPilloInputDeviceDidFailToConnect;

    /// <summary>
    /// Delegate invoked when the Pillo Input Device's state did changed.
    /// </summary>
    public static PilloInputDelegate.OnPilloInputDeviceStateDidChange onPilloInputDeviceStateDidChange;

    /// <summary>
    /// A list of connected Pillo Input Devices.
    /// </summary>
    public static readonly List<PilloInputDevice> pilloInputDevices = new List<PilloInputDevice> ();

    /// <summary>
    /// The number of connected Pillo Input Devices.
    /// </summary>
    public static int pilloInputDeviceCount {
      get {
        return PilloInput.pilloInputDevices.Count;
      }
    }

    private static PilloInputDevice GetPilloInputDevice (string identifier) {
      foreach (var pilloInputDevice in PilloInput.pilloInputDevices) {
        if (pilloInputDevice.identifier == identifier) {
          return pilloInputDevice;
        }
      }
      return null;
    }

    private static void ReassignPilloInputDevicePlayerIndexes () {
      for (var i = 0; i < PilloInput.pilloInputDevices.Count; i++) {
        PilloInput.pilloInputDevices[i].playerIndex = i;
      }
    }

    internal static void OnCentralDidInitialize () {
      PilloInput.onCentralDidInitialize?.Invoke ();
    }

    internal static void OnCentralDidFailToInitialize (string reason) {
      PilloInput.onCentralDidFailToInitialize?.Invoke (reason);
    }

    internal static void OnPeripheralDidConnect (string identifier) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice == null) {
        pilloInputDevice = new PilloInputDevice (identifier);
        PilloInput.pilloInputDevices.Add (pilloInputDevice);
      }
      PilloInput.ReassignPilloInputDevicePlayerIndexes ();
      PilloInput.onPilloInputDeviceDidConnect?.Invoke (pilloInputDevice);
    }

    internal static void OnPeripheralDidDisconnect (string identifier) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        PilloInput.pilloInputDevices.Remove (pilloInputDevice);
        PilloInput.ReassignPilloInputDevicePlayerIndexes ();
        PilloInput.onPilloInputDeviceDidDisconnect?.Invoke (pilloInputDevice);
      }
    }

    internal static void OnPeripheralDidFailToConnect () {
      PilloInput.onPilloInputDeviceDidFailToConnect?.Invoke ();
    }

    internal static void OnPeripheralBatteryLevelDidChange (string identifier, int batteryLevel) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.batteryLevel = batteryLevel;
        PilloInput.onPilloInputDeviceStateDidChange?.Invoke (pilloInputDevice);
      }
    }

    internal static void OnPeripheralPressureDidChange (string identifier, int pressure) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.pressure = pressure;
        PilloInput.onPilloInputDeviceStateDidChange?.Invoke (pilloInputDevice);
      }
    }
  }
}