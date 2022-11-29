using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
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
    /// The charge state of the Peripheral.
    /// </summary>
    public PeripheralChargeState chargeState;
  }
}