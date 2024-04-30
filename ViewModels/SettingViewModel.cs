using System;
using Avalonia.Markup.Xaml.Styling;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels;

public partial class SettingViewModel : ViewModelBase
{
    [ObservableProperty]
    private string languages;
    
    
    public void ChangeLanguage(string language)
    {
        var file = $"avares://Clash Vista//Assets/Languages/{language}.axaml";
        var data = new ResourceInclude(new Uri(file, UriKind.Absolute))
        {
            Source = new Uri(file, UriKind.Absolute)
        };
        Avalonia.Application.Current!.Resources.MergedDictionaries[0] = data;
    }
}