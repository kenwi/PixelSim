using Leopotam.Ecs;
using PixelSim.Services;
using SFML.Window;
using TiledSharp;
using SFML.System;
using SFML.Graphics;
using System;
using System.IO;
using System.Reflection;

namespace PixelSim.Systems
{
    public class TileRenderingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly RenderWindow window = default;
        TmxMap map = default;
        TiledRenderer service = default;
        readonly View view = default;

        public void Init()
        {
            var runtimePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(runtimePath, "Test1.tmx");
            map = new TmxMap(filePath);
            service = new TiledRenderer(map);
        }

        public void Run()
        {
            window.SetView(view);
            service.Draw(window);
        }
    }
}
