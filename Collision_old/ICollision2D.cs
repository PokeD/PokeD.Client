using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.PGL.Collision
{
    public interface ICollision2D
    {
        Vector2 Position { get; }

        Color[] TextureData { get; }
        Rectangle TextureRectangle { get; }
        int TextureScale { get; }

        Rectangle Bounds { get; }
        Rectangle BoundsPixel { get; }


        bool IntersectBounds(ICollision2D other);
        bool IntersectPixels(ICollision2D other);
        bool IntersectPixelBounds(ICollision2D other);

        void DrawBorders(SpriteBatch spriteBatch);
    }
}