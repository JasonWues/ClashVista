using System;
using System.Threading.Tasks;
using Avalonia;

namespace Clash_Vista
{
    internal sealed class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            await Init.InitConfig();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
        }
    }
}