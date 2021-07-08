
using System;

public class WbxDevices
{
    public Item[] items { get; set; }
}

public class Item
{
    public string id { get; set; }
    public string displayName { get; set; }
    public string personId { get; set; }
    public string orgId { get; set; }
    public object[] capabilities { get; set; }
    public object[] permissions { get; set; }
    public string product { get; set; }
    public string[] tags { get; set; }
    public string ip { get; set; }
    public string mac { get; set; }
    public string serial { get; set; }
    public string activeInterface { get; set; }
    public string software { get; set; }
    public string upgradeChannel { get; set; }
    public string primarySipUrl { get; set; }
    public string[] sipUrls { get; set; }
    public string[] errorCodes { get; set; }
    public string connectionStatus { get; set; }
    public DateTime created { get; set; }
    public string placeId { get; set; }
}
