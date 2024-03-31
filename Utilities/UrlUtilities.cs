using System;
using System.Diagnostics;

namespace Clash_Vista.Utilities;

public static class UrlUtilities
{
    public static void OpenURL(string url)
    {
        if (OperatingSystem.IsWindows())
        {
            Process.Start(new ProcessStartInfo(url.Replace("&", "^&")) { UseShellExecute = true });
        }else if (OperatingSystem.IsLinux())
        {
            Process.Start("xdg-open", url);
        }else if (OperatingSystem.IsMacOS())
        {
            Process.Start("open", url);
        }
    }
}