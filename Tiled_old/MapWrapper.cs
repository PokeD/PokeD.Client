using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.PGL.Collision;
using PokeD.PGL.Components;

using TmxMapperPCL;

namespace PokeD.PGL.Tiled
{
    public class MapWrapper : MineLibComponent, ICollisionManager
    {
#if DEBUG
        private ICollision2D SelectedTile { get; set; }
#endif

        private SpriteBatch SpriteBatch { get; }

        private Camera2DComponent Camera { get; }
        private PlayerSprite Player { get; }

        public int Width { get; }
        public int Height { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        private List<TileSetWrapper> TileSetList { get; }
        private List<LayerWrapper> LayerList { get; }

        public int MapScale { get; }


        public MapWrapper(Client game, Camera2DComponent camera, Map map, int mapScale, PlayerSprite playerSprite) : base(game)
        {
            Width = map.Width;
            Height = map.Height;

            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;

            MapScale = mapScale;

            TileSetList = map.TileSets.Where(tileSet => !string.IsNullOrEmpty(tileSet.Source))
                .Select(tileSet => new TileSetWrapper(GraphicsDevice, tileSet)).OrderBy(tileSet => tileSet.FirstGID).ToList();
            LayerList = map.Layers.Select(layer => new LayerWrapper(this, layer)).ToList();


            Player = playerSprite;

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Camera = camera;
            Camera.SetCollisionManager(this);
        }

        public TileSetWrapper GetTileSetData(DataTile tile) => TileSetList.Last(tileset => tileset.FirstGID <= tile.GID);

        public bool Collides(ICollision2D collision) => LayerList.Where(layer => layer.Name == "Collisions" || layer.Name == "Water").Any(layer => Collides(layer, collision));
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

        /*
        private IEnumerable<TileWrapper> GetWalkBehindTiles(Vector2 position) => LayerList.Where(layer => layer.Name == "WalkBehind").Select(layer => GetDataTile(position, layer));
        private IEnumerable<TileWrapper> GetCollisionTiles(Vector2 position) => LayerList.Where(layer => layer.Name == "Collisions").Select(layer => GetDataTile(position, layer));
        private TileWrapper GetDataTile(Vector2 coordinate, LayerWrapper layer)
        {
            var index = (int) (coordinate.X + (coordinate.Y * Width));
            if (index < 0 || layer.Tiles.Count < index)
                return TileWrapper.Empty;

            return layer.Tiles[index];
        }
        */

        public override void Update(GameTime gameTime)
        {
            Player.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            var eye = new Vector3(0, 0, -100f);
            //var eye = new Vector3(Camera.Position.X, Camera.Position.Y, 1000f);
            var view = Matrix.CreateLookAt(eye, Vector3.Zero, Vector3.Up);
            //var projection = Camera.GetProjectionMatrix1(view);
            var projection = Matrix.CreatePerspective(MathHelper.ToRadians(45f), GraphicsDevice.Viewport.AspectRatio, 1.0f, 1000f);
            //var projection = Camera.GetProjectionMatrix1(view);
            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetInverseViewMatrix());
            DrawMap();
#if DEBUG
            SelectedTile?.DrawBorders(SpriteBatch);
            Player?.DrawBorders(SpriteBatch);
            //RectangleSprite.DrawRectangle(SpriteBatch, new Rectangle((int) Camera.Box.Min.X, (int) Camera.Box.Min.Y, (int) Camera.Box.Max.X, (int) Camera.Box.Max.Y), Color.Green, 2);
            //RectangleSprite.DrawRectangle(SpriteBatch, Camera.GetBoundingRectangle(), Color.White, 3);
#endif
            SpriteBatch.End();
        }

