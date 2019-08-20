namespace LiveScore.Common.Helpers
{
    using System.Collections.Concurrent;
    using System.Diagnostics;

    public static class Profiler
    {
        private static readonly ConcurrentDictionary<string, Stopwatch> watches = new ConcurrentDictionary<string, Stopwatch>();

        public static void Start(object view)
        {
            Start(view.GetType().Name);
        }

        public static void Start(string tag)
        {
            Debug.WriteLine($"Profiler: Starting {tag}");

            var watch = watches[tag] = new Stopwatch();

            watch.Start();
        }

        public static void Stop(string tag)
        {
            if (watches.TryGetValue(tag, out Stopwatch watch))
            {
                Debug.WriteLine($"Profiler: {tag} took {watch.Elapsed.TotalMilliseconds}ms");
            }
        }
    }
}
