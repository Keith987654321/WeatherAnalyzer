using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherAnalyzer.Models;

namespace WeatherAnalyzer.Services.Interfaces;

public interface IWeatherParser
{
    List<WeatherData> Parse(string html, string city);
}
