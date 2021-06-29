using Leopotam.Ecs;
using PixelSim.Components.Events;
using SFML.Window;
using System.Collections.Generic;
using PixelSim.Extensions;
using PixelSim.Services;
using SFML.System;
using SFML.Graphics;

namespace PixelSim.Systems
{
    public class InputSystem : IEcsRunSystem, IEcsInitSystem
    {
        View view = default;
        readonly EcsWorld world = default;
        readonly Window window = default;

        bool leftMouseDown = false;
        bool rightMouseDown = false;

        public void Init()
        {
            world.SendLogEvent("InputSystem initialized");
        }

        public void Run()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                world.NewEntity().Get<MouseEvent>().Value = Mouse.Button.Left;
                leftMouseDown = true;
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                world.NewEntity().Get<MouseEvent>().Value = Mouse.Button.Right;
                rightMouseDown = true;
            }

            if (leftMouseDown)
            {
                if (!Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    world.NewEntity().Get<InputReleasedEvent>();
                    leftMouseDown = false;
                }
            }

            if (rightMouseDown)
            {
                if (!Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    world.NewEntity().Get<InputReleasedEvent>();
                    rightMouseDown = false;
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Q) || Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                world.NewEntity().Get<KeyboardEvent>().Value = Keyboard.Key.Q;
            }

            if(Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                var simSize = new Vector2u(32, 32);
                var v = QuadService.CreateVertexArray(simSize.X * simSize.Y, new Vector2u(window.Size.X, window.Size.Y), new Vector2u(16, 16), Color.Black);                
                world.SendVertexUpdateEvent(v);
            }

            var speed = 4.0f;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                world.SendLogEvent("Moving Left");
                view.Move(new Vector2f(1 , 0) * speed);

            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                world.SendLogEvent("Moving Right");
                view.Move(new Vector2f(-1, 0) * speed);
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                world.SendLogEvent("Moving Up");
                view.Move(new Vector2f(0, 1) * speed);

            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                world.SendLogEvent("Moving Down");
                view.Move(new Vector2f(0, -1) * speed);
            }
        }
    }
}
