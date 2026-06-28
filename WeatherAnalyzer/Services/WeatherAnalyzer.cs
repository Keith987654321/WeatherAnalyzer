using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class WeatherStatisticsAnalyzer : IWeatherAnalyzer
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
            AverageWindSpeed = Round(data.Average(x => x.WindSpeed)),
            RecordsCount = data.Count,
            AverageVisibility = weatherData.Average(x => x.Visibility),
            AveragePrecipitationAmount = weatherData.Average(x => x.PrecipitationAmount),
            AveragePrecipitationProbability = weatherData.Average(x => x.PrecipitationProbability)
        };
    }
    private static double Round(double value)
    {
        return Math.Round(value, 1);
    }
}