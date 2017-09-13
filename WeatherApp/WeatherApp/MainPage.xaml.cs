using Xamarin.Forms;

namespace WeatherApp
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();

      BindingContext = new Core();

      cbxCountry.ItemsSource = ((Core)this.BindingContext).Countries;

      ((Core)this.BindingContext).SelectCountryByISOCode();

      edtZipCode.Completed += (s, e) => this.GetWeather();
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();

      if (cbxCountry.SelectedIndex > -1)
        edtZipCode.Focus();
      else
        cbxCountry.Focus();
    }

    private void GetWeather()
    {
      if (((Core)BindingContext).GetWeather.CanExecute(null))
        ((Core)BindingContext).GetWeather.Execute(null);
    }
  }
}