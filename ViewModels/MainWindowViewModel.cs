using System;
using System.Diagnostics;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Clash_Vista.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private SubscriptionViewModel _subscriptionViewModel;
        
        
        public MainWindowViewModel(SubscriptionViewModel subscriptionViewModel)
        {
            SubscriptionViewModel = subscriptionViewModel;
        }
        

    }
}
