using YamlDotNet.Serialization;

namespace Clash_Vista.Models;

[YamlSerializable]
public class Vista
{
    public string Language { get; set; }

    public LogLevel LogLevel { get; set; } = LogLevel.Info;

    public ThemeMode ThemeMode { get; set; } = ThemeMode.System;

    public bool AutoLaunch { get; set; } = false;

    public bool SilentStart { get; set; } = false;

    public bool SystemProxy { get; set; } = false;

    public bool RandomPort { get; set; } = false;

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