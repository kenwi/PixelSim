using Leopotam.Ecs;
using PixelSim.Components.Events;
using SFML.System;
using PixelSim.Extensions;
using PixelSim.Components;

namespace PixelSim.Systems
{
    public class FpsCounterSystem : IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsWorld world = default;
        readonly EcsFilter<FpsCounterEvent> fpsEvent = default;

        readonly EcsFilter<FpsCounterComponent> fpsComponent = default;
        FpsCounterComponent component;
        Clock frameTimer = new Clock();

        public void Init()
        {
            world.SendLogEvent("FpsCounterSystem initialized");
        }

        public void Run()
        {
            if (fpsComponent.IsEmpty())
            {
                ref FpsCounterComponent component = ref world.NewEntity().Get<FpsCounterComponent>();
                world.SendLogEvent("Created component " + component.GetType());
            }

            component.frameCount++;
            if (frameTimer.ElapsedTime.AsSeconds() > 2)
            {
                var fps = component.frameCount / frameTimer.Restart().AsSeconds();
                SendFpsEvent(fps);

                component.totalFrames += component.frameCount;
                component.frameCount = 0;
                frameTimer.Restart();
                world.SendLogEvent("Timer restart");
            }

            if (!fpsEvent.IsEmpty())
            {
                foreach (var index in fpsEvent)
                {
                    ref var fps = ref fpsEvent.Get1(index).Value;
                    world.SendLogEvent("Fps: " + fps);
                }
            }
        }

        void SendFpsEvent(float fps) => world.NewEntity().Get<FpsCounterEvent>().Value = fps;
    }
}
