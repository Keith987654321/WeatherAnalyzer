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

    public async Task SaveAsync(IEnumerable<WeatherData> weatherData)
    {
        var newData = weatherData.ToList();

        if (newData.Count == 0)
        {
            return;
        }

        var city = newData[0].City;

        var existingData = await LoadAsync(city);

        foreach (var newItem in newData)
        {
            var existingItem = existingData.FirstOrDefault(
                item => item.Date == newItem.Date);

            if (existingItem is null)
            {
                existingData.Add(newItem);
            }
            else
            {
                existingItem.Temperature = newItem.Temperature;
                existingItem.Humidity = newItem.Humidity;
                existingItem.Pressure = newItem.Pressure;
                existingItem.WindSpeed = newItem.WindSpeed;
            }
        }

        var filePath = GetFilePath(city);

        existingData = existingData    
            .OrderBy(item => item.Date)
            .ToList();

        await using var stream = File.Create(filePath);

        await JsonSerializer.SerializeAsync(
            stream,
            existingData,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });
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
        return Path.Combine(_dataDirectory, $"{GetSafeFileName(city)}.json");
    }

    private static string GetSafeFileName(string city)
    {
        city = city.Trim();

        var invalidChars = Path.GetInvalidFileNameChars();

        var safeName = new string(
            city
                .Select(c => invalidChars.Contains(c) ? '_' : c)
                .ToArray());

        return safeName.Replace(' ', '_');
    }
}