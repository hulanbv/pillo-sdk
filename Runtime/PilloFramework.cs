using System.Runtime.InteropServices;
using UnityEngine;

namespace Hulan.Pillo.SDK {
  public class PilloFramework {

    [DllImport ("__Internal")]
    private static extern void PilloFrameworkInitialize ();

    private static PilloFrameworkCallbackListener pilloFrameworkCallbackListener;

    public PilloFramework (IPilloFrameworkDelegate delegateObject) {
      if (pilloFrameworkCallbackListener != null) {
        throw new System.Exception ("Can't have another Pillo Framework Instance");
      }
      pilloFrameworkCallbackListener = new GameObject ().AddComponent<PilloFrameworkCallbackListener> ();
      pilloFrameworkCallbackListener.gameObject.name = "~PilloFrameworkCallbackListener";
      pilloFrameworkCallbackListener.gameObject.hideFlags = HideFlags.HideInHierarchy;
      pilloFrameworkCallbackListener.delegateObject = delegateObject;
      Object.DontDestroyOnLoad (pilloFrameworkCallbackListener.gameObject);
      PilloFrameworkInitialize ();
    }
  }
}