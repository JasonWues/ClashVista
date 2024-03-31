using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Clash_Vista.Converters;

public class BytesToGigabytesConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {

        if (value is long b)
        {
            return b / 1024 / 1024 / 1024;
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}