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

    public async Task<string> DownloadAsync(string city)
    {
        var url = BuildUrl(city);

        return await _httpClient.GetStringAsync(url);
    }

    private static string BuildUrl(string city)
    {
        city = city.Trim();

        return $"https://wttr.in/{Uri.EscapeDataString(city)}";
    }
}