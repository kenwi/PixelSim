using System;
using Leopotam.Ecs;
using PixelSim.Components.Events;
using SFML.Window;
using PixelSim.Extensions;

namespace PixelSim.Systems
{
    public class InputProcessingSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
    {
        readonly EcsWorld world = default;
        readonly Window window = default;
        readonly EcsFilter<InputReleasedEvent> eventFilter = default;
        readonly EcsFilter<MouseEvent> mouseEvent = default;
        readonly EcsFilter<KeyboardEvent> keyboardEvent = default;

        public void Init()
        {
            world.SendLogEvent("InputProcessingSystem initialized");
        }

        public void Run()
        {
            if (!eventFilter.IsEmpty())
            {
                world.SendLogEvent("Caught EventReleased " + eventFilter.GetEntitiesCount());
            }

            if (!mouseEvent.IsEmpty())
            {
                foreach (var index in mouseEvent)
                {
                    var input = mouseEvent.Get1(index).Value;
                    world.SendLogEvent("Mouse " + input + " Button down " + mouseEvent.GetEntitiesCount());
                }
            }

            if (!keyboardEvent.IsEmpty())
            {
                foreach (var index in keyboardEvent)
                {
                    var input = keyboardEvent.Get1(index).Value;
                    world.SendLogEvent("Keyboard " + input + " Button down " + keyboardEvent.GetEntitiesCount());

                    if (input == Keyboard.Key.Q)
                    {
                        window.Close();
                    }
                }
            }
        }

        public void Destroy()
        {
            eventFilter.Destroy();
            mouseEvent.Destroy();
            keyboardEvent.Destroy();
        }
    }
}
