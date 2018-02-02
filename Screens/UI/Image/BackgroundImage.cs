using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.Image
{
    public class BackgroundImage : BaseImage
    {
        private static Texture2D GetTexture(GraphicsDevice graphicsDevice, Color color)
        {
            var texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { color });
            return texture;
        }

        public BackgroundImage(Screen screen, Rectangle backgroundRectangle, Color backgroundColor) : base(screen, backgroundRectangle, GetTexture(screen.Game.GraphicsDevice, backgroundColor)) { }
    }
}