using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Tiled
{
    public class LayerDirectRenderer : BaseLayerRenderer
    {
        public LayerDirectRenderer(MapWrapper map, LayerWrapper layer, SpriteBatch spriteBatch) : base(map, layer, spriteBatch) { }

        public override void Draw(GameTime gameTime)
        {
            var boundingRectangle = Camera.BoundingRectangle;
            
            /*
            var minVector2 = new Point(boundingRectangle.X, boundingRectangle.Y);
            var maxVector2 = new Point(boundingRectangle.X + boundingRectangle.Width, boundingRectangle.Y + boundingRectangle.Height);
            */

            ///*
            var minVector2 = new Point(
                (int)Math.Ceiling((double)(boundingRectangle.X) / (double)(Map.TileWidth * Map.MapScale)) - 1,
                (int)Math.Ceiling((double)(boundingRectangle.Y) / (double)(Map.TileHeight * Map.MapScale)) - 1);
            if (minVector2.X < 0) minVector2.X = 0;
            if (minVector2.Y < 0) minVector2.Y = 0;

            var maxVector2 = new Point(
                (int)Math.Ceiling((double)(boundingRectangle.X + boundingRectangle.Width) / (double)(Map.TileWidth * Map.MapScale)),
                (int)Math.Ceiling((double)(boundingRectangle.Y + boundingRectangle.Height) / (double)(Map.TileHeight * Map.MapScale)));
            if (maxVector2.X > Map.WidthInTiles) maxVector2.X = Map.WidthInTiles;
            if (maxVector2.Y > Map.HeightInTiles) maxVector2.Y = Map.HeightInTiles;
            //*/

            DrawLayer(SpriteBatch, minVector2, maxVector2, Layer);
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
                var tile = layer.Tiles[x + y * Map.WidthInTiles];

                if (tile.GID != 0)
                    spriteBatch.Draw(tile.Texture, tile.Position, tile.TextureRectangle, Color.White, 0.0f, Vector2.Zero, Map.MapScale, SpriteEffects.None, 0.0f);
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
                spriteBatch.Draw(tile.Texture, tile.Position, tile.TextureRectangle, Color.White, 0.0f, Vector2.Zero, Map.MapScale, SpriteEffects.None, 0.0f);
        }

        public override void Update(GameTime gameTime) { }
    }
}