namespace LiveScore.PerfomanceProfilers
{
    using System;
    using BenchmarkDotNet.Running;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;

    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MatchServiceBenchmark>();
        }
    }
}
