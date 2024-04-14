using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Logging;
using Clash_Vista.Utilities;
using Serilog;

namespace Clash_Vista
{
    internal sealed class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace(LogEventLevel.Information);
        }
    }
}