        private void DrawMap()
        {
            //var walkBehind = LayerList.Where(layer => layer.Name == "WalkBehind");
            //var collisions = LayerList.Where(layer => layer.Name == "Collisions");
            //var water = LayerList.Where(layer => layer.Name == "Water");
            //var walkable = LayerList.Where(layer => layer.Name == "Walkable");
            //var ledges = LayerList.Where(layer => layer.Name.Contains("Ledges"));

            var boundingRectangle = Camera.GetBoundingRectangle();

            /*
            var minVector2 = new Point(boundingRectangle.X, boundingRectangle.Y);
            var maxVector2 = new Point(boundingRectangle.X + boundingRectangle.Width, boundingRectangle.Y + boundingRectangle.Height);
            */

            ///*
            var minVector2 = new Point(
                (int) Math.Ceiling((double) (boundingRectangle.X) / (double) (TileWidth * MapScale)) - 1,
                (int) Math.Ceiling((double) (boundingRectangle.Y) / (double) (TileHeight * MapScale)) - 1);
            if (minVector2.X < 0) minVector2.X = 0;
            if (minVector2.Y < 0) minVector2.Y = 0;

            var maxVector2 = new Point(
                (int) Math.Ceiling((double) (boundingRectangle.X + boundingRectangle.Width) / (double) (TileWidth * MapScale)),
                (int) Math.Ceiling((double) (boundingRectangle.Y + boundingRectangle.Height) / (double) (TileHeight * MapScale)));
            if (maxVector2.X > Width) maxVector2.X = Width;
            if (maxVector2.Y > Height) maxVector2.Y = Height;
            //*/

            foreach (var layer in LayerList.Where(layer => layer.Name != "WalkBehind"))
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);

            Player.Draw(SpriteBatch);

            foreach (var layer in LayerList.Where(layer => layer.Name == "WalkBehind"))
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);
            

            /*
            foreach (var layer in walkable)
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);

            foreach (var layer in ledges)
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);
            
            foreach (var layer in water)
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);
            
            foreach (var layer in collisions)
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);

            Player.Draw(SpriteBatch);

            foreach (var layer in walkBehind)
                DrawLayer(SpriteBatch, minVector2, maxVector2, layer);
            */
        }
        private void DrawLayer(SpriteBatch spriteBatch, Point minPoint, Point maxPoint, LayerWrapper layer)
        {
            /*
            int minX = (int) Math.Ceiling((double) (boundingRectangle.X) / (double) (TileWidth * MapScale)) - 1;
            if (minX < 0) minX = 0;

            int minY = (int) Math.Ceiling((double) (boundingRectangle.Y) / (double) (TileHeight * MapScale)) - 1;
            if (minY < 0) minY = 0;


            int maxX = (int) Math.Ceiling((double) (boundingRectangle.X + boundingRectangle.Width) / (double) (TileWidth * MapScale));
            if (maxX > Width) maxX = Width ;

            int maxY = (int) Math.Ceiling((double) (boundingRectangle.Y + boundingRectangle.Height) / (double) (TileHeight * MapScale));
            if (maxY > Height) maxY = Height;
            */

            for (var x = minPoint.X; x < maxPoint.X; x++)
                for (var y = minPoint.Y; y < maxPoint.Y; y++)
                {
                    var tile = layer.Tiles[x + y * Width];

                    if (tile.GID != 0)
                        spriteBatch.Draw(tile.Texture, tile.Position, tile.TextureRectangle, Color.White, 0.0f, Vector2.Zero, MapScale, SpriteEffects.None, 0.0f);
                }
        }
        private void DrawLayer1(SpriteBatch spriteBatch, Point minPoint, Point maxPoint, LayerWrapper layer)
        {
            //foreach (var tile in layer.Tiles)
            //    spriteBatch.Draw(tile.Texture, tile.Position, tile.TextureRectangle, Color.White, 0.0f, Vector2.Zero, MapScale, SpriteEffects.None, 0.0f);
            //return;

            var min = minPoint.ToVector2();
            var max = maxPoint.ToVector2();

            foreach (var tile in layer.Tiles.SkipWhile(tile => tile.Position.X < min.X && tile.Position.Y < min.Y).TakeWhile(tile => tile.Position.X < max.X || tile.Position.Y < max.Y))
                spriteBatch.Draw(tile.Texture, tile.Position, tile.TextureRectangle, Color.White, 0.0f, Vector2.Zero, MapScale, SpriteEffects.None, 0.0f);
        }

        public override void Dispose()
        {

        }
    }
}