using Leopotam.Ecs;
using PixelSim.Components.Events;
using PixelSim.Services;
using SFML.Graphics;
using Spectre.Console;

namespace PixelSim.Extensions
{
    public static class EcsWorldEx
    {
        public static void SendLogEvent(this EcsWorld world, string message)
        {
            LoggingService.WriteLine(message);
            //world.NewEntity().Get<LogEvent>().Value = message;
        }

        public static void SendVertexUpdateEvent(this EcsWorld world, Vertex[] vertices)
        {
            world.NewEntity().Get<VertexUpdateEvent>().Value = vertices;
        }
    }
}