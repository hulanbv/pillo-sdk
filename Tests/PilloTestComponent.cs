using UnityEngine;
using UnityEngine.UI;
using Hulan.Pillo.SDK;
using UnityEngine.InputSystem;

namespace Hulan.Pillo.Test {
  public class PilloTestComponent : MonoBehaviour {

    public PilloTestInputActions pilloTestInputActions;

    private void Awake () {
      pilloTestInputActions = new PilloTestInputActions ();
      pilloTestInputActions.PilloActionMap.Test.performed += OnPilloPress;
    }

    private void OnPilloPress (InputAction.CallbackContext context) {
      Debug.Log ("WOOOOOOOOOOW");
    }

    private void OnEnable () {
      pilloTestInputActions.Enable ();
    }

    private void OnDisable () {
      pilloTestInputActions.Disable ();
    }
  }
}