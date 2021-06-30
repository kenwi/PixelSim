using Leopotam.Ecs;
using PixelSim.Components.Events;
using PixelSim.Extensions;
using PixelSim.Services;
using SFML.Graphics;
using SFML.System;
using System;
using System.IO;
using System.Reflection;
using TiledSharp;

namespace PixelSim.Systems
{
    public class TileRenderingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilter<MapReloadEvent> reloadEvents = default;
        readonly EcsWorld world = default;
        readonly RenderWindow window = default;
        TmxMap map = default;
        TileRenderingService service = default;
        readonly View view = default;
        DateTime mapLastWrite;
        Clock clock;
        string filePath;

        public void Init()
        {
            var mapName = "Testmap1.tmx";
            var runtimePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filePath = Path.Combine(runtimePath, "Assets", mapName);
            mapLastWrite = File.GetLastWriteTime(filePath);
            map = new TmxMap(filePath);
            service = new TileRenderingService(map);
            clock = new Clock();
            world.SendLogEvent("TileRenderingSystem initialized");
        }

        public void Run()
        {
            if(clock.ElapsedTime.AsSeconds() > 2)
            {
                var lastWrite = File.GetLastWriteTime(filePath);
                if (lastWrite > mapLastWrite)
                {
                    world.SendLogEvent("Map size changed");
                    world.NewEntity().Get<MapReloadEvent>();
                }
            }

            if(!reloadEvents.IsEmpty())
            {
                Init();
            }

            window.SetView(view);
            service.Draw(window);
        }
    }
}
