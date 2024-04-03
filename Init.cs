using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Clash_Vista.Models;
using Clash_Vista.Utilities;

namespace Clash_Vista;

public static class Init
{
    public static async Task InitConfig()
    {
        InitLog();

        var profilesPath = DirUtilities.GetProgramProfilesPath();

        if (!Path.Exists(profilesPath))
        {
            Directory.CreateDirectory(profilesPath);
        }

        var vistaConfigPath = DirUtilities.GetVistaConfigPath();
        if (!Path.Exists(vistaConfigPath))
        {
            await YamlUtilities.SaveYaml(vistaConfigPath, new Vista());
        }

        var profilesConfigPath = DirUtilities.GetProgramProfilesConfigPath();
        if (!Path.Exists(profilesConfigPath))
        {
            await YamlUtilities.SaveYaml(profilesConfigPath, new Profiles());
        }
    }

    private static void InitLog()
    {
        var logDir = DirUtilities.GetProgramLogPath();
        if (!Path.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }
    }

    public static void InitScheme()
    {
        if (OperatingSystem.IsLinux())
        {
            var status = Process.Start("xdg-mine", "default clash-verge.desktop x-scheme-handler/clash").Start();
        }
    }
}