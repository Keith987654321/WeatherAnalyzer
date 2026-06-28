using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using WeatherAnalyzer.Models;

namespace WeatherAnalyzer.Services.Interfaces;

public interface IWeatherReportParser
{
    List<WeatherData> Parse(string city, string report);

}
