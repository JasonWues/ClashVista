using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Clash_Vista.Models;
using Clash_Vista.Utilities;

namespace Clash_Vista.Services;

public class ResolveService
{
    private ConfigService _configService;

    private InitService _initService;
    public ResolveService(ConfigService configService,InitService initService)
    {
        _initService = initService;
        _configService = configService;
    }
    public async Task ResolveConfig()
    {
        _initService.InitScheme();

        var ConfigService = await _configService.CreateAsync();

        var vista = ConfigService.Vista;

        var port = vista.VistaMixedPort;
        
        if (vista.EnableRandomPort)
        {
            port = FindUnusedPort();
            vista.VistaMixedPort = port;
            await YamlUtilities.SaveYaml(Dir.GetVistaConfigPath(), vista);
        }

        var clashConfig = ConfigService.ClashConfig;
        if (port != clashConfig.MixedPort)
        {
            clashConfig.MixedPort = port;
            await YamlUtilities.SaveYaml(Dir.GetClashConfigPath(), clashConfig);
        }
    }

    private int FindUnusedPort()
    {
        try
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback,0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}