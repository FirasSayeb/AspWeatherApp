namespace WeaterApp.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public string City { get; set; }
       
        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double Humidity { get; set; }
    }
}
