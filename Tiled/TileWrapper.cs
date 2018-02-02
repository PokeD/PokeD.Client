using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.CPGL.Physics.Collision;

using TMXParserPCL;

namespace PokeD.CPGL.Tiled
{
    public class TileWrapper : IPosition2D
    {
        private TileSetWrapper TileSet { get; }

        public uint GID { get; }
        public Vector2 Position { get; }
        public Texture2D Texture => TileSet.Texture;
        public Rectangle TextureRectangle { get; }

        private TileWrapper(uint gid, Vector2 position) { GID = gid; Position = position; }
        public TileWrapper(MapWrapper mapWrapper, DataTile tile, Vector2 layerPosition) : this(tile.GID, new Vector2(layerPosition.X * mapWrapper.TileWidth, layerPosition.Y * mapWrapper.TileHeight) * mapWrapper.MapScale)
        {
            if(GID == 0)
                return;

            TileSet = mapWrapper.GetTileSetData(tile);

            var startGID = (int) tile.GID - TileSet.FirstGID;
            var tx = TileSet.Texture.Width / mapWrapper.TileWidth;
            var texX = startGID % tx;
            var texY = startGID / tx;
            TextureRectangle = new Rectangle(texX * mapWrapper.TileWidth, texY * mapWrapper.TileHeight, mapWrapper.TileWidth, mapWrapper.TileHeight);
        }
    }
}