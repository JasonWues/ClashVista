using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels;

public partial class MainWindowViewModel(
    SubscriptionViewModel subscriptionViewModel,
    SettingViewModel settingViewModel,
    RuleViewModel ruleViewModel,
    ProxyViewModel proxyViewModel)
    : ViewModelBase
{

    [ObservableProperty]
    private RuleViewModel _ruleViewModel = ruleViewModel;

    [ObservableProperty]
    private SettingViewModel _settingViewModel = settingViewModel;

    [ObservableProperty]
    private SubscriptionViewModel _subscriptionViewModel = subscriptionViewModel;

    [ObservableProperty]
    private ProxyViewModel _proxyViewModel = proxyViewModel;
}