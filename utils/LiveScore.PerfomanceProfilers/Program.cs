﻿// <auto-generated>
// </auto-generated>
namespace LiveScore.PerformanceProfilers
{
#pragma warning disable S1118 // Utility classes should not have public constructors
    public class Program
#pragma warning restore S1118 // Utility classes should not have public constructors
    {
        private static void Main(string[] args)
        {
            //BenchmarkRunner.Run<MatchServiceBenchmark>(ManualConfig
            //            .Create(DefaultConfig.Instance)
            //            .With(ConfigOptions.DisableOptimizationsValidator));

            TestMatchMessagePack.GetMatchByDate();
        }
    }
}