using YamlDotNet.Serialization;

namespace Clash_Vista.Models;

[YamlSerializable]
public class ProfileItem
{
    public string Uid { get; set; }

    public ProfileItemType Type { get; set; }

    public string Name { get; set; }

    public string File { get; set; }

    public string? Desc { get; set; }

    public string Url { get; set; }

    public ProfileItemSelected Selected { get; set; }

    public string? HomeUrl { get; set; }

    public long Updated { get; set; }

    public ProfileItemExtra Extra { get; set; }

    public ProfileItemOption Option { get; set; }
}

[YamlSerializable]
public class ProfileItemSelected
{
    public string Name { get; set; }
    public string Now { get; set; }
}

[YamlSerializable]
public class ProfileItemOption
{
    public string UserAgent { get; set; }

    public uint? UpdateInterval { get; set; }
}

[YamlSerializable]
public class ProfileItemExtra
{
    public long Upload { get; set; }

    public long Download { get; set; }

    public long Total { get; set; }

    public long Expire { get; set; }
}

public enum ProfileItemType
{
    Remote,

    Local
}