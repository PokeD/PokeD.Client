using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.CPGL.Components;
using PokeD.CPGL.Components.Debug;
using PokeD.CPGL.Data;
using PokeD.CPGL.Physics.Collision;

using TMXParserPCL;

namespace PokeD.CPGL.Tiled
{
    public class MapWrapper : DrawableComponent, ICollisionManager, ICameraBorders, IPosition2DConverter
    {
#if DEBUG
        private ICollision2D SelectedTile { get; set; }
#endif

        private SpriteBatch SpriteBatch { get; set; }
        private PlayerSprite Player { get; set; }

        public int WidthInTiles { get; }
        public int HeightInTiles { get; }

        public int TileWidth { get; }
        public int TileHeight { get; }

        private List<TileSetWrapper> TileSetList { get; set; }
        private List<BaseLayerRenderer> LayerRenderers { get; set; }

        public int MapScale { get; }

        public MapWrapper(PortableGame game, Map map, int mapScale, PlayerSprite playerSprite) : base(game)
        {
            WidthInTiles = map.Width;
            HeightInTiles = map.Height;

            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;

            MapScale = mapScale;

            Player = playerSprite;

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            Camera.SetCollisionManager(this);
            Camera.SetCameraBorders(this);
            Camera.SetPositionConverter(this);

            //TileSetList = map.TileSets.Where(tileSet => !string.IsNullOrEmpty(tileSet.Source))
            //    .Select(tileSet => new TileSetWrapper(GraphicsDevice, tileSet)).OrderBy(tileSet => tileSet.FirstGID).ToList();
            TileSetList = map.TileSets.Select(tileSet => new TileSetWrapper(GraphicsDevice, tileSet)).OrderBy(tileSet => tileSet.FirstGID).ToList();
            LayerRenderers = map.Layers.Select(layer => new LayerChunkedRenderer(this, new LayerWrapper(this, layer), SpriteBatch)).ToList<BaseLayerRenderer>();
        }

        public BoundingBox ObjectBoundingBox => new BoundingBox(
            new Vector3(new Vector2(0, 0), 0.0f) * MapScale,
            new Vector3(new Vector2(WidthInTiles * TileWidth, HeightInTiles * TileHeight), 0.0f) * MapScale);

        //public Func<Vector2, Vector2> Position2DConverterFunc => (camPosition) => camPosition / (new Vector2(TileWidth, TileHeight) * MapScale);
        public Func<Vector2, Vector2> Position2DConverterFunc => (camPosition) => camPosition * MapScale;

        public bool Collides(ICollision2D collision) => LayerRenderers.Where(layer => layer.Name == "Collisions" || layer.Name == "Water").Any(layer => Collides(layer.Layer, collision));
        private bool Collides(LayerWrapper collisionLayer, ICollision2D collision) => collisionLayer.Tiles.Any(tile => Collides(tile, collision));
        private bool Collides(TileWrapper tile, ICollision2D collision)
        {
            if (tile.GID == 0)
                return false;

            var rect = new Rectangle((int) tile.Position.X, (int) tile.Position.Y, TileWidth * MapScale, TileHeight * MapScale);
            if (collision.Bounds.Intersects(rect))
            {
                //var pX = (float) Math.Ceiling(collision.Position.X / (Map.TileWidth * MapScale));
                //var pY = (float) Math.Ceiling(collision.Position.Y / (Map.TileWidth * MapScale));
                //var t1 = GetCollisionTiles(new Vector2(tile.X, tile.Y)).Any(t => t.GID != 0);
                //var t2 = GetCollisionTiles(new Vector2(tile.X, tile.Y - 1)).Any(t => t.GID != 0);
                //var t3 = GetCollisionTiles(new Vector2(tile.X, tile.Y + 1)).Any(t => t.GID != 0);
                //
                //var t4 = GetWalkBehindTiles(new Vector2(tile.X, tile.Y)).Any(t => t.GID != 0);
                //var t5 = GetWalkBehindTiles(new Vector2(tile.X, tile.Y - 1)).Any(t => t.GID != 0);
                //var t6 = GetWalkBehindTiles(new Vector2(tile.X, tile.Y + 1)).Any(t => t.GID != 0);
                //
                //if (t1 && t6)
                //    return true;

#if DEBUG
                SelectedTile = new Collision2DTile(tile, MapScale);
                return collision.IntersectPixelBounds(SelectedTile);
#else
                //return player.IntersectPixels(new Collision2DTile(new TileWrapper(tile.GID, tilePosition), texture, texRect, MapScale));
                return collision.IntersectPixelBounds(new Collision2DTile(tile, MapScale));
#endif
            }


            return false;
        }

        public TileSetWrapper GetTileSetData(DataTile tile) => TileSetList.Last(tileset => tileset.FirstGID <= tile.GID);

        /*
        private IEnumerable<TileWrapper> GetWalkBehindTiles(Vector2 position) => LayerRenderers.Where(layer => layer.Name == "WalkBehind").Select(layer => GetDataTile(position, layer.Layer));
        private IEnumerable<TileWrapper> GetCollisionTiles(Vector2 position) => LayerRenderers.Where(layer => layer.Name == "Collisions").Select(layer => GetDataTile(position, layer.Layer));
        private TileWrapper GetDataTile(Vector2 coordinate, LayerWrapper layer)
        {
            var index = (int) (coordinate.X + (coordinate.Y * WidthInTiles));
            if (index < 0 || layer.Tiles.Count < index)
                return TileWrapper.Empty;

            return layer.Tiles[index];
        }
        */

        public override void Update(GameTime gameTime)
        {
            Player.Update(gameTime);

#if DEBUG
            DebugComponent.PlayerPos = Player.Position / (new Vector2(TileWidth, TileHeight) * MapScale);
#endif
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());

            foreach (var layerRenderer in LayerRenderers.Where(layer => layer.Name != "WalkBehind"))
                layerRenderer.Draw(gameTime);

            Player.Draw(gameTime, SpriteBatch);

            foreach (var layerRenderer in LayerRenderers.Where(layer => layer.Name == "WalkBehind"))
                layerRenderer.Draw(gameTime);
            
#if DEBUG
            //SelectedTile?.DrawBorders(gameTime, SpriteBatch);
            //Player?.DrawBorders(gameTime, SpriteBatch);

            //var cameraBox = Camera.CameraBorders.ObjectBoundingBox;
            //RectangleSprite.DrawRectangle(SpriteBatch, new Rectangle((int) cameraBox.Min.X, (int) cameraBox.Min.Y, (int) cameraBox.Max.X, (int) cameraBox.Max.Y), Color.Green, 4);
            //RectangleSprite.DrawRectangle(SpriteBatch, Camera.GetBoundingRectangle(), Color.White, 3);
#endif
            SpriteBatch.End();
        }


        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    SpriteBatch.Dispose();
                    SpriteBatch = null;

                    foreach (var tileSet in TileSetList)
                        tileSet?.Dispose();
                    TileSetList = null;

                    foreach (var layerRenderer in LayerRenderers)
                        layerRenderer?.Dispose();
                    LayerRenderers = null;

                    Player.Dispose();
                    Player = null;
                }
            }


            base.Dispose(disposing);
        }
    }
}