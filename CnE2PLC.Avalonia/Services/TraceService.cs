using System;
using CnE2PLC.Helpers;

namespace CnE2PLC.Avalonia.Services;

public enum LogLevel
{
    None = 0,
    Error = 1,
    Warning = 2,
    Info = 3
}

public record LogEntry(string Message, LogLevel Level);

public static class TraceService
{
    public static event EventHandler<LogEntry>? LogMessage;

    public static int CurrentDebugLevel { get; set; } = 0;

    public static void Initialize()
    {
        foreach (System.Diagnostics.TraceListener l in System.Diagnostics.Trace.Listeners)
        {
            if (l is UiTraceListener listener)
            {
                listener.TraceOutput += OnTraceOutput;
                break;
            }
        }
    }

    private static void OnTraceOutput(object? sender, string message)
    {
        var entry = Parse(message.TrimEnd());
        if (entry.Level == LogLevel.None || CurrentDebugLevel >= (int)entry.Level)
            LogMessage?.Invoke(null, entry);
    }

    private static LogEntry Parse(string message)
    {
        if (message.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase))
            return new LogEntry(message, LogLevel.Error);
        if (message.StartsWith("WARN", StringComparison.OrdinalIgnoreCase))
            return new LogEntry(message, LogLevel.Warning);
        if (message.StartsWith("INFO", StringComparison.OrdinalIgnoreCase))
            return new LogEntry(message, LogLevel.Info);
        return new LogEntry(message, LogLevel.None);
    }
}
