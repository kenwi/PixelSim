using SFML.System;
using SFML.Graphics;

namespace PixelSim.Services
{

    public class QuadService
    {
        public static Vertex[] CreateVertexArray(uint count, Vector2u simSize, Vector2u cellSize, Color color)
        {
            uint vertexCount = 0;
            Vertex[] vertices = new Vertex[simSize.X * simSize.Y * 4];
            for (int x = 0; x < simSize.X; x++)
            {
                for (int y = 0; y < simSize.Y; y++)
                {
                    var (pixelX, pixelY) = (x * cellSize.X, y * cellSize.Y);
                    vertices[vertexCount] = new Vertex(new Vector2f(pixelX, pixelY), color);
                    vertices[vertexCount + 3] = new Vertex(new Vector2f(pixelX + cellSize.X, pixelY), color);
                    vertices[vertexCount + 1] = new Vertex(new Vector2f(pixelX, pixelY + cellSize.Y), color);
                    vertices[vertexCount + 2] = new Vertex(new Vector2f(pixelX + cellSize.X, pixelY + cellSize.Y), color);
                    vertexCount += 4;
                }
            }
            return vertices;
        }
    }
}
