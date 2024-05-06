using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Clash_Vista.Models;
using Clash_Vista.Utilities;

namespace Clash_Vista.Services;

public class InitService
{
    public async Task InitConfig()
    {
        InitLogDir();

        var profilesPath = Dir.GetProgramProfilesPath();

        if (!Path.Exists(profilesPath))
        {
            Directory.CreateDirectory(profilesPath);
        }

        var clashConfigPath = Dir.GetClashConfigPath();
        if (!Path.Exists(clashConfigPath))
        {
            await YamlUtilities.SaveYamlAsync(clashConfigPath, new ClashConfig());
        }

        var vistaConfigPath = Dir.GetVistaConfigPath();
        if (!Path.Exists(vistaConfigPath))
        {
            await YamlUtilities.SaveYamlAsync(vistaConfigPath, new Vista());
        }

        var profilesConfigPath = Dir.GetProgramProfilesConfigPath();
        if (!Path.Exists(profilesConfigPath))
        {
            await YamlUtilities.SaveYamlAsync(profilesConfigPath, new Profiles());
        }
    }

    private void InitLogDir()
    {
        var logDir = Dir.GetProgramLogPath();
        if (!Path.Exists(logDir))
        {
            Directory.CreateDirectory(logDir);
        }
    }

    public void InitScheme()
    {
        if (OperatingSystem.IsLinux())
        {
            var status = Process.Start("xdg-mine", "default clash-verge.desktop x-scheme-handler/clash").Start();
        }
    }
}