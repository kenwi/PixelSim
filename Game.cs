using Leopotam.Ecs;
using Microsoft.AspNetCore.SignalR.Client;
using PixelSim.Components.Events;
using PixelSim.Systems;
using SFML.Graphics;
using SFML.Window;

namespace PixelSim
{
    public class Game : Singleton<Game>
    {
        EcsWorld world;
        EcsSystems systems;
        RenderWindow window;
        View view;
        HubConnection connection { get; set; } = new HubConnectionBuilder().WithUrl("https://hub.m0b.services/gamehub").Build();

        public void Init(uint width, uint height, uint targetFps = 60, bool vSync = false)
        {
            window = new RenderWindow(new VideoMode(1024, 768, VideoMode.DesktopMode.BitsPerPixel), "PixelSim");
            window.SetFramerateLimit(!vSync ? targetFps : 0);
            window.SetVerticalSyncEnabled(vSync);
            window.Closed += (sender, e) =>
            {
                window.Close();
            };

            var gameState = new GameState();
            view = window.DefaultView;
            world = new EcsWorld();
            systems = new EcsSystems(world)
                .Add(new PlayerSystem())
                .Add(new SignalRNetworkingSystem())
                .Add(new MovementSystem())
                .Add(new FpsCounterSystem())
                .Add(new LoggingSystem())

                .Inject(window)
                .Inject(view)
                .Inject(gameState)
                .Inject(connection)

                //.OneFrame<InputReleasedEvent>()
                //.OneFrame<MouseEvent>()
                .OneFrame<MoveRequest>()
                .OneFrame<KeyboardEvent>()
                .OneFrame<FpsCounterEvent>()
                .OneFrame<LogEvent>();
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
