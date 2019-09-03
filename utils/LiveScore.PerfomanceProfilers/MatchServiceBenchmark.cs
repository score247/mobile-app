namespace LiveScore.PerfomanceProfilers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Services;
    using SharpRaven;

    [InProcessAttribute, MinColumn, MaxColumn]
    public class MatchServiceBenchmark
    {
        public const string V1Api = nameof(V1Api);
        public const string V2Api = nameof(V2Api);
        private readonly MockSettingsService settingsService;
        private readonly CachingService cachingService;
        private readonly MockLoggingService loggingService;
        private readonly ApiService apiService;

        public MatchServiceBenchmark()
        {
            Akavache.Registrations.Start(nameof(MatchServiceBenchmark));
            cachingService = new CachingService(new MockEssential());
            loggingService = new MockLoggingService();
            settingsService = new MockSettingsService();
            apiService = new ApiService(new ApiPolicy(), settingsService, new Refit.RefitSettings());
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetMatchesBenchmarkParams))]
        public void BenchmarkGetMatches(string apiName, DateRange dateRange, Language language, bool forceFetchNewData = false)
        {
            settingsService.ApiEndpoint = ApiUrls[apiName];
            var matchService = new MatchService(apiService, cachingService, loggingService);

            matchService.GetMatches(dateRange, language, forceFetchNewData).GetAwaiter().GetResult();
        }

        private static Dictionary<string, string> ApiUrls = new Dictionary<string, string>
        {
            { V1Api, "https://score247-api1.nexdev.net/dev1/api/" },
            { V2Api, "https://score247-api1.nexdev.net/dev2/api/" }
        };

        public IEnumerable<object[]> GetMatchesBenchmarkParams()
        {
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 28, 0, 0, 0, DateTimeKind.Utc)), Language.English, true };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 28, 0, 0, 0, DateTimeKind.Utc)), Language.English, false };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 29, 0, 0, 0, DateTimeKind.Utc)), Language.English, true };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 29, 0, 0, 0, DateTimeKind.Utc)), Language.English, false };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 30, 0, 0, 0, DateTimeKind.Utc)), Language.English, true };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 30, 0, 0, 0, DateTimeKind.Utc)), Language.English, false };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 31, 0, 0, 0, DateTimeKind.Utc)), Language.English, true };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 08, 31, 0, 0, 0, DateTimeKind.Utc)), Language.English, false };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 09, 01, 0, 0, 0, DateTimeKind.Utc)), Language.English, true };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 09, 01, 0, 0, 0, DateTimeKind.Utc)), Language.English, false };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 09, 02, 0, 0, 0, DateTimeKind.Utc)), Language.English, true };
            yield return new object[] { V1Api, new DateRange(new DateTime(2019, 09, 02, 0, 0, 0, DateTimeKind.Utc)), Language.English, false };
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
    }

    public class MockSettingsService : ISettingsService
    {
        public bool IsDemo { get; set; }
        public string CurrentLanguage { get; set; }
        public string ApiEndpoint { get; set; }
        public string HubEndpoint { get; set; }

        public Language Language => Language.English;

        public SportType CurrentSportType { get; set; }
        public TimeZoneInfo CurrentTimeZone { get; set; }

        public UserSettings UserSettings => new UserSettings(SportType.Soccer.Value.ToString(), Language.English.Value.ToString(), "07");
    }
}