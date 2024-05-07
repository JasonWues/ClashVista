using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;

namespace Clash_Vista.Utilities;

public static class BaseUtilities
{
    public static Dictionary<string, string> SupportedLanguages = new Dictionary<string, string>
    {
        { "zh-CN", "中文" },
        { "en-US", "English" }
    };
    
    public static void OpenURL(string url)
    {
        if (OperatingSystem.IsWindows())
        {
            Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
        }
        else if (OperatingSystem.IsLinux())
        {
            Process.Start("xdg-open", url);
        }
        else if (OperatingSystem.IsMacOS())
        {
            Process.Start("open", url);
        }
    }

    public static void ChangeLanguage(string lang)
    {
        var translations = Application.Current?.Resources.MergedDictionaries.OfType<ResourceInclude>()
            .FirstOrDefault(x => x.Source?.OriginalString?.Contains("/Lang/") ?? false);

        if (!SupportedLanguages.ContainsKey(lang))
        {
            lang = "en-US";
        }
        
        if (translations != null && translations.Source.OriginalString.Contains(lang))
        {
            return;
        }

        if (translations != null)
        {
            Application.Current?.Resources.MergedDictionaries.Remove(translations);
        }

        Application.Current?.Resources.MergedDictionaries.Add(
            new ResourceInclude(new Uri($"avares://Clash Vista/Assets/Lang/{lang}.axaml"))
            {
                Source = new Uri($"avares://Clash Vista/Assets/Lang/{lang}.axaml")
            });
    }
}