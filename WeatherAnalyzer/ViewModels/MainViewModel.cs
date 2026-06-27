namespace WeatherAnalyzer.ViewModels;

public class MainViewModel : ViewModelBase
{
    private string _city = string.Empty;

    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }
}