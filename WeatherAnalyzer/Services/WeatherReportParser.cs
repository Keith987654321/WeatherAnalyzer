using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class WeatherReportParser : IWeatherReportParser
{
    private static readonly string[] Days =
    [
        "Mon",
        "Tue",
        "Wed",
        "Thu",
        "Fri",
        "Sat",
        "Sun"
    ];

    private static bool IsDayHeader(string line)
    {
        return Days.Any(line.StartsWith);
    }

    public List<WeatherData> Parse(string report, string city)
    {
        var lines = SplitLines(report);
        System.Diagnostics.Debug.WriteLine(lines.Count);
        return new List<WeatherData>();
    }

    private static List<string> SplitLines(string report)
    {
        return report
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .ToList();
    }

}
