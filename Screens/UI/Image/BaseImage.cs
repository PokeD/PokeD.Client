using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.Image
{
    public class BaseImage : GUIImage
    {
        public BaseImage(Screen screen, Rectangle backgroundRectangle, Texture2D backgroundTexture) : base(screen, backgroundRectangle, backgroundTexture) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            SpriteBatch.Draw(Texture, TextureRectangle, null, Color.White);
            //SpriteBatch.End();
        }
    }
}