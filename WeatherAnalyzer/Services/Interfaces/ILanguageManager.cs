namespace WeatherAnalyzer.Services.Interfaces;

public interface ILanguageManager
{
    event Action? LanguageChanged;
    void SetRussian();

    void SetEnglish();
}