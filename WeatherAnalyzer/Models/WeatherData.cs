namespace WeatherAnalyzer.Models;

public class WeatherData
{
    public string City { get; set; }
    public DateOnly Date { get; set; }

    public DayPeriod Period { get; set; }

    public int Temperature { get; set; }

    public int WindSpeed { get; set; }

    public int Humidity { get; set; }

    public int Pressure { get; set; }

    public double Precipitation { get; set; }

    public int Visibility { get; set; }

    public string Description { get; set; } = string.Empty;
}