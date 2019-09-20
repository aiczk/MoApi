using System;
using System.Reactive.Linq;

namespace API_Server.Script.Utility
{
    public static class Utils
    {
        public static IObservable<T> Share<T>(this IObservable<T> observable) => observable.Publish().RefCount();
        
        public static string Trim(this string str,int start,int end) => str.AsSpan().Slice(start, end).ToString();
        public static string GUID => Guid.NewGuid().ToByteArray().AsSpan().Slice(0, 10).ToString();
    }
}