using Leopotam.Ecs;
using PixelSim.Components.Events;
using System;
using Spectre.Console;
using PixelSim.Extensions;

namespace PixelSim.Systems
{
    public class LoggingSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsWorld world = default;
        readonly EcsFilter<LogEvent> logMessages = default;

        public void Init()
        {
            world.SendLogEvent("LoggingSystem initialized");
        }

        public void Run()
        {
            if (!logMessages.IsEmpty())
            {
                foreach (var index in logMessages)
                {
                    var message = logMessages.Get1(index).Value;
                    AnsiConsole.MarkupLine($"[gray][[{DateTime.Now.ToString()}]][/] {message}");
                }
            }
        }
    }
}
