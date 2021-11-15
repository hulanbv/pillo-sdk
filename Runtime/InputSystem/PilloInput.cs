using System.Runtime.InteropServices;
using Hulan.Pillo.SDK.Core;
using UnityEngine;
using System.Collections.Generic;

// Pillo Framework Unity SDK
// Author: Jeffrey Lanters at Hulan

namespace Hulan.Pillo.SDK.InputSystem {

  /// <summary>
  /// The Pillo Input class exposes a set of methods and delegates to interact 
  /// with the Pillo Peripherals, Services and Characteristics.
  /// </summary>
  public static class PilloInput {

    /// <summary>
    /// Delegate invoked when the Framework has been initialized.
    /// </summary>
    public static PilloInputDelegate.OnDidInitialize onDidInitialize;

    /// <summary>
    /// Delegate invoked when the Framework has failed to initialize.
    /// </summary>
    public static PilloInputDelegate.OnDidFailToInitialize onDidFailToInitialize;

    /// <summary>
    /// Delegate invoked when a Pillo has been connected.
    /// </summary>
    public static PilloInputDelegate.OnDeviceDidConnect onDeviceDidConnect;

    /// <summary>
    /// Delegate invoked when a Pillo has been disconnected.
    /// </summary>
    public static PilloInputDelegate.OnDeviceDidDisconnect onDeviceDidDisconnect;

    /// <summary>
    /// Delegate invoked when a Pillo Input Device has failed to connect.
    /// </summary>
    public static PilloInputDelegate.OnDeviceDidFailToConnect onDeviceDidFailToConnect;

    /// <summary>
    /// Delegate invoked when the Pillo Input Device's state did changed.
    /// </summary>
    public static PilloInputDelegate.OnDeviceStateDidChange onDeviceStateDidChange;

    /// <summary>
    /// A list of connected Pillo Input Devices.
    /// </summary>
    public static readonly List<PilloInputDevice> connections = new List<PilloInputDevice> ();

    /// <summary>
    /// The number of connected Pillo Input Devices.
    /// </summary>
    public static int connectionCount {
      get {
        return PilloInput.connections.Count;
      }
    }

    private static PilloInputDevice GetPilloInputDevice (string identifier) {
      foreach (var connection in PilloInput.connections) {
        if (connection.identifier == identifier) {
          return connection;
        }
      }
      return null;
    }

    private static void ReassignPilloInputDevicePlayerIndexes () {
      for (var i = 0; i < PilloInput.connections.Count; i++) {
        PilloInput.connections[i].playerIndex = i;
      }
    }

    internal static void OnDidInitialize () { }

    internal static void OnDidFailToInitialize (string reason) { }

    internal static void OnDeviceDidConnect (string identifier) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice == null) {
        pilloInputDevice = new PilloInputDevice (identifier);
        PilloInput.connections.Add (pilloInputDevice);
      }
      PilloInput.ReassignPilloInputDevicePlayerIndexes ();
      PilloInput.onDeviceDidConnect?.Invoke (pilloInputDevice);
    }

    internal static void OnDeviceDidDisconnect (string identifier) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        PilloInput.connections.Remove (pilloInputDevice);
        PilloInput.ReassignPilloInputDevicePlayerIndexes ();
        PilloInput.onDeviceDidDisconnect?.Invoke (pilloInputDevice);
      }
    }

    internal static void OnDeviceDidFailToConnect (string identifier) {
      PilloInput.onDeviceDidFailToConnect?.Invoke (identifier);
    }

    internal static void OnBatteryLevelDidChange (string identifier, int batteryLevel) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.batteryLevel = batteryLevel;
        PilloInput.onDeviceStateDidChange?.Invoke (pilloInputDevice);
      }
    }

    internal static void OnPressureDidChange (string identifier, int pressure) {
      var pilloInputDevice = PilloInput.GetPilloInputDevice (identifier);
      if (pilloInputDevice != null) {
        pilloInputDevice.pressure = pressure;
        PilloInput.onDeviceStateDidChange?.Invoke (pilloInputDevice);
      }
    }

    public static void DisconnectDevice (string pilloInputDeviceIdentifier) {
      // TODO Implement this after the new Pillo Hardware arrives.
    }
  }
}