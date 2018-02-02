using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TmxMapperPCL;

namespace PokeD.PGL.Tiled
{
    public class LayerWrapper
    {
        public string Name { get; }

        public List<TileWrapper> Tiles { get; } = new List<TileWrapper>();


        public LayerWrapper(MapWrapper mapWrapper, Layer layer)
        {
            Name = layer.Name;

            var width = layer.Width > mapWrapper.Width ? mapWrapper.Width : layer.Width;
            var height = layer.Height > mapWrapper.Height ? mapWrapper.Height : layer.Height;

            var layerWidth = layer.Width == 0 ? mapWrapper.Width : layer.Width;

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
                    var tile = layer.Data.Tiles[x + y * layerWidth];
                    //if(tile.GID != 0)
                        Tiles.Add(new TileWrapper(mapWrapper, tile, new Vector2(x, y)));
                }
        }
    }
}