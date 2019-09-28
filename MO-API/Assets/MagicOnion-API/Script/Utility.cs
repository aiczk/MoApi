using System;
using System.Threading;

namespace MagicOnion.Utils
{
    public static class Utility
    {
        public static string Trim(this string str,int start,int end) => str.AsSpan().Slice(start, end).ToString();
        public static string GUID => Guid.NewGuid().ToString().AsSpan().Slice(0, 8).ToString();
    }

    public static class RandomProvider
    {
        private static int seed = Environment.TickCount;
        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        public static int GetRandomValue() => randomWrapper.Value.Next();
    }
}
