using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Clash_Vista.Utilities;

public static class YamlUtilities
{
    readonly private static YamlContext _yamlContext = new YamlContext();

    public static async Task<T> ReadYaml<T>(string path)
    {
        if (!Path.Exists(path))
        {
            throw new FileNotFoundException();
        }

        var deserializer = new StaticDeserializerBuilder(_yamlContext).Build();

        return deserializer.Deserialize<T>(await File.ReadAllTextAsync(path));
    }

    public static async Task SaveYaml<T>(string path, T data)
    {
        var serializer = new StaticSerializerBuilder(_yamlContext)
            .Build();

        var yaml = serializer.Serialize(data, typeof(T));

        await File.WriteAllTextAsync(path, yaml);
    }
}