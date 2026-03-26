using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using CnE2PLC.PLC;

namespace CnE2PLC.Avalonia.Converters;

// ---------------------------------------------------------------------------
// State resolver — maps an XTO_AOI to its semantic TagRowState
// ---------------------------------------------------------------------------
public static class TagStateResolver
{
    public static TagRowState GetState(XTO_AOI tag)
    {
        // Error — highest priority
        if (tag.Simmed)                                              return TagRowState.Error;
        if (tag is AIData ai0 && ai0.BadPVAlarm == true)            return TagRowState.Error;
        if (tag is Intlk_8 il0 && il0.IntlkOK != true)             return TagRowState.Error;

        // Alarm
        if (tag is AIData ai1 && (ai1.HiHiAlarm == true || ai1.LoLoAlarm == true)) return TagRowState.Alarm;
        if (tag is DIData di  && di.Alarm == true)                  return TagRowState.Alarm;
        if (tag is Intlk_8 il1 && il1.BypActive == true)           return TagRowState.Alarm;

        // Warning
        if (tag is AIData ai2 && (ai2.HiAlarm == true || ai2.LoAlarm == true)) return TagRowState.Warning;

        // Disabled
        if (tag.Placeholder || tag.AOICalls == 0 || tag.NotInUse)  return TagRowState.Disabled;

        return TagRowState.Default;
    }
}

// ---------------------------------------------------------------------------
// Row theme — defines colors for each state in light and dark mode
// ---------------------------------------------------------------------------
public static class RowTheme
{
    public sealed record StateColors(
        IBrush?    Background,
        IBrush     Foreground,
        FontWeight FontWeight = default,
        FontStyle  FontStyle  = default)
    {
        public FontWeight FontWeight { get; } = FontWeight == default ? FontWeight.Normal : FontWeight;
        public FontStyle  FontStyle  { get; } = FontStyle  == default ? FontStyle.Normal  : FontStyle;
    }

    // Pre-built brushes
    private static readonly IBrush DimBg        = new SolidColorBrush(Color.FromArgb(50,  128, 128, 128));
    private static readonly IBrush DimFg        = new SolidColorBrush(Color.Parse("#888888"));

    // Warning — amber
    private static readonly IBrush WarnBgLight  = new SolidColorBrush(Color.Parse("#FFF3CD"));
    private static readonly IBrush WarnFgLight  = new SolidColorBrush(Color.Parse("#7B5800"));
    private static readonly IBrush WarnBgDark   = new SolidColorBrush(Color.Parse("#3D2E00"));
    private static readonly IBrush WarnFgDark   = Brushes.Gold;

    // Alarm — orange
    private static readonly IBrush AlarmBgLight = new SolidColorBrush(Color.Parse("#FF8C00"));
    private static readonly IBrush AlarmBgDark  = new SolidColorBrush(Color.Parse("#7A3800"));
    private static readonly IBrush AlarmFgDark  = Brushes.Orange;

    // Error — red
    private static readonly IBrush ErrBgLight   = new SolidColorBrush(Color.Parse("#DC3545"));
    private static readonly IBrush ErrBgDark    = new SolidColorBrush(Color.Parse("#6B1520"));
    private static readonly IBrush ErrFgDark    = new SolidColorBrush(Color.Parse("#FF8080"));

    private static bool IsDark =>
        Application.Current?.ActualThemeVariant == ThemeVariant.Dark;

    public static StateColors Get(TagRowState state)
    {
        bool dark = IsDark;
        return state switch
        {
            TagRowState.Disabled => new(DimBg,       DimFg,                              FontStyle: FontStyle.Italic),
            TagRowState.Warning  => dark
                ? new(WarnBgDark,  WarnFgDark)
                : new(WarnBgLight, WarnFgLight),
            TagRowState.Alarm    => dark
                ? new(AlarmBgDark, AlarmFgDark,  FontWeight.Bold)
                : new(AlarmBgLight, Brushes.White, FontWeight.Bold),
            TagRowState.Error    => dark
                ? new(ErrBgDark,   ErrFgDark,   FontWeight.Bold)
                : new(ErrBgLight,  Brushes.White, FontWeight.Bold),
            _ /* Default */      => new(null, dark ? Brushes.White : Brushes.Black),
        };
    }
}

// ---------------------------------------------------------------------------
// Converters — all delegate to TagStateResolver + RowTheme
// ---------------------------------------------------------------------------

public class TagBgConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not XTO_AOI tag) return null;
        return RowTheme.Get(TagStateResolver.GetState(tag)).Background;
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class TagFgConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not XTO_AOI tag) return RowTheme.Get(TagRowState.Default).Foreground;
        return RowTheme.Get(TagStateResolver.GetState(tag)).Foreground;
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class TagFontWeightConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not XTO_AOI tag) return FontWeight.Normal;
        return RowTheme.Get(TagStateResolver.GetState(tag)).FontWeight;
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class TagFontStyleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not XTO_AOI tag) return FontStyle.Normal;
        return RowTheme.Get(TagStateResolver.GetState(tag)).FontStyle;
    }
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}




