using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels;

public partial class SettingViewModel : ViewModelBase
{
    [ObservableProperty]
    private Dictionary<string, string> supportedLanguages = new Dictionary<string, string>
    {
        {"中文","zh-CN"},
        {"English","en-US"}
    };
    
    [ObservableProperty]
    private KeyValuePair<string,string> language;
    
    partial void OnLanguageChanged(KeyValuePair<string,string> value)
    {
        ChangeLanguage(value.Value);
    }

    public void ChangeLanguage(string lang)
    {
        var translations = Application.Current?.Resources.MergedDictionaries.OfType<ResourceInclude>().FirstOrDefault(x => x.Source?.OriginalString?.Contains("/Lang/") ?? false);

        if (translations != null)
            Application.Current?.Resources.MergedDictionaries.Remove(translations);

        var assemblyName  = Assembly.GetExecutingAssembly().GetName().Name;
        Application.Current?.Resources.MergedDictionaries.Add(
            new ResourceInclude(new Uri($"avares://{assemblyName}/Assets/Lang/{lang}.axaml"))
            {
                Source = new Uri($"avares://{assemblyName}/Assets/Lang/{lang}.axaml")
            });
    }
}
