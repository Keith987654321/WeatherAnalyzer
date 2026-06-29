using WeatherAnalyzer.Helpers;
using WeatherAnalyzer.Models;
using WeatherAnalyzer.Services;
using WeatherAnalyzer.Services.Interfaces;
using System.IO;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace WeatherAnalyzer.ViewModels;

public class MainViewModel : ViewModelBase
{
    public const int graphTextSize = 11;
    public const int graphGeometrySize = 11;
    public const int graphNameTextSize = 12;
    public const double graphLineSmoothness = 0.4;
    public MainViewModel(
    IWeatherRepository repository,
    IWeatherAnalyzer analyzer,
    IWeatherReportDownloader downloader,
    IWeatherReportParser parser,
    IThemeManager themeManager)
    {
        _repository = repository;
        _analyzer = analyzer;
        _downloader = downloader;
        _parser = parser;
        _themeManager = themeManager;

        LoadWeatherCommand = new RelayCommand(LoadWeatherAsync);

        Statistics = new WeatherStatistics();


    TemperatureSeries =
        [
            new LineSeries<int>
            {
                Name = "Температура",
                Values = [],
                GeometrySize = graphGeometrySize,
                LineSmoothness =graphLineSmoothness
            }
        ];

                XAxes =
                [
                    new Axis
            {
                TextSize = graphTextSize,
            }
                ];

                YAxes =
                [
                    new Axis
            {
                Name = "Температура (°C)",
                NameTextSize = graphNameTextSize,
                TextSize = graphTextSize,
                MinStep = 1
            }
                ];
    }

    private WeatherStatistics? _statistics;

    private readonly IWeatherRepository _repository;
    private readonly IWeatherAnalyzer _analyzer;
    private readonly IWeatherReportDownloader _downloader;
    private readonly IWeatherReportParser _parser;
    private readonly IThemeManager _themeManager;

    private List<WeatherData> _weatherHistory = [];

    public ObservableCollection<WeatherData> WeatherRecords { get; }
    = [];

    public ISeries[] TemperatureSeries { get; private set; } = [];

    public Axis[] XAxes { get; private set; } = [];

    public Axis[] YAxes { get; private set; } = [];

    private void NotifyStatisticsChanged()
    {
        OnPropertyChanged(nameof(AverageTemperature));
        OnPropertyChanged(nameof(MinimumTemperature));
        OnPropertyChanged(nameof(MaximumTemperature));
        OnPropertyChanged(nameof(AverageVisibility));
        OnPropertyChanged(nameof(AveragePrecipitationAmount));
        OnPropertyChanged(nameof(AveragePrecipitationProbability));
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

    public IReadOnlyList<ChartType> AvailableCharts { get; } =
    [
        ChartType.Temperature,
        ChartType.WindSpeed,
        ChartType.Visibility,
        ChartType.PrecipitationAmount,
        ChartType.PrecipitationProbability
    ];

    private ChartType _selectedChart = ChartType.Temperature;

    public double AverageTemperature =>
    Statistics?.AverageTemperature ?? 0;

    public double MinimumTemperature =>
        Statistics?.MinimumTemperature ?? 0;

    public double MaximumTemperature =>
        Statistics?.MaximumTemperature ?? 0;

    public double AverageVisibility =>
        Statistics?.AverageVisibility ?? 0;

    public double AveragePrecipitationAmount =>
        Statistics?.AveragePrecipitationAmount ?? 0;

    public double AveragePrecipitationProbability =>
        Statistics?.AveragePrecipitationProbability ?? 0;

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
            var weatherData = _parser.Parse(City, report);
            await _repository.SaveAsync(weatherData);
            var history = await _repository.LoadAsync(City);
            _weatherHistory = history;
            Statistics = _analyzer.Analyze(history);
            UpdateChart();
            WeatherRecords.Clear();

            foreach (var item in history)
            {
                WeatherRecords.Add(item);
            }

            Status = $"Загружено {history.Count} записей.";
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

    private void UpdateChart()
    {
        if (_weatherHistory.Count == 0)
            return;

        switch (SelectedChart)
        {
            case ChartType.Temperature:
                BuildTemperatureChart();
                break;

            case ChartType.WindSpeed:
                BuildWindChart();
                break;

            case ChartType.Visibility:
                BuildVisibilityChart();
                break;

            case ChartType.PrecipitationAmount:
                BuildPrecipitationAmountChart();
                break;

            case ChartType.PrecipitationProbability:
                BuildPrecipitationProbabilityChart();
                break;
        }
    }

    private void BuildChart(
    IEnumerable<double> values,
    string seriesName,
    string axisName)
    {
        TemperatureSeries =
        [
            new LineSeries<double>
        {
            Name = seriesName,
            GeometrySize = graphGeometrySize,
            LineSmoothness = graphLineSmoothness,
            Values = values.ToArray()
        }
        ];

        XAxes =
        [
            new Axis
        {
            Labels = _weatherHistory
                .Select(x => $"{x.Date:dd.MM}\n{GetPeriodShortName(x.Period)}")
                .ToArray(),

            TextSize = graphTextSize
        }
        ];

        YAxes =
        [
            new Axis
        {
            Name = axisName,
            NameTextSize = graphNameTextSize,
            TextSize = graphTextSize,
            MinStep = 1
        }
        ];

        OnPropertyChanged(nameof(TemperatureSeries));
        OnPropertyChanged(nameof(XAxes));
        OnPropertyChanged(nameof(YAxes));
    }

    private void BuildTemperatureChart()
    {
        BuildChart(
            _weatherHistory.Select(x => (double)x.Temperature),
            "Температура",
            "Температура (°C)");
    }

    private void BuildWindChart()
    {
        BuildChart(
            _weatherHistory.Select(x => x.WindSpeed),
            "Скорость ветра",
            "км/ч");
    }

    private void BuildVisibilityChart()
    {
        BuildChart(
            _weatherHistory.Select(x => x.Visibility),
            "Видимость",
            "км");
    }

    private void BuildPrecipitationAmountChart()
    {
        BuildChart(
            _weatherHistory.Select(x => x.PrecipitationAmount),
            "Количество осадков",
            "мм");
    }

    private void BuildPrecipitationProbabilityChart()
    {
        BuildChart(
            _weatherHistory.Select(x => x.PrecipitationProbability),
            "Вероятность осадков",
            "%");
    }

    private static string GetPeriodShortName(DayPeriod period)
    {
        return period switch
        {
            DayPeriod.Morning => "У",
            DayPeriod.Noon => "Д",
            DayPeriod.Evening => "В",
            DayPeriod.Night => "Н",
            _ => string.Empty
        };
    }

    public ChartType SelectedChart
    {
        get => _selectedChart;
        set
        {
            if (SetProperty(ref _selectedChart, value))
            {
                UpdateChart();
            }
        }
    }
}