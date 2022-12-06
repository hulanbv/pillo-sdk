using UnityEngine;

// Unity Engine Pillo SDK Framework Tests
// Author: Jeffrey Lanters at Hulan
namespace Hulan.PilloSDK.Framework.Tests {
  /// <summary>
  /// The Pillo Framework Test MonoBehaviour will be debug log all the incoming
  /// Pillo Framework events.
  /// </summary>
  internal class PilloFrameworkTest : MonoBehaviour {
    /// <summary>
    /// Binds the Pillo Framework events to the Pillo Framework Test MonoBe-
    /// haviour.
    /// </summary>
    private void Start () {
      PilloFramework.onCentralDidInitialize += OnCentralDidInitialize;
      PilloFramework.onCentralDidFailToInitialize += OnCentralDidFailToInitialize;
      PilloFramework.onPeripheralDidConnect += OnPeripheralDidConnect;
      PilloFramework.onPeripheralDidDisconnect += OnPeripheralDidDisconnect;
      PilloFramework.onPeripheralDidFailToConnect += OnPeripheralDidFailToConnect;
      PilloFramework.onPeripheralBatteryLevelDidChange += OnPeripheralBatteryLevelDidChange;
      PilloFramework.onPeripheralPressureDidChange += OnPeripheralPressureDidChange;
      PilloFramework.onPeripheralChargeStateDidChange += OnPeripheralChargeStateDidChange;
    }

    /// <summary>
    /// Delegate will be invoked when the Central has been initialized.
    /// </summary>
    private void OnCentralDidInitialize () {
      Debug.Log ("Pillo Framework Central Did Initialize");
    }

    /// <summary>
    /// Delegate will be invoked when the Central has failed to initialize.
    /// </summary>
    /// <param name="message">The error message.</param>
    private void OnCentralDidFailToInitialize (string message) {
      Debug.Log ($"Pillo Framework Central Did Fail To Initialize with message {message}");
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did connect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    private void OnPeripheralDidConnect (string identifier) {
      Debug.Log ($"Pillo Framework Peripheral with identifier {identifier} Did Connect");
    }

    /// <summary>
    /// Delegate should be invoked when a Peripheral did disconnect.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    private void OnPeripheralDidDisconnect (string identifier) {
      Debug.Log ($"Pillo Framework Peripheral with identifier {identifier} Did Disconnect");
    }

    /// <summary>
    /// Delegate will be invoked when a Peripheral did fail to connect.
    /// <param name="identifier">The identifier of the Peripheral.</param>
    private void OnPeripheralDidFailToConnect (string identifier) {
      Debug.Log ($"Pillo Framework Peripheral with identifier {identifier} Did Fail To Connect");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's battery level did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel">The battery level of the Peripheral.</param>
    private void OnPeripheralBatteryLevelDidChange (string identifier, int batteryLevel) {
      Debug.Log ($"Pillo Framework Peripheral with identifier {identifier} Battery Level Did Change to {batteryLevel}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's pressure did change.
    /// </summary>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="batteryLevel"> The pressure of the Peripheral.</param>
    private void OnPeripheralPressureDidChange (string identifier, int pressure) {
      Debug.Log ($"Pillo Framework Peripheral with identifier {identifier} Pressure Did Change to {pressure}");
    }

    /// <summary>
    /// Delegate will be invoked when the Peripheral's charge state did change.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="identifier">The identifier of the Peripheral.</param>
    /// <param name="chargeState">The charge state of the Peripheral.</param>
    private void OnPeripheralChargeStateDidChange (string identifier, PeripheralChargeState chargeState) {
      Debug.Log ($"Pillo Framework Peripheral with identifier {identifier} Charge State Did Change to {chargeState}");
    }
  }
}
