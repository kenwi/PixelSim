using System;

namespace PixelSim
{
    public class Singleton<T>
    {
        static readonly Lazy<T> instance = new Lazy<T>();
        public static T Instance => instance.Value;
    }
}
