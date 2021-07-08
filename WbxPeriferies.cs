
public class WbxPeriferies
{
    public string deviceId { get; set; }
    public Result result { get; set; }
}

public class Result
{
    public Peripherals Peripherals { get; set; }
}

public class Peripherals
{
    public Connecteddevice[] ConnectedDevice { get; set; }
}

public class Connecteddevice
{
    public int id { get; set; }
    public string UpgradeStatus { get; set; }
    public string Status { get; set; }
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string NetworkAddress { get; set; }
    public string SoftwareInfo { get; set; }
    public string Type { get; set; }
    public string HardwareInfo { get; set; }
    public string ID { get; set; }
}
