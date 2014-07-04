using System;
using System.Diagnostics;

namespace Dommel.Linq.Utils
{
    public class Profiler : IDisposable
    {
        private readonly string _reference;
        private readonly Stopwatch _sw;

        public Profiler(string reference)
        {
            _reference = reference;
            _sw = Stopwatch.StartNew();
        }

        public static void Profile(string reference, Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            Console.WriteLine("{0:HH:mm:ss.ffff}: {1} - elapsed: {2}ms", DateTime.Now, reference, sw.Elapsed.TotalMilliseconds);
        }

        public void Dispose()
        {
            _sw.Stop();
            Console.WriteLine("{0:HH:mm:ss.ffff}: {1} - elapsed: {2}ms", DateTime.Now, _reference, _sw.Elapsed.TotalMilliseconds);
        }
    }
}
