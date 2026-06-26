using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services;

namespace WeatherAnalyzer.Tests;

public class WeatherAnalyzerTests
{
    [Fact]
    public void Analyze_ShouldCalculateStatistics()
    {
        // Arrange
        var analyzer = new WeatherStatisticsAnalyzer();

        var weatherData = new List<WeatherData>
    {
        CreateWeatherData(
            new DateTime(2026, 6, 25),
            temperature: 20,
            humidity: 40,
            pressure: 1000,
            windSpeed: 4),

        CreateWeatherData(
            new DateTime(2026, 6, 26),
            temperature: 30,
            humidity: 60,
            pressure: 1020,
            windSpeed: 6)
    };

        // Act
        var statistics = analyzer.Analyze(weatherData);

        // Assert
        Assert.Equal(25, statistics.AverageTemperature);
        Assert.Equal(20, statistics.MinimumTemperature);
        Assert.Equal(30, statistics.MaximumTemperature);

        Assert.Equal(50, statistics.AverageHumidity);
        Assert.Equal(1010, statistics.AveragePressure);
        Assert.Equal(5, statistics.AverageWindSpeed);
    }

    private static WeatherData CreateWeatherData(
    DateTime date,
    double temperature,
    int humidity = 50,
    int pressure = 1013,
    double windSpeed = 5,
    string city = "Berlin")
    {
        return new WeatherData
        {
            City = city,
            Date = date,
            Temperature = temperature,
            Humidity = humidity,
            Pressure = pressure,
            WindSpeed = windSpeed
        };
    }

    [Fact]
    public void Analyze_ShouldSetRecordsCount()
    {
        // Arrange
        var analyzer = new WeatherStatisticsAnalyzer();

        var weatherData = new List<WeatherData>
    {
        CreateWeatherData(new DateTime(2026, 6, 25), 20),
        CreateWeatherData(new DateTime(2026, 6, 26), 21),
        CreateWeatherData(new DateTime(2026, 6, 27), 22)
    };

        // Act
        var statistics = analyzer.Analyze(weatherData);

        // Assert
        Assert.Equal(3, statistics.RecordsCount);
    }

    [Fact]
    public void Analyze_ShouldThrowArgumentException_WhenCollectionIsEmpty()
    {
        // Arrange
        var analyzer = new WeatherStatisticsAnalyzer();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            analyzer.Analyze([]));
    }
}