using Avalonia;
using Avalonia.Native;
using CnE2PLC.Helpers;
using System;

namespace CnE2PLC.Avalonia;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var listener = new UiTraceListener();
        System.Diagnostics.Trace.Listeners.Add(listener);

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();

        // Suppress Avalonia's built-in "About Avalonia" / Hide / Quit items so
        // we can own the full app menu ourselves.
        if (OperatingSystem.IsMacOS())
            builder = builder.With(new MacOSPlatformOptions
            {
                DisableDefaultApplicationMenuItems = true,
            });

        return builder;
    }
}
