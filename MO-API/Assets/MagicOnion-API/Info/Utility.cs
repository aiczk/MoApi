using System;

namespace Info
{
    public static class Utility
    {
        public static string Trim(this string str,int start,int end) => str.AsSpan().Slice(start, end).ToString();
        public static string GUID => Guid.NewGuid().ToString().AsSpan().Slice(0, 8).ToString();
    }
}
