using System;
using System.Globalization;
using Xamarin.Forms;

namespace WeatherApp
{
  class PercentageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return $"{value.ToString()} %";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}