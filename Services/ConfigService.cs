using System.Threading.Tasks;
using Clash_Vista.Models;
using Clash_Vista.Utilities;

namespace Clash_Vista.Services;

public class ConfigService
{
    public ClashConfig ClashConfig { get; set; }
    
    public Vista Vista { get; set; }

    public Profiles Profiles { get; set; }

    public async Task<ConfigService> CreateAsync()
    {
        var service = new ConfigService();
        await service.GetOrInit();
        return service;
    }

    private async Task GetOrInit()
    {
        ClashConfig = await YamlUtilities.ReadYaml<ClashConfig>(Dir.GetClashConfigPath());
        Vista = await YamlUtilities.ReadYaml<Vista>(Dir.GetVistaConfigPath());
        Profiles = await YamlUtilities.ReadYaml<Profiles>(Dir.GetProgramProfilesConfigPath());
    }
}