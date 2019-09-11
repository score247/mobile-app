namespace LiveScore.PerformanceProfilers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Caching;
    using LiveScore.Common.Helpers;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Soccer.Services;
    using MessagePack.Resolvers;
    using Refit;

    public static class TestMatchMessagePack
    {
        public static void GetMatchByDate()
        {
            try
            {

                CompositeResolver.RegisterAndSetAsDefault(
                    SoccerModelResolver.Instance,
                    CoreModelResolver.Instance,
                    BuiltinResolver.Instance,
                    PrimitiveObjectResolver.Instance);

                var cachingService = new CacheService();
                var loggingService = new MockLoggingService();
                var httpService = new HttpService(new Uri("http://localhost:57392/api"));
                var apiService = new ApiService(
                    new ApiPolicy(),
                    httpService,
                    new RefitSettings
                    {
                        ContentSerializer = new MessagePackContentSerializer()
                    });
                var matchService = new MatchService(apiService, cachingService, loggingService);

                var matches = matchService.GetMatchesByDate(DateTime.Now, Language.English, true).GetAwaiter().GetResult();
                var match = matchService.GetMatch(matches.FirstOrDefault().Id, Language.English, true).GetAwaiter().GetResult();

                ////var oddsService = new OddsService(loggingService, apiService);

                ////var matchOdds = oddsService.GetOdds("en", "testmatch", 1, "eu", true).GetAwaiter().GetResult();

                ////var matchOddsMovement = oddsService.GetOddsMovement("en", "testmatch", 1, "eu", "bookmaker id", true).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
