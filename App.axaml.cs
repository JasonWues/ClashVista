using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Clash_Vista.ViewModels;
using Clash_Vista.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Clash_Vista
{
    public partial class App : Application
    {
        public IServiceProvider Provider;
        
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            Provider = ConfigureServices();
            RegisterLocator();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                
                BindingPlugins.DataValidators.RemoveAt(0);
                var viewLocator = Provider?.GetRequiredService<IDataTemplate>();
                var mainVm = Provider?.GetRequiredService<MainWindowViewModel>();
                desktop.MainWindow = viewLocator?.Build(mainVm) as Window;
            }

            base.OnFrameworkInitializationCompleted();
        }
        
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            var viewlocator = Current?.DataTemplates.First(x => x is ViewLocator);
            if (viewlocator is not null)
            {
                services.AddSingleton(viewlocator);
            }

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<SubscriptionViewModel>();
            services.AddHttpClient();

            return services.BuildServiceProvider();
        }

        private static void RegisterLocator()
        {
            ViewLocator.Register<MainWindowViewModel, MainWindow>();
            ViewLocator.Register<SubscriptionViewModel, SubscriptionView>();
        }
    }
}