using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.Image
{
    public abstract class GUIImage : GUIItem
    {
        protected Texture2D Texture { get; }
        protected Rectangle TextureRectangle { get; }


        protected GUIImage(Screen screen, Rectangle rectangle, Texture2D texture) : base(screen, false)
        {
            TextureRectangle = rectangle;
            Texture = texture;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Texture?.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}