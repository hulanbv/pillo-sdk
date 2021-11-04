using Hulan.Pillo.SDK.Enums;

namespace Hulan.Pillo.SDK.Structures {
  public struct BeaconResponseData {
    public string UUID;
    public int Major;
    public int Minor;
    public int RSSI;
    public int AndroidSignalPower;
    public DeviceProximity deviceProximity;
  }
}