using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WeatherAnalyzer.Helpers;
using WeatherAnalyzer.Services;
using WeatherAnalyzer.ViewModels;

namespace WeatherAnalyzer.Views
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var languageManager = new LanguageManager();

            DataContext = new MainViewModel(
                new WeatherRepository(),
                new WeatherStatisticsAnalyzer(),
                new WeatherReportDownloader(),
                new WeatherReportParser(),
                new ThemeManager(),
                languageManager);

            languageManager.LanguageChanged += UpdateColumnHeaders;

            Loaded += (_, _) => UpdateColumnHeaders();
        }

        private void UpdateColumnHeaders()
        {
            ForecastGrid.Columns[0].Header = LocalizationHelper.GetString("ColumnDate");
            ForecastGrid.Columns[1].Header = LocalizationHelper.GetString("ColumnPeriod");
            ForecastGrid.Columns[2].Header = LocalizationHelper.GetString("ColumnTemperature");
            ForecastGrid.Columns[3].Header = LocalizationHelper.GetString("ColumnWind");
            ForecastGrid.Columns[4].Header = LocalizationHelper.GetString("ColumnVisibility");
            ForecastGrid.Columns[5].Header = LocalizationHelper.GetString("ColumnRain");
            ForecastGrid.Columns[6].Header = LocalizationHelper.GetString("ColumnProbability");
        }
    }
}
