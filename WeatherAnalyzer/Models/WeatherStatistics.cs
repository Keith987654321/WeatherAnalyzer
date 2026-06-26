using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalyzer.Models;

public class WeatherStatistics
{
    public double AverageTemperature { get; init; }

    public double MinimumTemperature { get; init; }

    public double MaximumTemperature { get; init; }

    public double AverageHumidity { get; init; }

    public double AveragePressure { get; init; }

    public double AverageWindSpeed { get; init; }

    public int RecordsCount { get; init; }
}
