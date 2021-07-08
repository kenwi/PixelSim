using Leopotam.Ecs;
using Microsoft.AspNetCore.SignalR.Client;
using PixelSim.Components.Events;
using PixelSim.Extensions;

namespace PixelSim.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        readonly EcsFilter<MoveRequest> moveRequests = default;
        readonly GameState gameState = default;
        readonly HubConnection connection = default;
        readonly EcsWorld world = default;

        public void Run()
        {
            if (moveRequests.GetEntitiesCount() == 0)
                return;

            ref var request = ref moveRequests.Get1(0);
            if (request.Online)
            {
                MoveOnline(request);
            }
            else
            {
                MoveOffline(request);
            }
        }

        private void MoveOffline(MoveRequest request)
        {
            world.SendLogEvent($"IsOnline: {gameState.IsOnline}");
            world.SendLogEvent($"Request move offline: {request.X} {request.Y}");
        }

        private void MoveOnline(MoveRequest request)
        {
            connection.InvokeCoreAsync("Move", args: new object[] { request.X, request.Y }).Wait();
        }
    }
}
