using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.Text
{
    public sealed class BaseShadowText : GUIText
    {
        private Color TextShadowColor { get; }
        
        private Rectangle TextShadowRectangle { get; }


        public BaseShadowText(Screen screen, string text, Rectangle textRect, Color textColor, Color shadowColor) : base(screen, text, textRect, textColor)
        {
            TextShadowColor = shadowColor;

            TextShadowRectangle = new Rectangle(TextRectangle.X + 1, TextRectangle.Y + 1, TextRectangle.Width, TextRectangle.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            TextRenderer.DrawText(SpriteBatch, Text, TextShadowRectangle, TextShadowColor);
            TextRenderer.DrawText(SpriteBatch, Text, TextRectangle, TextColor);
            //SpriteBatch.End();
        }
    }
}
