using MyHome.Models;

namespace MyHomeBlazorApp.BlazorData
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate)
        {


            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }
        public WeatherForecastService()
        {
            string path = @"C:\Temp\usersListTestData111.xml";

            Users = MyHome.Data.GetUsersListFromXml(path);
        }

        public List<UserProfile> Users { get; set; } = new List<UserProfile>();
    }


}