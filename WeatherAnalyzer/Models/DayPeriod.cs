using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalyzer.Models;

public enum DayPeriod
{
    Morning,
    Noon,
    Evening,
    Night
}

public static class DayPeriodExtensions
{
    public static string ToDisplayString(this DayPeriod period)
    {
        return period switch
        {
            DayPeriod.Morning => "Утро",
            DayPeriod.Noon => "День",
            DayPeriod.Evening => "Вечер",
            DayPeriod.Night => "Ночь",
            _ => period.ToString()
        };
    }
}