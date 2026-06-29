using System.Windows;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class ThemeManager : IThemeManager
{
    public void SetLightTheme()
    {
        ApplyTheme("Themes/Light.xaml");
    }

    public void SetDarkTheme()
    {
        ApplyTheme("Themes/Dark.xaml");
    }

    private static void ApplyTheme(string themePath)
    {
        var application = Application.Current;

        if (application is null)
        {
            return;
        }

        var dictionaries = application.Resources.MergedDictionaries;

        dictionaries.Clear();

        dictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(themePath, UriKind.Relative)
        });
    }
}