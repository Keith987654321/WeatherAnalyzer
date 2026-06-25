using System.IO;
using System.Linq;
using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;
using System.Text.Json;

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

    public async Task<List<WeatherData>> LoadAsync(string city)
    {
        var filePath = GetFilePath(city);

        if (!File.Exists(filePath))
        {
            return [];
        }

        await using var stream = File.OpenRead(filePath);

        var weatherData =
            await JsonSerializer.DeserializeAsync<List<WeatherData>>(stream);

        return weatherData ?? []; // Если десериализация вернула null, возвращаем пустой список.
    }

    public IEnumerable<string> GetAvailableCities()
    {
        return Directory
        .EnumerateFiles(_dataDirectory, "*.json")
        .Select(file => Path.GetFileNameWithoutExtension(file)!); // (file)! гарантирует компилятору, что результат не будет null, так как мы ожидаем, что файлы будут иметь имена.
    }

    private string GetFilePath(string city)
    {
        return Path.Combine(_dataDirectory, $"{city}.json");
    }
}