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
using Clash_Vista.Services;
using Clash_Vista.Utilities;
using Clash_Vista.ViewModels;
using Clash_Vista.Views;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Clash_Vista;

public class App : Application
{
    public IServiceProvider Provider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Provider = ConfigureServices();
        RegisterLocator();
        CreateLog();
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
            var initService = Provider?.GetRequiredService<InitService>();
            await initService!.InitConfig();

            var resolveService = Provider?.GetRequiredService<ResolveService>();
            await resolveService!.ResolveConfig();
        }).Wait();


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
        services.AddSingleton<SettingViewModel>();
        services.AddSingleton<RuleViewModel>();
        services.AddSingleton<InitService>();
        services.AddSingleton<ResolveService>();
        services.AddSingleton<ConfigService>();
        services.AddHttpClient();
        services.AddMapster();
        return services.BuildServiceProvider();
    }

    private static void RegisterLocator()
    {
        ViewLocator.Register<MainWindowViewModel, MainWindow>();
        ViewLocator.Register<SubscriptionViewModel, SubscriptionView>();
        ViewLocator.Register<SettingViewModel, SettingView>();
        ViewLocator.Register<RuleViewModel, RuleView>();
    }
}