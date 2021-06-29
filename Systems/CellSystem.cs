using Leopotam.Ecs;
using PixelSim.Components;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using PixelSim.Extensions;
using System.Threading.Tasks;

namespace PixelSim.Systems
{
    public class CellSystem : IEcsInitSystem
    {
        readonly EcsWorld world = default;
        readonly Window window = default;
        readonly uint SizeX = 32;
        readonly uint SizeY = 32;

        Cell[] cells;

        public void Init()
        {            
            // initializeParallel();
            initializeSequential();
            world.SendLogEvent("CellSystem initialized");
        }

        private void initializeParallel()
        {
            var simSize = new Vector2u(SizeX, SizeY);
            var cellSize = new Vector2f(window.Size.X / simSize.X, window.Size.Y / simSize.Y);
            cells = new Cell[simSize.X * simSize.Y];

            var index = 0;
            Parallel.ForEach(cells, (c) =>
            {
                var (x, y) = (index % (int)simSize.X, index / (int)simSize.Y);
                (c.Index, c.X, c.Y, c.Color) = (index, x, y, Color.White);
                index++;
            });
        }

        private void initializeSequential()
        {
            var simSize = new Vector2u(SizeX, SizeY);
            var cellSize = new Vector2f(window.Size.X / simSize.X, window.Size.Y / simSize.Y);
            cells = new Cell[simSize.X * simSize.Y];

            int index = 0;
            for (int y = 0; y < SizeY; y++)
            {
                for (int x = 0; x < SizeX; x++)
                {
                    var (pixelX, pixelY) = ((int)x * cellSize.X, (int)y * cellSize.Y);
                    ref var cell = ref cells[index];
                    (cell.Index, cell.X, cell.Y, cell.Color) = (index++, x, y, Color.White);
                }
            }
        }
    }
}
