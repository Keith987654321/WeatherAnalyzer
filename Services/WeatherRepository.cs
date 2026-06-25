using System.IO;
using System.Linq;
using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class WeatherRepository : IWeatherRepository
{
    private readonly string _dataDirectory;

    public WeatherRepository()
    {
        _dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");

        Directory.CreateDirectory(_dataDirectory);
    }

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
        return Directory
        .EnumerateFiles(_dataDirectory, "*.json")
        .Select(file => Path.GetFileNameWithoutExtension(file)!); // (file)! гарантирует компилятору, что результат не будет null, так как мы ожидаем, что файлы будут иметь имена.
    }
}