namespace LiveScore.PerformanceProfilers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Fanex.Caching;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Soccer.Services;
    using SharpRaven;

    [InProcess, MinColumn, MaxColumn]
    public class MatchServiceBenchmark
    {
        public const string V1Api = nameof(V1Api);
        public const string V2Api = nameof(V2Api);
        private readonly CacheService cachingService;
        private readonly MockLoggingService loggingService;

        public MatchServiceBenchmark()
        {
            Akavache.Registrations.Start(nameof(MatchServiceBenchmark));
            cachingService = new CacheService();
            loggingService = new MockLoggingService();
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetMatchesBenchmarkParams))]
        public void BenchmarkGetMatches(string apiName, DateTime date, Language language, bool forceFetchNewData = false)
        {
            var httpService = new HttpService(new Uri(ApiUrls[apiName]));
            var apiService = new ApiService(new ApiPolicy(), httpService);
            var matchService = new MatchService(apiService, cachingService, loggingService);

            var matches = matchService.GetMatchesByDate(date, language, forceFetchNewData).GetAwaiter().GetResult();

            Console.WriteLine($"Total Match: {matches.Count()}");
        }

        private static Dictionary<string, string> ApiUrls = new Dictionary<string, string>
        {
            { V1Api, "https://score247-api1.nexdev.net/dev1/api/" },
            { V2Api, "https://score247-api1.nexdev.net/dev2/api/" }
        };

        public IEnumerable<object[]> GetMatchesBenchmarkParams()
        {
            yield return new object[] { V2Api, new DateTime(2019, 08, 28, 0, 0, 0, DateTimeKind.Utc), Language.English, true };
            yield return new object[] { V2Api, new DateTime(2019, 08, 28, 0, 0, 0, DateTimeKind.Utc), Language.English, false };
            yield return new object[] { V2Api, new DateTime(2019, 08, 29, 0, 0, 0, DateTimeKind.Utc), Language.English, true };
            yield return new object[] { V2Api, new DateTime(2019, 08, 29, 0, 0, 0, DateTimeKind.Utc), Language.English, false };
            yield return new object[] { V2Api, new DateTime(2019, 08, 30, 0, 0, 0, DateTimeKind.Utc), Language.English, true };
            yield return new object[] { V2Api, new DateTime(2019, 08, 30, 0, 0, 0, DateTimeKind.Utc), Language.English, false };
            yield return new object[] { V2Api, new DateTime(2019, 08, 31, 0, 0, 0, DateTimeKind.Utc), Language.English, true };
            yield return new object[] { V2Api, new DateTime(2019, 08, 31, 0, 0, 0, DateTimeKind.Utc), Language.English, false };
            yield return new object[] { V2Api, new DateTime(2019, 09, 01, 0, 0, 0, DateTimeKind.Utc), Language.English, true };
            yield return new object[] { V2Api, new DateTime(2019, 09, 01, 0, 0, 0, DateTimeKind.Utc), Language.English, false };
            yield return new object[] { V2Api, new DateTime(2019, 09, 02, 0, 0, 0, DateTimeKind.Utc), Language.English, true };
            yield return new object[] { V2Api, new DateTime(2019, 09, 02, 0, 0, 0, DateTimeKind.Utc), Language.English, false };
        }
    }

    public class MockEssential : IEssential
    {
        public string Model => "Model";

        public string Name => "Name";

        public string OperatingSystemName => "OperatingSystemName";

        public string OperatingSystemVersion => "OperatingSystemVersion";

        public string AppVersion => "AppVersion";

        public string AppName => "AppName";
    }

    public class MockLoggingService : ILoggingService
    {
#pragma warning disable S1186 // Methods should not be empty

        public void Init(string Dsn, IRavenClient ravenClient = null)
        {
        }

        public void LogError(Exception exception)
        {
        }

        public void LogError(string message, Exception exception)
        {
        }

        public Task LogErrorAsync(Exception exception)
            => Task.FromResult(0);

        public Task LogErrorAsync(string message, Exception exception)
            => Task.FromResult(0);

        public void TrackEvent(string trackIdentifier, string key, string value)
        {
        }

#pragma warning restore S1186 // Methods should not be empty
    }
}