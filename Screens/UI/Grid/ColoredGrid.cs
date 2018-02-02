using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.Grid
{
    public class ColoredGrid : GUIGrid
    {
        public ColoredGrid(Screen screen, Rectangle backgroundRectangle, Color gridColor) : base(screen)
        {
            BackgroundRectangle = backgroundRectangle;

            FrameTopRectangle = new Rectangle(BackgroundRectangle.X, BackgroundRectangle.Y, BackgroundRectangle.Width, FrameSize.Y);
            FrameBottomRectangle = new Rectangle(BackgroundRectangle.X, BackgroundRectangle.Y + BackgroundRectangle.Height - FrameSize.Y, BackgroundRectangle.Width, FrameSize.Y);
            FrameLeftRectangle = new Rectangle(BackgroundRectangle.X, BackgroundRectangle.Y, FrameSize.X, BackgroundRectangle.Height);
            FrameRightRectangle = new Rectangle(BackgroundRectangle.X + BackgroundRectangle.Width - FrameSize.X, BackgroundRectangle.Y, FrameSize.X, BackgroundRectangle.Height);

            BackgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            BackgroundTexture.SetData(new[] { gridColor });

            FrameTexture = new Texture2D(GraphicsDevice, 1, 1);
            FrameTexture.SetData(new[] { new Color(0, 0, 0, 255) });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            SpriteBatch.Draw(BackgroundTexture, BackgroundRectangle, Rectangle.Empty, Color.White);

            SpriteBatch.Draw(FrameTexture, FrameTopRectangle, new Rectangle(0, 0, BackgroundRectangle.X, FrameSize.Y), Color.White);
            SpriteBatch.Draw(FrameTexture, FrameBottomRectangle, new Rectangle(0, 0, BackgroundRectangle.X, FrameSize.Y), Color.White);
            SpriteBatch.Draw(FrameTexture, FrameLeftRectangle, new Rectangle(0, 0, BackgroundRectangle.Y, FrameSize.X), Color.White);
            SpriteBatch.Draw(FrameTexture, FrameRightRectangle, new Rectangle(0, 0, BackgroundRectangle.Y, FrameSize.X), Color.White);

            //SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    BackgroundTexture?.Dispose();
                    FrameTexture?.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
