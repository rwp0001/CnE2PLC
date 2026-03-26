using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace CnE2PLC.Avalonia.Converters;

/// <summary>
/// Converts a bool to a GridLength — true returns the size specified by the parameter (default 200),
/// false returns 0 (collapsed).
/// </summary>
public class BoolToGridLengthConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is true)
        {
            if (parameter is string s && double.TryParse(s, out double d))
                return new GridLength(d);
            return new GridLength(200);
        }
        return new GridLength(0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
