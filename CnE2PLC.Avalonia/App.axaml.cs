using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;
using System.Linq;
using Avalonia.Markup.Xaml;
using CnE2PLC.Avalonia.Services;
using CnE2PLC.Avalonia.ViewModels;
using CnE2PLC.Avalonia.Views;

namespace CnE2PLC.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            var settings = SettingsService.Load();
            var vm = new MainWindowViewModel();
            vm.DebugVisible = settings.Debug;
            vm.DebugLevel = settings.DebugLevel;
            TraceService.CurrentDebugLevel = settings.DebugLevel;

            // Set Application.DataContext so NativeMenu bindings in App.axaml resolve.
            DataContext = vm;

            var mainWindow = new MainWindow
            {
                DataContext = vm,
                Width = settings.WindowWidth,
                Height = settings.WindowHeight,
            };

            if (settings.WindowMaximized)
            {
                mainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                mainWindow.Position = new PixelPoint((int)settings.WindowX, (int)settings.WindowY);
            }

            desktop.MainWindow = mainWindow;

            // Wire up the parts of the native menu that require code:
            // checked filter/level items and the dynamic recent files sub-menu.
            if (OperatingSystem.IsMacOS())
                WireNativeMenuDynamics(vm, mainWindow);

            // Handle macOS "Open With" / Finder double-click via Apple Events.
            if (ApplicationLifetime is IActivatableLifetime activatable)
            {
                activatable.Activated += (_, e) =>
                {
                    if (e.Kind == ActivationKind.File && e is FileActivatedEventArgs fileArgs)
                    {
                        var path = fileArgs.Files
                            .OfType<IStorageFile>()
                            .Select(f => f.Path.LocalPath)
                            .FirstOrDefault(p => p.EndsWith(".L5X", StringComparison.OrdinalIgnoreCase)
                                              || p.EndsWith(".l5x", StringComparison.OrdinalIgnoreCase));

                        if (path != null)
                        {
                            Dispatcher.UIThread.Post(async () =>
                                await vm.OpenFileByPathAsync(path));
                        }
                    }
                };
            }
        }

        base.OnFrameworkInitializationCompleted();
    }

    /// <summary>
    /// Builds the macOS system menu bar.
    /// - Application.NativeMenu → the macOS "app name" dropdown (About, Quit).
    /// - Window.NativeMenu     → additional top-level menus (File, PLC, Output, …).
    /// </summary>
    private void WireNativeMenuDynamics(MainWindowViewModel vm, Window mainWindow)
    {
        // ── Application menu — items defined in App.axaml; wire handlers here ─────
        var appMenu = NativeMenu.GetMenu(this)!;
        foreach (var item in appMenu.Items.OfType<NativeMenuItem>())
        {
            if (item.Header == "About CnE2PLC")
                item.Click += (_, _) => vm.ShowAboutCommand.Execute(null);
            else if (item.Header == "Quit CnE2PLC")
            {
                item.Gesture = new KeyGesture(Key.Q, KeyModifiers.Meta);
                item.Click += (_, _) => vm.QuitCommand.Execute(null);
            }
        }

        // ── Window menus (appear after the app menu in the system menu bar) ───────
        var windowMenuBar = new NativeMenu();

        // ── File ──────────────────────────────────────────────────────────────────
        var fileMenu = new NativeMenuItem("File") { Menu = new NativeMenu() };
        fileMenu.Menu!.Add(new NativeMenuItem("Open L5X…")
        {
            Command = vm.OpenL5XCommand,
            Gesture = new KeyGesture(Key.O, KeyModifiers.Meta),
        });

        var recentMenu = new NativeMenuItem("Open Recent") { Menu = new NativeMenu(), IsEnabled = false };
        void RefreshRecent()
        {
            recentMenu.Menu!.Items.Clear();
            foreach (var path in vm.RecentFiles)
            {
                var p = path;
                recentMenu.Menu.Add(new NativeMenuItem(p)
                {
                    Command = vm.OpenRecentFileCommand,
                    CommandParameter = p,
                });
            }
            recentMenu.IsEnabled = vm.RecentFiles.Count > 0;
        }
        RefreshRecent();
        vm.RecentFiles.CollectionChanged += (_, _) => RefreshRecent();

        fileMenu.Menu.Add(recentMenu);
        windowMenuBar.Add(fileMenu);

        // ── PLC ───────────────────────────────────────────────────────────────────
        var plcMenu = new NativeMenuItem("PLC") { Menu = new NativeMenu() };
        plcMenu.Menu!.Add(new NativeMenuItem("Get UDTs") { Command = vm.GetUDTsCommand });
        plcMenu.Menu.Add(new NativeMenuItem("Get Tags")  { Command = vm.GetTagsCommand });
        windowMenuBar.Add(plcMenu);

        // ── Output ────────────────────────────────────────────────────────────────
        var outputMenu = new NativeMenuItem("Output") { Menu = new NativeMenu() };
        outputMenu.Menu!.Add(new NativeMenuItem("Export Tags")    { Command = vm.ExportTagsCommand });
        outputMenu.Menu.Add(new NativeMenuItem("IO Report")       { Command = vm.IOReportCommand });
        outputMenu.Menu.Add(new NativeMenuItem("In Use Summary")  { Command = vm.InUseSummaryCommand });
        windowMenuBar.Add(outputMenu);

        // ── Filter ────────────────────────────────────────────────────────────────
        var filterMenu = new NativeMenuItem("Filter") { Menu = new NativeMenu() };
        filterMenu.Menu!.Add(MakeFilterItem(vm, "In Use",       () => vm.FilterInUse,       v => vm.FilterInUse       = v));
        filterMenu.Menu.Add(MakeFilterItem(vm,  "Simmed",       () => vm.FilterSimmed,      v => vm.FilterSimmed      = v));
        filterMenu.Menu.Add(MakeFilterItem(vm,  "Bypassed",     () => vm.FilterBypassed,    v => vm.FilterBypassed    = v));
        filterMenu.Menu.Add(MakeFilterItem(vm,  "Alarmed",      () => vm.FilterAlarmed,     v => vm.FilterAlarmed     = v));
        filterMenu.Menu.Add(MakeFilterItem(vm,  "Placeholder",  () => vm.FilterPlaceholder, v => vm.FilterPlaceholder = v));
        windowMenuBar.Add(filterMenu);

        // ── Debug ─────────────────────────────────────────────────────────────────
        var debugMenu = new NativeMenuItem("Debug") { Menu = new NativeMenu() };
        var levelMenu = new NativeMenuItem("Level") { Menu = new NativeMenu() };
        levelMenu.Menu!.Add(MakeLevelItem(vm, "Info",    3));
        levelMenu.Menu.Add(MakeLevelItem(vm,  "Warning", 2));
        levelMenu.Menu.Add(MakeLevelItem(vm,  "Error",   1));
        levelMenu.Menu.Add(MakeLevelItem(vm,  "None",    0));
        debugMenu.Menu!.Add(levelMenu);
        debugMenu.Menu.Add(new NativeMenuItem("Toggle Debug Panel") { Command = vm.ToggleDebugCommand });
        windowMenuBar.Add(debugMenu);

        // About is in the macOS app menu; no separate Help menu needed.

        NativeMenu.SetMenu(mainWindow, windowMenuBar);
    }

    private static NativeMenuItem MakeFilterItem(
        MainWindowViewModel vm, string header,
        Func<bool> getter, Action<bool> setter)
    {
        var item = new NativeMenuItem(header)
        {
            ToggleType = NativeMenuItemToggleType.CheckBox,
            IsChecked = getter(),
        };
        item.Click += (_, _) =>
        {
            setter(!getter());
            item.IsChecked = getter();
        };
        vm.PropertyChanged += (_, _) => item.IsChecked = getter();
        return item;
    }

    private static NativeMenuItem MakeLevelItem(MainWindowViewModel vm, string header, int level)
    {
        var item = new NativeMenuItem(header)
        {
            ToggleType = NativeMenuItemToggleType.CheckBox,
            IsChecked = vm.DebugLevel == level,
        };
        item.Click += (_, _) => vm.DebugLevel = level;
        vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(vm.DebugLevel))
                item.IsChecked = vm.DebugLevel == level;
        };
        return item;
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
            BindingPlugins.DataValidators.Remove(plugin);
    }
}
