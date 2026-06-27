using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class WeatherHtmlDownloader : IWeatherHtmlDownloader
{
    private readonly HttpClient _httpClient = new();

    public async Task<string> DownloadAsync(string url)
    {
        return await _httpClient.GetStringAsync(url);
    }
}
