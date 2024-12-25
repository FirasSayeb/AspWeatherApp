namespace WeaterApp.Models
{
    public class WeatherData
    {


        public string city { get; set; }

        public double[] temperature { get; set; }
        public double[] windSpeed { get; set; }
        public double[] humidity { get; set; }
    }
}
