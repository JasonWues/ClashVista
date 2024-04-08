using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Clash_Vista.Models;

namespace Clash_Vista.Utilities;

public class Resolve
{
    public async Task ResolveConfig()
    {
        Init.InitScheme();

        var vista = await YamlUtilities.ReadYaml<Vista>(Dir.GetVistaConfigPath());

        var port = vista.VistaMixedPort;
        
        if (vista.EnableRandomPort)
        {
            port = FindUnusedPort();
            vista.VistaMixedPort = port;
            await YamlUtilities.SaveYaml(Dir.GetVistaConfigPath(), vista);
        }
        
        var clashConfig = await YamlUtilities.ReadYaml<ClashConfig>(Dir.GetClashConfigPath());
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