using System.Threading.Tasks;
using Clash_Vista.Models;
using Clash_Vista.Utilities;

namespace Clash_Vista.Services;

public class ConfigService
{
    public ClashConfig ClashConfig { get; set; }

    public Profiles Profiles{ get; set; }

    public Vista Vista { get; set; }
    
    
    public async Task<ConfigService> CreateAsync()
    {
        var service = this;
        await service.GetOrInitAsync();
        return service;
    }

    private async Task GetOrInitAsync()
    {
        ClashConfig = await YamlUtilities.ReadYamlAsync<ClashConfig>(Dir.GetClashConfigPath());
        Vista = await YamlUtilities.ReadYamlAsync<Vista>(Dir.GetVistaConfigPath());
        Profiles = await YamlUtilities.ReadYamlAsync<Profiles>(Dir.GetProgramProfilesConfigPath());
    }

    public void SaveClashConfig()
    {
        YamlUtilities.SaveYaml(Dir.GetClashConfigPath(), ClashConfig);
    }
    public void SaveVista()
    {
        YamlUtilities.SaveYaml(Dir.GetVistaConfigPath(), Vista);
    }
    public void SaveProfiles()
    {
        YamlUtilities.SaveYaml(Dir.GetProgramProfilesConfigPath(), Profiles);
    }
}