using UnityEngine;

// Unity Engine Pillo SDK Framework Editor
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Editor {
  /// <summary>
  /// The Pillo Framework Test MonoBehaviour will be debug log all the incoming
  /// Pillo Framework events.
  /// </summary>
  internal class PilloFrameworkTest : MonoBehaviour {
    /// <summary>
    /// Binds the Pillo Framework events to the Pillo Framework Test
    /// MonoBehaviour.
    /// </summary>
    private void Start () {
      PilloFramework.onCentralDidInitialize += OnCentralDidInitialize;
      PilloFramework.onCentralDidFailToInitialize += OnCentralDidFailToInitialize;
    }

    /// <summary>
    /// Delegate invoked when the Central has been initialized.
    /// </summary>
    private void OnCentralDidInitialize () {
      Debug.Log ("Pillo Framework Central Did Initialize");
    }

    /// <summary>
    /// Delegate invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    private void OnCentralDidFailToInitialize (string message) {
      Debug.Log ("Pillo Framework Central Did Fail To Initialize");
    }
  }
}
