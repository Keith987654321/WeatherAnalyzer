using WeatherAnalyzer.Models;

namespace WeatherAnalyzer.Services.Interfaces;

public interface IWeatherRepository
{
    Task SaveAsync(IEnumerable<WeatherData> weatherData);

    Task<List<WeatherData>> LoadAsync(string city);

    IEnumerable<string> GetAvailableCities();
}
