﻿using System;
using System.IO;

namespace Clash_Vista.Utilities;

public static class Dir
{
    public static string GetProgramConfigPath()
    {
        return Path.Join(AppContext.BaseDirectory, ".config");
    }

    public static string GetProgramLogPath()
    {
        return Path.Join(GetProgramConfigPath(), "log");
    }

    public static string GetClashConfigPath()
    {
        return Path.Join(GetProgramConfigPath(), "config.yaml");
    }

    public static string GetProgramProfilesPath()
    {
        return Path.Join(GetProgramConfigPath(), "profiles");
    }

    public static string GetProgramProfilesConfigPath()
    {
        return Path.Join(GetProgramConfigPath(), "profiles.yaml");
    }

    public static string GetVistaConfigPath()
    {
        return Path.Join(GetProgramConfigPath(), "vista.yaml");
    }
}