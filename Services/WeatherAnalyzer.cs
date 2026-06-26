using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class WeatherAnalyzer : IWeatherAnalyzer
{
    public WeatherStatistics Analyze(IEnumerable<WeatherData> weatherData)
    {
        var data = weatherData.ToList();

        if (data.Count == 0)
        {
            throw new ArgumentException(
                "Weather data collection cannot be empty.",
                nameof(weatherData));
        }

        return new WeatherStatistics
        {
            AverageTemperature = Round(data.Average(x => x.Temperature)),
            MinimumTemperature = data.Min(x => x.Temperature),
            MaximumTemperature = data.Max(x => x.Temperature),

            AverageHumidity = Round(data.Average(x => x.Humidity)),
            AveragePressure = Round(data.Average(x => x.Pressure)),
            AverageWindSpeed = Round(data.Average(x => x.WindSpeed)),

            RecordsCount = data.Count
        };
    }
    private static double Round(double value)
    {
        return Math.Round(value, 1);
    }
}