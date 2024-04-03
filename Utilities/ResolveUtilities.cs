using System.Threading.Tasks;
using Clash_Vista.Models;

namespace Clash_Vista.Utilities;

public class ResolveUtilities
{
    public async Task ResolveConfig()
    {
        Init.InitScheme();

        var vista = await YamlUtilities.ReadYaml<Vista>(DirUtilities.GetVistaConfigPath());
    }
}