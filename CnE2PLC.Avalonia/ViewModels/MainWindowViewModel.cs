using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CnE2PLC.Avalonia.Services;
using CnE2PLC.Avalonia.Views;
using CnE2PLC.Helpers;
using CnE2PLC.PLC;
using CnE2PLC.Reporting;
using SystemTask = System.Threading.Tasks.Task;

namespace CnE2PLC.Avalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private CnE2PLC.PLC.Controller? plc;
    [ObservableProperty] private string title = "CnE Toolbox";
    [ObservableProperty] private string tagCount = "No Tags";
    [ObservableProperty] private string udtCount = "No UDTs";
    [ObservableProperty] private string deviceCount = "No Devices";
    [ObservableProperty] private bool debugVisible = false;
    [ObservableProperty] private int debugLevel = 0;
    [ObservableProperty] private bool isLoading = false;
    [ObservableProperty] private string logText = string.Empty;

    [ObservableProperty] private bool filterInUse = false;
    [ObservableProperty] private bool filterSimmed = false;
    [ObservableProperty] private bool filterBypassed = false;
    [ObservableProperty] private bool filterAlarmed = false;
    [ObservableProperty] private bool filterPlaceholder = false;

    public ObservableCollection<LogEntry> LogEntries { get; } = new();
    public ObservableCollection<string> RecentFiles { get; } = new();

    private readonly StringBuilder _logBuilder = new();

    public IEnumerable<XTO_AOI> FilteredTags => Plc?.AOI_Tags ?? Enumerable.Empty<XTO_AOI>();

    public MainWindowViewModel()
    {
        TraceService.LogMessage += OnLogMessage;
        TraceService.Initialize();

        // Restore recent files from settings
        var settings = SettingsService.Load();
        foreach (var path in settings.RecentFiles.Where(File.Exists))
            RecentFiles.Add(path);

        // Re-evaluate all row color bindings when the user switches light/dark mode at runtime.
        if (Application.Current is { } app)
            app.ActualThemeVariantChanged += (_, _) => OnPropertyChanged(nameof(FilteredTags));
    }

    partial void OnPlcChanged(CnE2PLC.PLC.Controller? value)
    {
        OnPropertyChanged(nameof(FilteredTags));
    }

    partial void OnFilterInUseChanged(bool value)
    {
        if (Plc != null) Plc.Filter_InUse = value;
        OnPropertyChanged(nameof(FilteredTags));
    }

    partial void OnFilterSimmedChanged(bool value)
    {
        if (Plc != null) Plc.Filter_Simmed = value;
        OnPropertyChanged(nameof(FilteredTags));
    }

    partial void OnFilterBypassedChanged(bool value)
    {
        if (Plc != null) Plc.Filter_Bypassed = value;
        OnPropertyChanged(nameof(FilteredTags));
    }

    partial void OnFilterAlarmedChanged(bool value)
    {
        if (Plc != null) Plc.Filter_Alarmed = value;
        OnPropertyChanged(nameof(FilteredTags));
    }

    partial void OnFilterPlaceholderChanged(bool value)
    {
        if (Plc != null) Plc.Filter_Placeholder = value;
        OnPropertyChanged(nameof(FilteredTags));
    }

    private void OnLogMessage(object? sender, LogEntry entry)
    {
        Dispatcher.UIThread.Post(() =>
        {
            LogEntries.Add(entry);
            _logBuilder.AppendLine(entry.Message);
            LogText = _logBuilder.ToString();
        });
    }

    private global::Avalonia.Controls.Window? GetMainWindow() =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;

    /// <summary>
    /// Opens a file by path. Called from App startup when a path is passed via command-line args
    /// (e.g. macOS "Open With" / Finder double-click).
    /// </summary>
    public async SystemTask OpenFileByPathAsync(string path)
    {
        var mainWindow = GetMainWindow();
        if (mainWindow == null) return;

        await LoadFileAsync(path, Path.GetFileName(path), mainWindow);
    }

    [RelayCommand]
    private async SystemTask OpenL5X()
    {
        var mainWindow = GetMainWindow();
        if (mainWindow == null) return;

        var topLevel = global::Avalonia.Controls.TopLevel.GetTopLevel(mainWindow);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open L5X File",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("L5X Files") { Patterns = new[] { "*.L5X", "*.l5x" } },
                new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count == 0) return;

        var file = files[0];

        // Resolve the local path on the UI thread — IStorageFile stream operations
        // are not safe to call from background threads on macOS.
        var localPath = file.Path?.LocalPath;
        if (string.IsNullOrEmpty(localPath)) return;

        try
        {
            await LoadFileAsync(localPath, file.Name, mainWindow);
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowMessage("Error", $"Failed to open L5X file:\n{ex.Message}", mainWindow);
        }
    }

    [RelayCommand]
    private async SystemTask OpenRecentFile(string path)
    {
        var mainWindow = GetMainWindow();
        if (mainWindow == null) return;

        if (!File.Exists(path))
        {
            await DialogHelper.ShowMessage("File Not Found", $"The file no longer exists:\n{path}", mainWindow);
            RecentFiles.Remove(path);
            var s = SettingsService.Load();
            s.RecentFiles.Remove(path);
            SettingsService.Save(s);
            return;
        }

        await LoadFileAsync(path, Path.GetFileName(path), mainWindow);
    }

    private async SystemTask LoadFileAsync(string localPath, string displayName, global::Avalonia.Controls.Window mainWindow)
    {
        IsLoading = true;
        try
        {
            await SystemTask.Run(() =>
            {
                var doc = new XmlDocument();
                doc.Load(localPath);
                var controllerNode = doc.SelectSingleNode("//Controller")
                    ?? throw new InvalidOperationException("No Controller node found in L5X file.");
                var loaded = new CnE2PLC.PLC.Controller(controllerNode);
                Dispatcher.UIThread.Post(() =>
                {
                    Plc = loaded;
                    Title = $"CnE Toolbox — {displayName}";
                    TagCount = $"Tags: {Plc.AOI_Tags.Count}";
                    UdtCount = $"UDTs: {Plc.DataTypes.Count}";
                    DeviceCount = $"Devices: {Plc.Modules.Count}";

                    // Update recent files list and persist
                    RecentFiles.Remove(localPath);
                    RecentFiles.Insert(0, localPath);
                    while (RecentFiles.Count > 10) RecentFiles.RemoveAt(RecentFiles.Count - 1);

                    var settings = SettingsService.Load();
                    settings.AddRecentFile(localPath);
                    SettingsService.Save(settings);
                });
            });
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowMessage("Error", $"Failed to open file:\n{ex.Message}", mainWindow);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async SystemTask ExportTags()
    {
        if (Plc == null || Plc.AOI_Tags.Count == 0)
        {
            await DialogHelper.ShowMessage("Export Tags", "No tags loaded.", GetMainWindow()!);
            return;
        }

        var mainWindow = GetMainWindow();
        if (mainWindow == null) return;
        var topLevel = global::Avalonia.Controls.TopLevel.GetTopLevel(mainWindow);
        if (topLevel == null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save CnE Report",
            SuggestedFileName = $"{Plc.Name}_CnE_Report.xlsx",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Excel Files") { Patterns = new[] { "*.xlsx" } }
            }
        });

        if (file == null) return;

        IsLoading = true;
        try
        {
            var path = file.Path.LocalPath;
            await SystemTask.Run(() =>
            {
                using var stream = System.IO.File.Create(path);
                CnE_Report.CreateReport(Plc, stream);
            });
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowMessage("Export Error", $"Failed to export tags:\n{ex.Message}", mainWindow);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async SystemTask IOReport()
    {
        if (Plc == null || Plc.AOI_Tags.Count == 0)
        {
            await DialogHelper.ShowMessage("IO Report", "No tags loaded.", GetMainWindow()!);
            return;
        }

        var mainWindow = GetMainWindow();
        if (mainWindow == null) return;
        var topLevel = global::Avalonia.Controls.TopLevel.GetTopLevel(mainWindow);
        if (topLevel == null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save IO Report",
            SuggestedFileName = $"{Plc.Name}_IO_Report.xlsx",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("Excel Files") { Patterns = new[] { "*.xlsx" } }
            }
        });

        if (file == null) return;

        IsLoading = true;
        try
        {
            var path = file.Path.LocalPath;
            await SystemTask.Run(() =>
            {
                using var stream = System.IO.File.Create(path);
                IO_Report.CreateReport(Plc, stream);
            });
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            await DialogHelper.ShowMessage("IO Report Error", $"Failed to generate IO report:\n{ex.Message}", mainWindow);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async SystemTask InUseSummary()
    {
        if (Plc == null)
        {
            await DialogHelper.ShowMessage("In Use Summary", "No PLC loaded.", GetMainWindow()!);
            return;
        }

        var tags = Plc.AOI_Tags;
        int ai = tags.OfType<AIData>().Count(t => !t.NotInUse);
        int di = tags.OfType<DIData>().Count(t => !t.NotInUse);
        int ao = tags.Count(t => t.DataType.Equals("AOData", StringComparison.OrdinalIgnoreCase) && !t.NotInUse);
        int doCount = tags.Count(t => t.DataType.Equals("DOData", StringComparison.OrdinalIgnoreCase) && !t.NotInUse);

        var msg = $"In-Use Device Summary for {Plc.Name}:\n\n" +
                  $"  Analog Inputs (AI):   {ai}\n" +
                  $"  Analog Outputs (AO):  {ao}\n" +
                  $"  Digital Inputs (DI):  {di}\n" +
                  $"  Digital Outputs (DO): {doCount}\n" +
                  $"\n  Total: {ai + ao + di + doCount}";

        await DialogHelper.ShowMessage("In Use Summary", msg, GetMainWindow()!);
    }

    [RelayCommand]
    private void Quit() => Environment.Exit(0);

    [RelayCommand]
    private void ToggleDebug() => DebugVisible = !DebugVisible;

    [RelayCommand] private void SetDebugLevelInfo()    => SetDebugLevel(3);
    [RelayCommand] private void SetDebugLevelWarning() => SetDebugLevel(2);
    [RelayCommand] private void SetDebugLevelError()   => SetDebugLevel(1);
    [RelayCommand] private void SetDebugLevelNone()    => SetDebugLevel(0);

    public bool DebugLevelIsInfo    => DebugLevel == 3;
    public bool DebugLevelIsWarning => DebugLevel == 2;
    public bool DebugLevelIsError   => DebugLevel == 1;
    public bool DebugLevelIsNone    => DebugLevel == 0;

    private void SetDebugLevel(int level)
    {
        DebugLevel = level;
        TraceService.CurrentDebugLevel = level;
        OnPropertyChanged(nameof(DebugLevelIsInfo));
        OnPropertyChanged(nameof(DebugLevelIsWarning));
        OnPropertyChanged(nameof(DebugLevelIsError));
        OnPropertyChanged(nameof(DebugLevelIsNone));
    }

    [RelayCommand]
    private void GetUDTs()
    {
        try
        {
            var result = Plc?.PrintUdtTags();
            if (!string.IsNullOrEmpty(result))
                LogHelper.DebugPrint(result);
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: GetUDTs failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private void GetTags()
    {
        try
        {
            var result = Plc?.PrintTags();
            if (!string.IsNullOrEmpty(result))
                LogHelper.DebugPrint(result);
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: GetTags failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private async SystemTask ShowAbout()
    {
        var mainWindow = GetMainWindow();
        if (mainWindow == null) return;
        var dialog = new AboutDialog();
        await dialog.ShowDialog(mainWindow);
    }
}

