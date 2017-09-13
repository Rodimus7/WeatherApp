using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace WeatherApp
{
  public class DataService
  {
#pragma warning disable IDE1006 // Naming Styles
    public static async Task<dynamic> getDataFromService(string pQueryString)
#pragma warning restore IDE1006 // Naming Styles
    {
      HttpClient client = new HttpClient();
      var response = await client.GetAsync(pQueryString);

      dynamic data = null;
      if (response != null)
      {
         string json = response.Content.ReadAsStringAsync().Result;
         data = JsonConvert.DeserializeObject(json);
      }

      return data;
    }
  }
}