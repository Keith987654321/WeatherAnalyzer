using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

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

    private static readonly string[] Periods =
    [
        "Morning",
        "Noon",
        "Evening",
        "Night"
    ];

    private static readonly Regex TemperatureRegex =
    new(@"[-+]?\d+", RegexOptions.Compiled);

    private static bool IsDayHeader(string line)
    {
        return Days.Any(line.StartsWith);
    }

    private static bool IsPeriodHeader(string line)
    {
        return Periods.Contains(line);
    }

    public List<WeatherData> Parse(string report)
    {
        var lines = SplitLines(report);
        var currentDay = string.Empty;

        foreach (var line in lines)
        {
            if (!line.Contains('│'))
            {
                continue;
            }

            var columns = SplitColumns(line);

            if (columns.Count == 4)
            {
                System.Diagnostics.Debug.WriteLine("-----");

                foreach (var column in columns)
                {
                    System.Diagnostics.Debug.WriteLine(column);
                }
            }
        }

        return [];
    }

    private static List<string> SplitLines(string report)
    {
        return report
            .Replace("\r\n", "\n")
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();
    }

    private static bool TryParseTemperature(
    string line,
    out int temperature)
    {
        temperature = 0;

        if (!line.Contains("°C"))
        {
            return false;
        }

        var match = TemperatureRegex.Match(line);

        if (!match.Success)
        {
            return false;
        }

        return int.TryParse(
            match.Value,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out temperature);
    }

    private static List<string> SplitColumns(string line)
    {
        return line
            .Split('│', StringSplitOptions.RemoveEmptyEntries)
            .Select(column => column.Trim())
            .ToList();
    }
}
