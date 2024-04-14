using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Clash_Vista.Models;

[YamlSerializable]
public class Profiles
{
    public string? Current { get; set; } = null;

    public List<string>? Chain { get; set; } = null;

    public List<ProfileItem> Items { get; set; } = [];
}