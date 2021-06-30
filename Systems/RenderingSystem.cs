using Leopotam.Ecs;
using PixelSim.Components.Events;
using SFML.Window;
using PixelSim.Extensions;
using SFML.System;
using SFML.Graphics;
using PixelSim.Services;

namespace PixelSim.Systems
{
    public class VertexRenderingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilter<VertexUpdateEvent> updateEvents = default;
        readonly RenderWindow window = default;
        readonly EcsWorld world = default;
        VertexBuffer buffer;

        public void Init()
        {
            var simSize = new Vector2u(32, 32);
            var vertices = QuadService.CreateVertexArray(simSize.X * simSize.Y, new Vector2u(window.Size.X, window.Size.Y), new Vector2u(32, 32), Color.White);
            buffer = new VertexBuffer(simSize.X * simSize.Y * 4, PrimitiveType.Quads, VertexBuffer.UsageSpecifier.Static);
            buffer.Update(vertices);

            world.SendLogEvent("RenderingSystem initialized");
        }

        public void Run()
        {
            if(!updateEvents.IsEmpty())
            {
                foreach(var index in updateEvents)
                {
                    ref var updatedVertices = ref updateEvents.Get1(index).Value;
                    buffer.Update(updatedVertices);
                    world.SendLogEvent("Vertexbuffer updated");
                }
            }
            window.Draw(buffer);
        }
    }
}
