using Leopotam.Ecs;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.Window;
using System;
using PixelSim.Extensions;
using System.Threading.Tasks;
using SFML.System;
using PixelSim.Services;

namespace PixelSim.Systems
{
    public class NetworkingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsWorld world = default;
        HubConnection connection;
        Clock clock;
        int id;

        public void Init()
        {
            id = new RandomService((int)DateTime.Now.Ticks).Range(0, 100000);
            connection = new HubConnectionBuilder()
            .WithUrl("https://hub.m0b.services/chathub")
            .Build();

            connection.On("ReceiveMessage", (string username, string message) =>
            {
                world.SendLogEvent($"ReceiveMessage: FROM {username} MSG: {message}");
            });
            connection.StartAsync().Wait();
            clock = new Clock();
        }

        public void Run()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
                if(clock.ElapsedTime.AsSeconds() > 0.0001)
                {
                    connection.InvokeCoreAsync("ReceiveMessage", args: new[] { id.ToString(), $"Hello at {DateTime.Now}" }).Wait();
                    clock.Restart();
                }
            }
        }
    }
}
