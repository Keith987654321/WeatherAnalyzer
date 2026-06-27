using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalyzer.Services.Interfaces;

public interface IWeatherHtmlDownloader
{
    Task<string> DownloadAsync(string url);
}
