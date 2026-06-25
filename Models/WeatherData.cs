namespace WeatherAnalyzer.Models;

public class WeatherData
{
    public string City { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public double Temperature { get; set; }

    public int Humidity { get; set; }

    public int Pressure { get; set; }

    public double WindSpeed { get; set; }
}