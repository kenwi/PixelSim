using Leopotam.Ecs;
using Microsoft.AspNetCore.SignalR.Client;
using PixelSim.Components.Events;
using PixelSim.Extensions;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace PixelSim.Systems
{
    public class PlayerSystem : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorld world = default;
        readonly RenderWindow window = default;
        readonly View view = default;
        readonly GameState gameState = default;

        Clock clock = new Clock();

        public void Init()
        {
            world.SendLogEvent($"IsOnline: {gameState.IsOnline}");
            view.Move(new Vector2f(-window.Size.X / 2, -window.Size.Y / 2));
        }

        public void Run()
        {
            GetInput();
            window.SetView(view);

            foreach (var position in gameState.PlayerPositions)
            {
                var size = 10;
                var shape = new CircleShape(size, 10) { Position = new Vector2f(position.X, position.Y) - new Vector2f(size / 2, size / 2) };
                window.Draw(shape);
            }
        }

        void CreateMoveRequest(float x, float y, bool isOnline)
        {
            ref var entity = ref world.NewEntity().Get<MoveRequest>();
            (entity.X, entity.Y) = (x, y);
            entity.Online = isOnline;
        }

        private void GetInput()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                window.Close();
            }

            if (clock.ElapsedTime.AsSeconds() > 1.0 / 20)
            {
                var (x, y, speed) = (0f, 0f, 10f);
                bool moved = false;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    y -= speed;
                    moved = true;
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    y += speed;
                    moved = true;
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                {
                    x -= speed;
                    moved = true;
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                {
                    x += speed;
                    moved = true;
                }

                if (moved)
                {
                    CreateMoveRequest(x, y, gameState.IsOnline);
                }
                clock.Restart();
            }
        }
    }
}
