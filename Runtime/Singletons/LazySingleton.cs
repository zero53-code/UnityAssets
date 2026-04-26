using System;

namespace Zero53.Singletons
{
    public class LazySingleton<T>
    {
        private static readonly Lazy<T> _instance = new Lazy<T>();
        public static T instance => _instance.Value;
    }
}