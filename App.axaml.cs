using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Clash_Vista.Services;
using Clash_Vista.Utilities;
using Clash_Vista.ViewModels;
using Clash_Vista.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SukiUI.Controls;

namespace Clash_Vista;

public class App : Application
{
    private IServiceProvider _provider;

    private Window _mainWindow;

    public App()
    {
        CreateLog();
        Dispatcher.UIThread.UnhandledException += (sender, args) =>
        {
            SukiHost.ShowDialog(args.Exception.ToString());
            Log.Logger.Warning("Exception:{exception}",args.Exception.ToString());
            args.Handled = true;
            
        };
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        _provider = ConfigureServices();
        RegisterLocator();
    }

    public void CreateLog()
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .WriteTo.Console()
#endif
            .WriteTo.File(Path.Join(Dir.GetProgramLogPath(), "log.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Task.Run(async () =>
        {
            var initService = _provider?.GetRequiredService<InitService>();
            await initService!.InitConfig();

            var resolveService = _provider?.GetRequiredService<ResolveService>();
            await resolveService!.ResolveConfig();
        }).Wait();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            
            BindingPlugins.DataValidators.RemoveAt(0);
            var viewLocator = _provider?.GetRequiredService<IDataTemplate>();
            var mainVm = _provider?.GetRequiredService<MainWindowViewModel>();
            _mainWindow = viewLocator?.Build(mainVm) as Window;
            if (mainVm.SettingViewModel.SilentStart)
            {
                desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                return;
            }
            desktop.MainWindow = _mainWindow;

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
        services.AddSingleton<SettingViewModel>();
        services.AddSingleton<RuleViewModel>();
        services.AddSingleton<InitService>();
        services.AddSingleton<ResolveService>();
        services.AddSingleton<ConfigService>();
        services.AddHttpClient();
        return services.BuildServiceProvider();
    }

    private static void RegisterLocator()
    {
        ViewLocator.Register<MainWindowViewModel, MainWindow>();
        ViewLocator.Register<SubscriptionViewModel, SubscriptionView>();
        ViewLocator.Register<SettingViewModel, SettingView>();
        ViewLocator.Register<RuleViewModel, RuleView>();
    }
    private void TrayIcon_OnClicked(object? sender, EventArgs e)
    {
        if (!_mainWindow.IsVisible)
        {
            _mainWindow.Show();
        }
    }
    private void NativeMenuItem_OnClick(object? sender, EventArgs e)
    {
        (_mainWindow as MainWindow)?.CurrentProcess.Dispose();
        Environment.Exit(0);
    }
}