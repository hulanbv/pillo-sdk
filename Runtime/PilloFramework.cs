using System.Runtime.InteropServices;

namespace Hulan.Pillo.SDK {
  public static class PilloFramework {

    [DllImport ("__Internal")]
    private static extern void PilloInitialize ();

    public static void Initialize () {
      PilloInitialize ();
    }
  }
}