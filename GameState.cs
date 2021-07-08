using Microsoft.AspNetCore.SignalR.Client;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace PixelSim
{
    public class GameState
    {
        public CircleShape PlayerShape { get; set; } = new CircleShape(10, 10);
        public List<Vector2f> PlayerPositions { get; set; } = new List<Vector2f>();
        public bool IsOnline { get; set; } = false;
        public bool IsRendering { get; set; } = false;
    }
}
