using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.CPGL.Components;

namespace PokeD.CPGL.Tiled
{
    public class LayerChunkedRenderer : BaseLayerRenderer
    {
        private class LayerChunk : DrawableComponent
        {
            public const int TileCount = 24;

            private MapWrapper Map { get; }
            private LayerWrapper Layer { get; }
            private SpriteBatch SpriteBatch { get; }
            private SpriteBatch ScalingSpriteBatch { get; }
            private RenderTarget2D RenderTarget { get; }

            private Point MinTilePoint { get; }
            private Point MaxTilePoint { get; }
            public Vector2 DrawPosition => MinTilePoint.ToVector2() * new Vector2(Map.TileWidth, Map.TileHeight) * Map.MapScale;
            public Vector2 DrawSize => new Vector2(TileCount) * new Vector2(Map.TileWidth, Map.TileHeight) * Map.MapScale;
            public Rectangle DrawRectangle => new Rectangle((int) DrawPosition.X, (int) DrawPosition.Y, (int) DrawSize.X, (int) DrawSize.Y);

  
            public LayerChunk(MapWrapper map, LayerWrapper layer, Point minTilePoint, SpriteBatch spriteBatch) : base(map)
            {
                Map = map;
                Layer = layer;
                MinTilePoint = minTilePoint;
                MaxTilePoint = MinTilePoint + new Point(TileCount, TileCount);
                SpriteBatch = spriteBatch;
                ScalingSpriteBatch = new SpriteBatch(GraphicsDevice);
                RenderTarget = new RenderTarget2D(GraphicsDevice, TileCount * Map.TileWidth, TileCount * Map.TileHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);

                Generate();
            }
            private void Generate()
            {
                GraphicsDevice.SetRenderTarget(RenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                
                using (var spriteBatch = new SpriteBatch(GraphicsDevice))
                {
                    spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                    for (int x = MinTilePoint.X, pX = 0; x < MaxTilePoint.X; x++, pX += Map.TileWidth)
                    for (int y = MinTilePoint.Y, pY = 0; y < MaxTilePoint.Y; y++, pY += Map.TileHeight)
                    {
                        var index = x + y * Map.WidthInTiles;
                        if (index < Layer.Tiles.Count)
                        {
                            var tile = Layer.Tiles[index];
                            var drawPosition = new Vector2(pX, pY);

                            if (tile.GID != 0)
                                spriteBatch.Draw(tile.Texture, drawPosition, tile.TextureRectangle, Color.White);
                        }
                    }
                    spriteBatch.End();
                }

                GraphicsDevice.SetRenderTarget(null);
            }

            public override void Draw(GameTime gameTime)
            {
                /*
                GraphicsDevice.SetRenderTarget(ScaledRenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                effect.TextureSize = new Vector2(RenderTarget.Width, RenderTarget.Height);
                ScalingSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, effect, null);
                ScalingSpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, ScaledRenderTarget.Width, ScaledRenderTarget.Height), Color.White);
                ScalingSpriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);
                */


                SpriteBatch.Draw(RenderTarget, DrawPosition, RenderTarget.Bounds, Color.White, 0.0f, Vector2.Zero, Map.MapScale, SpriteEffects.None, 0.0f);
            }

            public override void Update(GameTime gameTime) { }

            protected override void Dispose(bool disposing)
            {
                RenderTarget?.Dispose();

                base.Dispose(disposing);
            }
        }

        private IList<LayerChunk> Chunks { get; } = new List<LayerChunk>();

        public LayerChunkedRenderer(MapWrapper map, LayerWrapper layer, SpriteBatch spriteBatch) : base(map, layer, spriteBatch)
        {
            Generate();
        }
        private void Generate()
        {
            var xMax = (int) Math.Ceiling((double) (Map.WidthInTiles / (double) LayerChunk.TileCount));
            var yMax = (int) Math.Ceiling((double) (Map.HeightInTiles / (double) LayerChunk.TileCount));
            for (var x = 0; x < xMax; x++)
            for (var y = 0; y < yMax; y++)
            {
                Chunks.Add(new LayerChunk(Map, Layer, new Point(x * LayerChunk.TileCount, y * LayerChunk.TileCount), SpriteBatch));
            }
        }
        

        public override void Draw(GameTime gameTime)
        {
            var boundingRectangle = Camera.BoundingRectangle;
            foreach (var chunk in Chunks.Where(chunk => chunk.DrawRectangle.Intersects(boundingRectangle)))
                chunk.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var chunk in Chunks)
                chunk?.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var chunk in Chunks)
                chunk?.Dispose();

            base.Dispose(disposing);
        }
    }
}