using System;
using Leopotam.Ecs;

namespace PixelSim.Services
{    
    public class RandomService
    {
        private readonly Random _rng;

        public RandomService(int? seed)
        {
            _rng = seed.HasValue ? new Random(seed.Value) : new Random();
        }
        
        public int Range(int min, int max)
        {
            return _rng.Next(min, max);
        }

        public float Range(float min, float max)
        {
            return min + (max - min) * (float)_rng.NextDouble();
        }
    }
}