using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.PokeD
{
    public class ScaledGUIItem<T> : ScreenUI where T : GUIItem
    {
        private T GUIItem { get; }
        private Rectangle Rectangle { get; }
        private RenderTarget2D RenderTarget { get; }
        private float Scale { get; }


        public ScaledGUIItem(Screen screen, T guiItem, Rectangle rectangle, float scale) : base(screen)
        {
            GUIItem = guiItem;
            Rectangle = rectangle;
            Scale = scale;

            //var rec = new Rectangle(GUIItem.DrawSpace.X, GUIItem.DrawSpace.Y, GUIItem.DrawSpace.Width * 2, GUIItem.DrawSpace.Height * 2);
            //RenderTarget = new RenderTarget2D(Game.GraphicsDevice, rec.Width, rec.Height);
        }

        public override void Update(GameTime gameTime) { GUIItem.Update(gameTime); }
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GUIItem.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            //var rec = new Rectangle(GUIItem.DrawSpace.X, GUIItem.DrawSpace.Y, GUIItem.DrawSpace.Width * 2, GUIItem.DrawSpace.Height * 2);
            //SpriteBatch.Draw(RenderTarget, rec, GUIItem.DrawSpace, Color.White);
            //SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            GUIItem.Dispose();

            base.Dispose(disposing);
        }
    }
}