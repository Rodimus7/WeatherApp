using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Forms;

namespace WeatherApp
{
  public class Core: INotifyPropertyChanged
  {
    public string ButtonContent { get { return "Get Weather"; } }

    private bool UseMetric;

    public void SelectCountryByISOCode()
    {
      foreach (Country item in Countries)
        if (item.CountryCode == System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName)
          SelectedCountry = item;
    }

    private Country _SelectedCountry;
    public Country SelectedCountry
    {
      get { return _SelectedCountry; }
      set
      {
        if (_SelectedCountry != value)
        {
          _SelectedCountry = value;
          OnPropertyChanged(nameof(SelectedCountry));

          UseMetric = new RegionInfo(SelectedCountry?.CountryCode).IsMetric;

          ZipCode = "";
 }

      }
    }
    public ObservableCollection<Country> Countries = Country.Init();

    private bool _CanGetWeather = true;
    public bool CanGetWeather
    {
      get { return _CanGetWeather; }
      set
      {
        if (_CanGetWeather != value)
        {
          _CanGetWeather = value;
          OnPropertyChanged(nameof(CanGetWeather));
        }
      }
    }

    public ICommand GetWeather { get; private set; }

    public Core()
    {
      GetWeather = new Command(async () => await _GetWeather(), () => CanGetWeather);
    }

    private string _ZipCode;
    public string ZipCode
    {
      get { return _ZipCode; }
      set
      {
        if (_ZipCode != value)
        {
          _ZipCode = value;
          OnPropertyChanged(nameof(ZipCode));

          CanGetWeather = !String.IsNullOrEmpty(value);
        }
      }
    }
    public string Title
    {
      get { return _weather.Title; }
      set
      {
        if (_weather.Title != value)
        {
          _weather.Title = value;
          OnPropertyChanged(nameof(Title));
        }
      }
    }
    public double Temperature
    {
       get { return _weather.Temperature; }
       set
       {
         if (_weather.Temperature != value)
         {
           _weather.Temperature = value;
           OnPropertyChanged(nameof(Temperature));
           OnPropertyChanged(nameof(TemperatureDisplay));
         }
       }
    }
    public string TemperatureDisplay => $"{Temperature.ToString()} {(UseMetric ? TemperatureUnits.tuCelsius.EnumDescription() : TemperatureUnits.tuFahrenheit.EnumDescription())}";
    public double Wind
    {
      get { return _weather.Wind; }
      set
      {
        if (_weather.Wind != value)
        {
          _weather.Wind = value;
          OnPropertyChanged(nameof(Wind));
        }
      }
    }
    public string WindUnits => $"{(UseMetric ? WeatherApp.WindUnits.wuKmh.EnumDescription() : WeatherApp.WindUnits.wuMph.EnumDescription())}";
    public double Humidity
    {
      get { return _weather.Humidity; }
      set
      {
        if (_weather.Humidity != value)
        {
          _weather.Humidity = value;
          OnPropertyChanged(nameof(Humidity));
        }
      }
    }
    public string Visibility
    {
       get { return _weather.Visibility; }
       set
       {
         if (_weather.Visibility != value)
         {
           _weather.Visibility = value;
           OnPropertyChanged(nameof(Visibility));
         }
       }
    }
    public string Sunrise
    {
      get { return _weather.Sunrise; }
      set
      {
        if (_weather.Sunrise != value)
        {
          _weather.Sunrise = value;
          OnPropertyChanged(nameof(Sunrise));
        }
      }
    }
    public string Sunset
    {
      get { return _weather.Sunset; }
      set
      {
        if (_weather.Sunset != value)
        {
          _weather.Sunset = value;
          OnPropertyChanged(nameof(Sunset));
        }
      }
    }
    private string _ErrorMessage = default(string);
    public string ErrorMessage
    {
      get { return _ErrorMessage; }
      set
      {
        if (value != _ErrorMessage)
        {
          _ErrorMessage = value;
          OnPropertyChanged(nameof(ErrorMessage));
        }
      }
    }

    private Weather _weather = new Weather();
    public Weather Weather
    {
      get { return _weather; }
      private set
      {
        if ((_weather != value) && (value != null))
        {
          _weather = value;

          OnPropertyChanged(nameof(Title));
          OnPropertyChanged(nameof(Temperature));
          OnPropertyChanged(nameof(TemperatureDisplay));
          OnPropertyChanged(nameof(Wind));
          OnPropertyChanged(nameof(WindUnits));
          OnPropertyChanged(nameof(Humidity));
          OnPropertyChanged(nameof(Visibility));
          OnPropertyChanged(nameof(Sunrise));
          OnPropertyChanged(nameof(Sunset));
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
       var changed = PropertyChanged;
       if (changed != null)
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task _GetWeather()
    {
      CanGetWeather = false;

      try
      {
        try
        {
          ErrorMessage = default(string);

          //Clear our the results so that we are not displaying incorrect information should an excption occur.
          Title = default(String);
          Temperature = default(double);
          Wind = default(double);
          Humidity = default(double);
          Visibility = default(String);
          Sunrise = default(String);
          Sunset = default(String);

          Weather = await Weather.RetrieveWeather(ZipCode, SelectedCountry?.CountryCode, UseMetric);
        }
        catch (Exception ex)
        {
          ErrorMessage = ex.Message;
        }
      }
      finally
      {
        CanGetWeather = true;
      }
    }
  }
}