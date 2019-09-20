using System;
using System.Reactive.Linq;
using System.Text;

namespace API_Server.Script.Utility
{
    public static class Utils
    {
        public static IObservable<T> Share<T>(this IObservable<T> observable) => observable.Publish().RefCount();
        
        public static string Trim(this string str,int start,int end) => str.AsSpan().Slice(start, end).ToString();
        public static string GUID => Guid.NewGuid().ToString().AsSpan().Slice(0, 8).ToString();
    }
}