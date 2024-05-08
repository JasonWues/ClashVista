using System.Collections.Generic;
using System.ComponentModel;
using Clash_Vista.Services;
using Clash_Vista.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Clash_Vista.ViewModels;

public partial class SettingViewModel : ViewModelBase
{

    private bool _isWriteConfig;
    
    readonly private ConfigService _configService;

    [ObservableProperty]
    private bool _autoLaunch;

    [ObservableProperty]
    private bool _silentStart;

    [ObservableProperty]
    private bool _systemProxy;
    
    [ObservableProperty]
    private KeyValuePair<string, string> _language;

    [ObservableProperty]
    private Dictionary<string, string> _supportedLanguages = BaseUtilities.SupportedLanguages;
    
    public SettingViewModel(ConfigService configService)
    {
        _configService = configService;
        
        _language = new KeyValuePair<string, string>(_configService.Vista.Language,
            _supportedLanguages[_configService.Vista.Language]);

        AutoLaunch = _configService.Vista.AutoLaunch;
        SilentStart = _configService.Vista.SilentStart;
        SystemProxy = _configService.Vista.SystemProxy;

        _isWriteConfig = true;
    }

    partial void OnLanguageChanged(KeyValuePair<string, string> value)
    {
        _configService.Vista.Language = value.Key;
        BaseUtilities.ChangeLanguage(value.Key);
    }


    partial void OnAutoLaunchChanged(bool value)
    {
        _configService.Vista.AutoLaunch = value;
    }

    partial void OnSilentStartChanged(bool value)
    {
        _configService.Vista.SilentStart = value;
    }

    partial void OnSystemProxyChanged(bool value)
    {
        _configService.Vista.SystemProxy = value;
    }

    [RelayCommand]
    public void RandomPort()
    {
        
    }
    

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (_isWriteConfig)
        {
            _configService.SaveVista();
        }
    }
}