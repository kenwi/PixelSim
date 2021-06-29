using System;

namespace PixelSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.Instance.Init(1024, 768);
            Game.Instance.Run();
        }
    }
}