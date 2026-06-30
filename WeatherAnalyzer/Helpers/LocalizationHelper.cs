using System.Windows;

namespace WeatherAnalyzer.Helpers;

public static class LocalizationHelper
{
    public static string GetString(string key)
    {
        return Application.Current.TryFindResource(key) as string ?? key;
    }
}