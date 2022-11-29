using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework {
  /// <summary>
  /// Payload for the OnPeripheralDidDisconnect event.
  /// </summary>
  [Serializable]
  internal class PeripheralDidDisconnectPayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;
  }
}