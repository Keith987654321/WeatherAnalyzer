namespace WeatherAnalyzer.Models;

public class WeatherData
{
    public string City { get; set; }
    public DateOnly Date { get; set; }

    public DayPeriod Period { get; set; }

    public int Temperature { get; set; }

    public double WindSpeed { get; set; }

    public double PrecipitationAmount { get; set; }

    public double PrecipitationProbability { get; set; }

    public double Visibility { get; set; }

    public string Description { get; set; } = string.Empty;

    public string PeriodName => Period.ToDisplayString();
}