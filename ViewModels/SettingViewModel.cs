using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using Clash_Vista.Services;
using Clash_Vista.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels;

public partial class SettingViewModel : ViewModelBase
{
    
    readonly private ConfigService _configService;
    /// <inheritdoc/>
    public SettingViewModel(ConfigService configService)
    {
        _configService = configService;
        
        language = new KeyValuePair<string, string>(_configService.Vista.Language,supportedLanguages[_configService.Vista.Language]);
    }
    
    [ObservableProperty]
    private Dictionary<string, string> supportedLanguages = new Dictionary<string, string>
    {
        {"zh-CN","中文"},
        {"en-US","English"}
    };
    
    [ObservableProperty]
    private KeyValuePair<string,string> language;

    partial void OnLanguageChanged(KeyValuePair<string,string> value)
    {
        _configService.Vista.Language = value.Key;
        BaseUtilities.ChangeLanguage(value.Key);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        _configService.SaveVista();
    }

}
