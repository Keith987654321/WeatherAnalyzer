using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class WeatherRepository : IWeatherRepository
{
    public Task SaveAsync(IEnumerable<WeatherData> weatherData)
    {
        throw new NotImplementedException();
    }

    public Task<List<WeatherData>> LoadAsync(string city)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetAvailableCities()
    {
        throw new NotImplementedException();
    }
}