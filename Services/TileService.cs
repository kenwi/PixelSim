using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using TiledSharp;

namespace PixelSim.Services
{
    public class TileService
    {
        internal TmxMap map;
        internal Clock clock = new Clock();
        internal Dictionary<string, Texture> tilesheets = new Dictionary<string, Texture>();
        private Dictionary<TmxLayer, RenderLayer<TmxLayer>> layers = new Dictionary<TmxLayer, RenderLayer<TmxLayer>>();
        //private Dictionary<TmxObjectGroup, RenderLayer<TmxObjectGroup>> groups = new Dictionary<TmxObjectGroup, RenderLayer<TmxObjectGroup>>();

        public TileService(TmxMap map)
        {
            this.map = map;
            map.Tilesets.ToList().ForEach(t => tilesheets.Add(t.Image.Source, new Texture(t.Image.Source)));
        }

        public void UpdateAnimations()
        {
            foreach (var layer in map.Layers)
            {
                if (!layers.ContainsKey(layer))
                {
                    layers.Add(layer, new RenderLayer<TmxLayer>(this, layer.Name));
                    layers[layer].Build();
                }
                layers[layer].UpdateAnimations();
            }
        }

        public void DrawLayer<T>(RenderTarget target, string layerName)
        {
            var layer = map.Layers[layerName];
            if (!layers.ContainsKey(layer))
            {
                layers.Add(layer, new RenderLayer<TmxLayer>(this, layer.Name));
                layers[layer].Build();
            }
            layers[layer].Draw(target);
        }

        public void Draw(RenderTarget target)
        {
            map.Layers.ToList().ForEach(l => DrawLayer<TmxLayer>(target, l.Name));
            var enemies = map.ObjectGroups["Enemies"].Objects;
            var skeleton = enemies.Single(e => e.Type == "Undead");

            var tile = skeleton.Tile;
            var layer = map.ObjectGroups["Enemies"];
            TmxTileset tileset = null;
            foreach (var ts in map.Tilesets)
            {
                if (tile.Gid >= ts.FirstGid && tile.Gid < ts.FirstGid + ts.TileCount)
                {
                    tileset = ts;
                    break;
                }
            }

            Vertex tl = new Vertex(new Vector2f((float)(layer?.OffsetX ?? 0.0) + tile.X * tileset.TileWidth, (float)(layer?.OffsetY ?? 0) + tile.Y * tileset.TileHeight));
            Vertex bl = new Vertex(new Vector2f(tl.Position.X, tl.Position.Y + tileset.TileHeight));
            Vertex br = new Vertex(new Vector2f(tl.Position.X + tileset.TileWidth, tl.Position.Y + tileset.TileHeight));
            Vertex tr = new Vertex(new Vector2f(tl.Position.X + tileset.TileWidth, tl.Position.Y));

            // Find the tile in the tileset
            int lid = tile.Gid - tileset.FirstGid;
            // TmxTilesetTile tsTile = null;
            foreach (var tsTileCheck in tileset.Tiles)
            {
                //if (tsTileCheck.Id == lid)
                {
                    //tsTile = tsTileCheck;
                    break;
                }
            }
        }
    }
}
