using System;
using System.Diagnostics;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels
{
    public partial class MainWindowViewModel(
        SubscriptionViewModel subscriptionViewModel,
        SettingViewModel settingViewModel,
        RuleViewModel ruleViewModel)
        : ViewModelBase
    {
        [ObservableProperty]
        private SubscriptionViewModel _subscriptionViewModel = subscriptionViewModel;

        [ObservableProperty]
        private SettingViewModel _settingViewModel = settingViewModel;

        [ObservableProperty]
        private RuleViewModel _ruleViewModel = ruleViewModel;
        
    }
}
