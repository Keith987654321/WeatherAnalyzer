using WeatherAnalyzer.Helpers;
using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services;
using WeatherAnalyzer.Services.Interfaces;
using System.IO;

namespace WeatherAnalyzer.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(
    IWeatherRepository repository,
    IWeatherAnalyzer analyzer,
    IWeatherReportDownloader downloader,
    IWeatherReportParser parser)
    {
        _repository = repository;
        _analyzer = analyzer;
        _downloader = downloader;
        _parser = parser;

        LoadWeatherCommand = new RelayCommand(LoadWeatherAsync);

        Statistics = new WeatherStatistics();
    }

    private WeatherStatistics? _statistics;

    private readonly IWeatherRepository _repository;
    private readonly IWeatherAnalyzer _analyzer;
    private readonly IWeatherReportDownloader _downloader;
    private readonly IWeatherReportParser _parser;

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

    private async Task LoadWeatherAsync()
    {
        if (string.IsNullOrWhiteSpace(City))
        {
            Status = "Введите название города.";
            return;
        }

        try
        {
            Status = "Загрузка данных...";

            var report = await _downloader.DownloadAsync(City);

            report = AnsiTextCleaner.Clean(report);
            var weatherData = _parser.Parse(report);
            Status = report[..Math.Min(report.Length, 300)];

            // await File.WriteAllTextAsync("CleanedResponse.txt", report);
        }
        catch (Exception ex)
        {
            Status = $"Ошибка: {ex.Message}";
        }
    }

    private string _status = "Введите название города.";

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }
}