using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.Text
{
    public sealed class BaseCenteredText : GUIText
    {
        public BaseCenteredText(Screen screen, string text, Rectangle textRect, Color textColor) : base(screen, text, textRect, textColor) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            TextRenderer.DrawTextCentered(SpriteBatch, Text, TextRectangle, TextColor);
            //SpriteBatch.End();
        }
    }
}
