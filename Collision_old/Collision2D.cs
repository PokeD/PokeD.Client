using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.PGL.Extensions;
using PokeD.PGL.Tiled;

namespace PokeD.PGL.Collision
{
    public class Collision2D : ICollision2D, IPosition2D
    {
        protected IPosition2D ObjectRef { get; }
        public Vector2 Position => ObjectRef.Position;

        public Color[] TextureData { get; }
        public virtual Rectangle TextureRectangle { get; }
        public int TextureScale { get; }

        public virtual Rectangle Bounds => new Rectangle((int) Position.X, (int) Position.Y, TextureRectangle.Width * TextureScale, TextureRectangle.Height * TextureScale);
        public Rectangle BoundsPixel => GetPixelRectangle(this);


        public Collision2D(IPosition2D objectRef, Texture2D texture, Rectangle textureRectangle, int textureScale = 1)
        {
            ObjectRef = objectRef;

            TextureRectangle = textureRectangle;
            TextureScale = textureScale;
            TextureData = texture.GetImageData(textureRectangle, textureScale);
        }
        public Collision2D(IPosition2D objectRef, Color[] textureData, Rectangle textureRectangle, int textureScale = 1)
        {
            ObjectRef = objectRef;

            TextureRectangle = textureRectangle;
            TextureScale = textureScale;
            TextureData = textureData;
        }


        public virtual bool IntersectBounds(ICollision2D other) => DefaultIntersectBounds(this, other);
        public virtual bool IntersectPixels(ICollision2D other) => DefaultIntersectPixels(this, other);
        public virtual bool IntersectPixelBounds(ICollision2D other) => DefaultIntersectPixelBounds(this, other);

        public virtual void DrawBorders(SpriteBatch spriteBatch)
        {
            RectangleSprite.DrawRectangle(spriteBatch, Bounds, Color.Red, 2);
            RectangleSprite.DrawRectangle(spriteBatch, BoundsPixel, Color.Yellow, 2);
        }


        protected static bool DefaultIntersectBounds(ICollision2D a, ICollision2D b) => a.Bounds.Intersects(b.Bounds);
        protected static bool DefaultIntersectPixels(ICollision2D a, ICollision2D b)
        {
            var x1 = Math.Max(a.Bounds.X, b.Bounds.X);
            var x2 = Math.Min(a.Bounds.X + a.Bounds.Width, b.Bounds.X + b.Bounds.Width);
            var y1 = Math.Max(a.Bounds.Y, b.Bounds.Y);
            var y2 = Math.Min(a.Bounds.Y + a.Bounds.Height, b.Bounds.Y + b.Bounds.Height);

            for (var y = y1; y < y2; ++y)
                for (var x = x1; x < x2; ++x)
                {
                    var ac = a.TextureData[(x - a.Bounds.X) + (y - a.Bounds.Y) * a.Bounds.Width];
                    var bc = b.TextureData[(x - b.Bounds.X) + (y - b.Bounds.Y) * b.Bounds.Width];

                    if (ac.A != 0 && bc.A != 0)
                        return true;
                }

            return false;
        }
        protected static bool DefaultIntersectPixelBounds(ICollision2D a, ICollision2D b) => GetPixelRectangle(a).Intersects(GetPixelRectangle(b));

        protected static Rectangle GetPixelRectangle(ICollision2D sprite)
        {
            int x1 = int.MaxValue, y1 = int.MaxValue;
            int x2 = int.MinValue, y2 = int.MinValue;

            for (var x = 0; x < sprite.TextureRectangle.Width * sprite.TextureScale; x++)
                for (var y = 0; y < sprite.TextureRectangle.Height * sprite.TextureScale; y++)
                    if (sprite.TextureData[x + y * sprite.TextureRectangle.Width * sprite.TextureScale].A != 0)
                    {
                        if (x1 > x) x1 = x;
                        if (x2 < x) x2 = x;

                        if (y1 > y) y1 = y;
                        if (y2 < y) y2 = y;
                    }

            return new Rectangle((int) (sprite.Position.X + x1), (int) (sprite.Position.Y + y1), x2 - x1 + 1, y2 - y1 + 1);
        }
    }

    public class Collision2DTile : Collision2D
    {
        public static bool CacheEnabled { get; set; } = true;
        private static Dictionary<uint, Color[]> ColorCache { get; } = new Dictionary<uint, Color[]>();
        private static Color[] CheckCache(TileWrapper tile, int textureScale = 1)
        {
            if (!CacheEnabled)
                return tile.Texture.GetImageData(tile.TextureRectangle, textureScale);

            if (!ColorCache.ContainsKey(tile.GID))
                ColorCache.Add(tile.GID, tile.Texture.GetImageData(tile.TextureRectangle, textureScale));

            return ColorCache[tile.GID];
        }

        public Collision2DTile(TileWrapper tile, int textureScale = 1) : base(tile, CheckCache(tile, textureScale), tile.TextureRectangle, textureScale) { }
    }
}