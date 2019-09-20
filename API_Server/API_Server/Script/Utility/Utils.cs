using System;
using System.Reactive.Linq;

namespace API_Server.Script.Utility
{
    public static class Utils
    {
        public static IObservable<T> Share<T>(this IObservable<T> observable) => observable.Publish().RefCount();
    }
}