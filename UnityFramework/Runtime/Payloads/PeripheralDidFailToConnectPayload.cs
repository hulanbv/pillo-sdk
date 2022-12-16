using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Payloads {
  /// <summary>
  /// Payload for the OnPeripheralDidFailToConnect event.
  /// </summary>
  [Serializable]
  internal class PeripheralDidFailToConnectPayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;
  }
}