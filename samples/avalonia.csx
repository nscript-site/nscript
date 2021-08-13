#r "nuget: Avalonia, 0.10.0"
#r "nuget: Avalonia.Desktop, 0.10.0"

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

public class App : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Avalonia.Controls.Window window = new Window();
            window.Width = 200;
            window.Height = 200;
            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}

public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();

BuildAvaloniaApp().StartWithClassicDesktopLifetime(null);

