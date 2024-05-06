using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Clash_Vista.Utilities;

public static class YamlUtilities
{
    readonly private static YamlContext YamlContext = new YamlContext();

    public static async Task<T> ReadYamlAsync<T>(string path)
    {
        if (!Path.Exists(path))
        {
            throw new FileNotFoundException();
        }

        var deserializer = new StaticDeserializerBuilder(YamlContext).Build();

        return deserializer.Deserialize<T>(await File.ReadAllTextAsync(path));
    }
    
    public static void SaveYaml<T>(string path, T data) where T : class
    {
        var serializer = new StaticSerializerBuilder(YamlContext)
            .Build();

        var yaml = serializer.Serialize(data, typeof(T));

        File.WriteAllText(path, yaml);
    }

    public static async Task SaveYamlAsync<T>(string path, T data) where T : class
    {
        var serializer = new StaticSerializerBuilder(YamlContext)
            .Build();

        var yaml = serializer.Serialize(data, typeof(T));

        await File.WriteAllTextAsync(path, yaml);
    }
}