using System.Collections.Generic;
using System.ComponentModel;
using Clash_Vista.Services;
using Clash_Vista.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Clash_Vista.ViewModels;

public partial class SettingViewModel : ViewModelBase
{

    readonly private ConfigService _configService;

    [ObservableProperty]
    private KeyValuePair<string, string> language;

    [ObservableProperty]
    private Dictionary<string, string> supportedLanguages = BaseUtilities.SupportedLanguages;
    
    public SettingViewModel(ConfigService configService)
    {
        _configService = configService;
        
        language = new KeyValuePair<string, string>(_configService.Vista.Language,
            supportedLanguages[_configService.Vista.Language]);
    }

    partial void OnLanguageChanged(KeyValuePair<string, string> value)
    {
        _configService.Vista.Language = value.Key;
        BaseUtilities.ChangeLanguage(value.Key);
    }

    [RelayCommand]
    public void RandomPort()
    {
        
    }
    

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        _configService.SaveVista();
    }
}