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

    private static readonly Regex TemperatureBlockRegex =
        new(@"\+?\d+(?:\(\d+\))?\s*°C|\d+\s*°C", RegexOptions.Compiled);

    private static readonly Regex NumberRegex =
        new(@"[-+]?\d+", RegexOptions.Compiled);

    private static readonly Regex DayRegex =
        new(@"(Sun|Mon|Tue|Wed|Thu|Fri|Sat)\s+(\d+\s+\w+)", RegexOptions.Compiled);

    private static readonly Regex WindRegex =
        new(@"\d+(?:-\d+)?\s*km/h", RegexOptions.Compiled);

    private static readonly Regex VisibilityRegex =
        new(@"\d+\s*km", RegexOptions.Compiled);

    private static readonly Regex RainRegex =
        new(@"\d+\.\d+\s*mm\s*\|\s*\d+%", RegexOptions.Compiled);
    

    private static bool IsDayHeader(string line)
    {
        return Days.Any(line.StartsWith);
    }

    private static bool IsPeriodHeader(string line)
    {
        return Periods.Contains(line);
    }

    private static int ParseTemperature(string value)
    {
        var match = NumberRegex.Match(value);

        return int.Parse(match.Value);
    }

    private static int ParseWindSpeed(string value)
    {
        var match = Regex.Match(value, @"\d+");

        return int.Parse(match.Value);
    }

    private static int ParseVisibility(string value)
    {
        var match = Regex.Match(value, @"\d+");

        return int.Parse(match.Value);
    }

    private static double ParsePrecipitation(string value)
    {
        var match = Regex.Match(value, @"\d+\.\d+");

        return double.Parse(
            match.Value,
            CultureInfo.InvariantCulture);
    }

    private static DateOnly ParseDate(string value)
    {
        var currentYear = DateTime.Now.Year;

        return DateOnly.ParseExact(
            $"{value} {currentYear}",
            "ddd dd MMM yyyy",
            CultureInfo.InvariantCulture);
    }

    public List<WeatherData> Parse(string report)
    {
        var result = new List<WeatherData>();
        var lines = SplitLines(report);

        string? currentDay = null;

        for (int i = 0; i < lines.Count; i++)
        {
            var dayMatch = DayRegex.Match(lines[i]);

            if (dayMatch.Success)
            {
                currentDay = $"{dayMatch.Groups[1].Value} {dayMatch.Groups[2].Value}";
                continue;
            }

            if (!(lines[i].Contains("Morning") &&
                  lines[i].Contains("Noon") &&
                  lines[i].Contains("Evening") &&
                  lines[i].Contains("Night")))
            {
                continue;
            }

            var periods = new[]
            {
            "Morning",
            "Noon",
            "Evening",
            "Night"
        };

            var tempLine = lines[i + 3];
            var windLine = lines[i + 4];
            var visibilityLine = lines[i + 5];
            var rainLine = lines[i + 6];

            var temperatures = TemperatureBlockRegex.Matches(tempLine);

            var winds = WindRegex.Matches(windLine);

            var visibility = VisibilityRegex.Matches(visibilityLine);

            var rains = RainRegex.Matches(rainLine);

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine($"===== {currentDay} =====");

            for (int j = 0; j < 4; j++)
            {
                var weather = new WeatherData
                {
                    Date = ParseDate(currentDay!),

                    Period = (DayPeriod)j,

                    Temperature = ParseTemperature(temperatures[j].Value),

                    WindSpeed = ParseWindSpeed(winds[j].Value),

                    Visibility = ParseVisibility(visibility[j].Value),

                    Precipitation = ParsePrecipitation(rains[j].Value),

                    Description = string.Empty
                };

                result.Add(weather);
            }
        }

        return result;
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

        var match = NumberRegex.Match(line);

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
