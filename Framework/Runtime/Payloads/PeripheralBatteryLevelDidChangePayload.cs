using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// Payload for the OnPeripheralBatteryLevelDidChange event.
  /// </summary>
  [Serializable]
  internal class PeripheralBatteryLevelDidChangePayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;

    /// <summary>
    /// The battery level of the Peripheral.
    /// </summary>
    public int batteryLevel;
  }
}