using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TMXParserPCL;

namespace PokeD.CPGL.Tiled
{
    public class LayerWrapper
    {
        public string Name { get; }

        public List<TileWrapper> Tiles { get; } = new List<TileWrapper>();


        public LayerWrapper(MapWrapper mapWrapper, Layer layer)
        {
            Name = layer.Name;

            var width = layer.Width > mapWrapper.WidthInTiles ? mapWrapper.WidthInTiles : layer.Width;
            var height = layer.Height > mapWrapper.HeightInTiles ? mapWrapper.HeightInTiles : layer.Height;

            var layerWidth = layer.Width == 0 ? mapWrapper.WidthInTiles : layer.Width;

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    Tiles.Add(new TileWrapper(mapWrapper, layer.Data.Tiles[x + y * layerWidth], new Vector2(x, y)));
        }
    }
}