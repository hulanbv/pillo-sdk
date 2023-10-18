using System;

// Unity Engine Pillo SDK Framework
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Payloads {
  /// <summary>
  /// Payload for the OnCentralDidFailToInitialize event.
  /// </summary>
  [Serializable]
  public class CentralDidFailToInitializePayload {
    /// <summary>
    /// The error message.
    /// </summary>
    public string message;
  }
}