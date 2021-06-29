using Leopotam.Ecs;
using PixelSim.Components.Events;
using PixelSim.Systems;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace PixelSim
{
    public class Game : Singleton<Game>
    {
        EcsWorld world;
        EcsSystems systems;
        RenderWindow window;
        View view;

        public void Init(uint width, uint height, uint targetFps = 120, bool vSync = false)
        {
            window = new RenderWindow(new VideoMode(1024, 768, VideoMode.DesktopMode.BitsPerPixel), "PixelSim");
            window.Position += new Vector2i(2000, 0);
            window.SetFramerateLimit(!vSync ? targetFps : 0);
            window.SetVerticalSyncEnabled(vSync);
            window.Closed += (sender, e) =>
            {
                window.Close();
            };

            view = window.DefaultView;
            world = new EcsWorld();
            systems = new EcsSystems(world)
                .Add(new InputSystem())
                .Add(new InputProcessingSystem())
                .Add(new TileRenderingSystem())
                .Add(new FpsCounterSystem())
                .Add(new LoggingSystem())

                .Inject(window)
                .Inject(view)

                .OneFrame<InputReleasedEvent>()
                .OneFrame<MouseEvent>()
                .OneFrame<KeyboardEvent>()
                .OneFrame<FpsCounterEvent>()
                .OneFrame<LogEvent>()
                .OneFrame<VertexUpdateEvent>();
            systems.Init();
        }

        public void Run()
        {
            while (window.IsOpen)
            {
                window.Clear();
                systems.Run();
                window.Display();
                window.DispatchEvents();
            }

            if (systems != null)
            {
                System.Console.WriteLine("Final shutdown");
                systems.Destroy();
                systems = null;
                world.Destroy();
                world = null;
            }
            System.Console.WriteLine("Exit");
        }
    }
}
