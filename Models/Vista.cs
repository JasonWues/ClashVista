using YamlDotNet.Serialization;

namespace Clash_Vista.Models;

[YamlSerializable]
public class Vista
{
    public string Language { get; set; } = "en-US";
    
    public LogLevel LogLevel { get; set; } = LogLevel.Info;

    public ThemeMode ThemeMode { get; set; } = ThemeMode.System;

    public bool EnableAutoLaunch { get; set; } = false;

    public bool EnableSilentStart { get; set; } = false;

    public bool EnableSystemProxy { get; set; } = false;

    public bool EnableRandomPort { get; set; } = false;

    public int VistaMixedPort { get; set; }

    public int VistaSocksPort { get; set; }

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