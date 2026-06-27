using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.Services;

public class CitySearcher : ICitySearcher
{
    public Task<string> FindCityUrlAsync(string city)
    {
        throw new NotImplementedException();
    }
}
