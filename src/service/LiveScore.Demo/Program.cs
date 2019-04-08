namespace LiveScore.Demo
{
    using System;
    using LiveScore.Features.Leagues;
    using Newtonsoft.Json;
    using Refit;

    public class Program
    {
        static void Main(string[] args)
        {
            var leagueApi = RestService.For<ILeagueApi>("http://localhost:64296/api");

            JsonConvert.DefaultSettings =
                () => new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

            var leagues = leagueApi.GetLeagues(
                1, 
                DateTime.Now.ToString(), 
                DateTime.Now.ToString())
                .Result;

            Console.WriteLine("Done");
        }
    }
}
