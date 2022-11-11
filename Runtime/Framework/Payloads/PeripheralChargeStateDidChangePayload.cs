using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Payloads {
  /// <summary>
  /// Payload for the OnPeripheralChargeStateDidChange event.
  /// </summary>
  [Serializable]
  internal class PeripheralChargeStateDidChangePayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;

    /// <summary>
    /// The charging state of the Peripheral.
    /// </summary>
    public int chargeState;
  }
}