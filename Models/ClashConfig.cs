using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Clash_Vista.Models;

[YamlSerializable]
public class ClashConfig
{
    public int MixedPort { get; set; } = 6780;
    public int SocksPort { get; set; } = 6788;
    public int Port { get; set; } = 6789;
    public LogLevel LogLevel { get; set; } = LogLevel.Info;
    public bool AllowLan { get; set; } = false;
    public Mode Mode { get; set; } = Mode.Rule;
    public string ExternalController { get; set; } = "127.0.0.1:9097";
    public string Secret { get; set; } = string.Empty;
    public Tun Tun { get; set; } = new Tun();
}

[YamlSerializable]
public class Tun
{
    public string Stack { get; set; }
    public string Device { get; set; } = "Meta";
    public bool AutoRoute { get; set; } = true;
    public bool StrictRoute { get; set; } = false;
    public bool AutoDetectInterface { get; set; } = true;
    public List<string> DnsHijack { get; set; } = ["any:53"];
    public int Mtu { get; set; } = 9000;
}

public enum Mode
{
    Rule,
    Global,
    Direct
}