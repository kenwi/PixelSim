using Leopotam.Ecs;
using Microsoft.AspNetCore.SignalR.Client;
using SFML.System;
using System.Threading.Tasks;
using PixelSim.Extensions;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using PixelSim.Components.Events;

namespace PixelSim.Systems
{
    public class SignalRNetworkingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilter<MoveRequest> moveRequests = default;
        readonly EcsWorld world = default;
        readonly HubConnection connection = default;
        readonly GameState gameState = default;

        readonly Clock clock = new Clock();
        Vector2f position = new Vector2f();

        public void Init()
        {
            connection.On("BroadcastPositions", (float[] p) => OnBroadCastPositionsReceived(p));
            connection.On("RequestPlayerCount", (int count) => OnRequestPlayerCountReceived(count));
            connection.StartAsync().Wait();
            gameState.IsOnline = true;
            world.SendLogEvent($"IsOnline: {gameState.IsOnline}");
        }

        private void OnRequestPlayerCountReceived(int count)
        {
            world.SendLogEvent($"Player count: {count}");
        }

        public void Run()
        {
            if(moveRequests.GetEntitiesCount() > 0)
            {
                if (clock.ElapsedTime.AsSeconds() > 1.0f)
                {
                    world.SendLogEvent($"Player position: {position.X} {position.Y}");
                    connection.InvokeCoreAsync("RequestPlayerCount", new object[] { });
                    clock.Restart();
                    return;
                }
            }
        }

        private void OnBroadCastPositionsReceived(float[] p)
        {
            gameState.PlayerPositions.Clear();
            for (int i=0; i<p.Length; i += 2)
            {
                gameState.PlayerPositions.Add(new Vector2f(p[i], p[i + 1]));
            }
        }
    }
}
