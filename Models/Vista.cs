using YamlDotNet.Serialization;

namespace Clash_Vista.Models;

[YamlSerializable]
public class Vista
{

    public LogLevel LogLevel { get; set; }

    public ThemeMode ThemeMode { get; set; }

    public bool EnableAutoLaunch { get; set; }

    public bool EnableSilentStart { get; set; }

    public bool EnableSystemProxy { get; set; }

    public short VistaMixedPort { get; set; }

    public short VistaSocksPort { get; set; }

}

public enum ThemeMode
{
    Light,

    Dark,

    System
}

public enum LogLevel
{
    Silent,

    Error,

    Warn,

    Info,

    Debug,

    Trace
}