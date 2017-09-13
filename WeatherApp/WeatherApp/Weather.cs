using System;
using System.Threading.Tasks;

namespace WeatherApp
{
  public enum TemperatureUnits
  {
    [Description("C")]
    tuCelsius,
    [Description("F")]
    tuFahrenheit
  }
  public enum WindUnits
  {
    [Description("mph")]
    wuMph,
    [Description("km/h")]
    wuKmh
  }

  public class Weather
  {
    public string Title { get; set; }
    public double Temperature { get; set; }
    public double Wind { get; set; }
    public double Humidity { get; set; }
    public string Visibility { get; set; }
    public string Sunrise { get; set; }
    public string Sunset { get; set; }

    public Weather()
    {
      //Because labels bind to these values, set them to an empty string to 
      //ensure that the label appears on all platforms by default. 
      this.Title = default(string);
      this.Temperature = default(double);
      this.Wind = default(double);
      this.Humidity = default(double);
      this.Visibility = default(string);
      this.Sunrise = default(string);
      this.Sunset = default(string);
    }

    public static async Task<Weather> RetrieveWeather(string pZipCode, string pCountryCode, bool pUseMetric)
    {
      //Sign up for a free API key at http://openweathermap.org/appid 
      string key = "CHANGE THIS STRING";

      string queryString = default(String); //"http://api.openweathermap.org/data/2.5/weather?zip=";

      //IF pUseMetric, set the unitsString to "metric" else "imperial"
      string unitsString = (pUseMetric ? "metric" : "imperial");

      if (String.IsNullOrEmpty(pCountryCode)) //If Country is not specified, Open Weather Defaults to the US.
        queryString = $"http://api.openweathermap.org/data/2.5/weather?zip={pZipCode},us&appid={key}&units={unitsString}";
      else
        queryString = $"http://api.openweathermap.org/data/2.5/weather?zip={pZipCode},{pCountryCode}&appid={key}&units={unitsString}";

      //Make sure developers running this sample replaced the API key
      if (key != "CHANGE THIS STRING")
        throw new ArgumentOutOfRangeException("You must obtain an API key from openweathermap.org/appid and save it in the 'key' variable.");

      dynamic results = null;

      try
      {
        results = await DataService.getDataFromService(queryString).ConfigureAwait(false);

        if (results["weather"] != null)
        {  
          DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

          return new Weather()
          {
            Title = (string)results["name"],
            Temperature = (double)results["main"]["temp"],
            Wind = (double)results["wind"]["speed"],
            Humidity = (double)results["main"]["humidity"],
            Visibility = (string)results["weather"][0]["main"],
            Sunrise = $"{time.AddSeconds((double)results["sys"]["sunrise"]).ToString()} UTC",
            Sunset = $"{time.AddSeconds((double)results["sys"]["sunset"]).ToString()} UTC"
          };
        }
        else
          throw new Exception((string)results["message"]);
      }
      catch (Exception ex)
      {
        if (results == null)
          throw new Exception($"Unable to retreived Weather data.  Please check your internet connection ({ex.Message})");
        else
          throw new Exception(ex.Message);
      }
    }
  }
}