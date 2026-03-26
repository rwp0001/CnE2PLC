using Avalonia.Controls;
using Avalonia.Interactivity;
using CnE2PLC.Avalonia.Services;
using CnE2PLC.Avalonia.ViewModels;
using System;

namespace CnE2PLC.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Closing += OnWindowClosing;

        // On macOS the app uses the system-level NativeMenu (built in App.axaml.cs).
        // Hide the in-window menu bar so it doesn't appear twice.
        if (OperatingSystem.IsMacOS())
            InWindowMenu.IsVisible = false;
    }

    private void OnWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        var settings = SettingsService.Load();
        settings.WindowMaximized = WindowState == WindowState.Maximized;
        if (WindowState == WindowState.Normal)
        {
            settings.WindowX = Position.X;
            settings.WindowY = Position.Y;
            settings.WindowWidth = Width;
            settings.WindowHeight = Height;
        }
        if (DataContext is MainWindowViewModel vm)
        {
            settings.Debug = vm.DebugVisible;
            settings.DebugLevel = vm.DebugLevel;
        }
        SettingsService.Save(settings);
    }

    private void ClearLog_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
            vm.LogText = string.Empty;
    }
}
