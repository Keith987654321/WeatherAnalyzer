using System.Windows;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class LanguageManager : ILanguageManager
{
    public void SetRussian()
    {
        ApplyLanguage("Resources/Strings.ru.xaml");
    }

    public void SetEnglish()
    {
        ApplyLanguage("Resources/Strings.en.xaml");
    }

    private static void ApplyLanguage(string languagePath)
    {
        var application = Application.Current;

        if (application is null)
        {
            return;
        }

        var dictionaries = application.Resources.MergedDictionaries;

        // Ищем старый словарь локализации
        var existingDictionary = dictionaries.FirstOrDefault(d =>
            d.Source != null &&
            d.Source.OriginalString.Contains("Strings."));

        if (existingDictionary != null)
        {
            dictionaries.Remove(existingDictionary);
        }

        dictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(languagePath, UriKind.Relative)
        });
    }
}