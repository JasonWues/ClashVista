using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Clash_Vista.Utilities;
using Serilog;

namespace Clash_Vista
{
    internal sealed class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            await Init.InitConfig();
            Resolve resolve = new Resolve();
            await resolve.ResolveConfig();
            
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .WriteTo.Console()    
#endif
                .WriteTo.File(Path.Join(Dir.GetProgramLogPath(),"log.txt"),rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            Log.Information("sss");
            
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