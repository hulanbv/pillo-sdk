using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Payloads {
  /// <summary>
  /// Payload for the OnPeripheralDidConnect event.
  /// </summary>
  [Serializable]
  internal class PeripheralDidConnectPayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;
  }
}