using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CnE2PLC.Avalonia.Services;

public class SettingsService
{
    private const int MaxRecentFiles = 10;

    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "CnE2PLC", "settings.json");

    public double WindowX { get; set; } = 100;
    public double WindowY { get; set; } = 100;
    public double WindowWidth { get; set; } = 1200;
    public double WindowHeight { get; set; } = 700;
    public bool WindowMaximized { get; set; } = false;
    public bool Debug { get; set; } = false;
    public int DebugLevel { get; set; } = 0;
    public List<string> RecentFiles { get; set; } = new();

    /// <summary>
    /// Adds a path to the top of the recent files list, removing duplicates and capping at MaxRecentFiles.
    /// </summary>
    public void AddRecentFile(string path)
    {
        RecentFiles.RemoveAll(p => string.Equals(p, path, StringComparison.OrdinalIgnoreCase));
        RecentFiles.Insert(0, path);
        if (RecentFiles.Count > MaxRecentFiles)
            RecentFiles = RecentFiles.Take(MaxRecentFiles).ToList();
    }

    public static SettingsService Load()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<SettingsService>(json) ?? new SettingsService();
            }
        }
        catch { }
        return new SettingsService();
    }

    public static void Save(SettingsService settings)
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsPath)!;
            Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
        catch { }
    }
}
