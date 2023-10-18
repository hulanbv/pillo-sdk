using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Payloads {
  /// <summary>
  /// Payload for the OnPeripheralHardwareDidChange event.
  /// </summary>
  [Serializable]
  public class PeripheralHardwareVersionDidChangePayload {
    /// <summary>
    /// The identifier of the Peripheral.
    /// </summary>
    public string identifier;

    /// <summary>
    /// The model number of the Peripheral.
    /// </summary>
    public string hardwareVersion;
  }
}