using WeatherAnalyzer.Helpers;
using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services;
using WeatherAnalyzer.Services.Interfaces;

namespace WeatherAnalyzer.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IWeatherRepository repository,
    IWeatherAnalyzer analyzer,
    IWeatherHtmlDownloader downloader)
    {
        LoadWeatherCommand = new RelayCommand(LoadWeather);

        Statistics = new WeatherStatistics
        {
            AverageTemperature = 0,
            MinimumTemperature = 0,
            MaximumTemperature = 0,
            AverageHumidity = 0,
            AveragePressure = 0,
            AverageWindSpeed = 0,
            RecordsCount = 0
        };
    }

    private WeatherStatistics? _statistics;

    private void NotifyStatisticsChanged()
    {
        OnPropertyChanged(nameof(AverageTemperature));
        OnPropertyChanged(nameof(MinimumTemperature));
        OnPropertyChanged(nameof(MaximumTemperature));
        OnPropertyChanged(nameof(AverageHumidity));
        OnPropertyChanged(nameof(AveragePressure));
        OnPropertyChanged(nameof(AverageWindSpeed));
        OnPropertyChanged(nameof(RecordsCount));
    }

    public WeatherStatistics? Statistics
    {
        get => _statistics;
        set
        {
            if (SetProperty(ref _statistics, value))
            {
                NotifyStatisticsChanged();
            }
        }
    }

    public double AverageTemperature =>
    Statistics?.AverageTemperature ?? 0;

    public double MinimumTemperature =>
        Statistics?.MinimumTemperature ?? 0;

    public double MaximumTemperature =>
        Statistics?.MaximumTemperature ?? 0;

    public double AverageHumidity =>
        Statistics?.AverageHumidity ?? 0;

    public double AveragePressure =>
        Statistics?.AveragePressure ?? 0;

    public double AverageWindSpeed =>
        Statistics?.AverageWindSpeed ?? 0;

    public int RecordsCount =>
        Statistics?.RecordsCount ?? 0;

    private string _city = string.Empty;

    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }

    public RelayCommand LoadWeatherCommand { get; }

    private void LoadWeather()
    {
        Statistics = new WeatherStatistics
        {
            AverageTemperature = 23.5,
            MinimumTemperature = 18,
            MaximumTemperature = 29,
            AverageHumidity = 65,
            AveragePressure = 1013,
            AverageWindSpeed = 4.2,
            RecordsCount = 7
        };
    }
}