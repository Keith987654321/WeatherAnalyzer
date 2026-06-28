using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalyzer.Models;

public class WeatherStatistics
{
    public double AverageTemperature { get; set; }

    public double MinimumTemperature { get; set; }

    public double MaximumTemperature { get; set; }

    public double AverageWindSpeed { get; set; }

    public double AverageVisibility { get; set; }

    public double AveragePrecipitationAmount { get; set; }

    public double AveragePrecipitationProbability { get; set; }

    public int RecordsCount { get; set; }
}
