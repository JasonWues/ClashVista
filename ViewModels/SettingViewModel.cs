using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Clash_Vista.Services;
using Clash_Vista.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace Clash_Vista.ViewModels;

public partial class SettingViewModel : ViewModelBase
{
    
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

        _autoLaunch = _configService.Vista.AutoLaunch;
        _silentStart = _configService.Vista.SilentStart;
        _systemProxy = _configService.Vista.SystemProxy;
    }

    partial void OnLanguageChanged(KeyValuePair<string, string> value)
    {
        _configService.Vista.Language = value.Key;
        BaseUtilities.ChangeLanguage(value.Key);
    }


    partial void OnAutoLaunchChanged(bool value)
    {
        try
        {
            _configService.Vista.AutoLaunch = value;
            if (OperatingSystem.IsWindows())
            {
                var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (!value)
                {
                    key?.DeleteValue("ClashVista",false);
                    key?.Close();
                    return;
                }

                if (key.GetValue("ClashVista") != null)
                {
                    key?.DeleteValue("ClashVista",false);
                }
                key?.SetValue("ClashVista", $"{Process.GetCurrentProcess().MainModule?.FileName}");
                key?.Close();
            }

            if (OperatingSystem.IsLinux())
            {
                var currentFilePath = Process.GetCurrentProcess().MainModule?.FileName;
                var service = $"""
                              [Unit]
                              Description=ClashVista
                              After=network.target

                              [Service]
                              User=myuser
                              Group=mygroup
                              WorkingDirectory=/home/myuser/myapp
                              ExecStart={currentFilePath}
                              Restart=on-failure

                              [Install]
                              WantedBy=multi-user.target
                              """;
                
                //TODO write service
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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

        _configService.SaveVista();

    }
}