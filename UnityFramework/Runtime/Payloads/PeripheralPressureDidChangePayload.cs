using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Payloads {
  /// <summary>
  /// Payload for the OnPeripheralPressureDidChange event.
  /// </summary>
  [Serializable]
  internal class PeripheralPressureDidChangePayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;

    /// <summary>
    /// The pressure of the Peripheral.
    /// </summary>
    public int pressure;
  }
}