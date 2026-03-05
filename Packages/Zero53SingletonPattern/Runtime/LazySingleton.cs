using System;

namespace Zero53.SingletonPattern
{
    public class LazySingleton<T>
    {
        private static readonly Lazy<T> _instance = new Lazy<T>();
        public static T instance => _instance.Value;
    }
}