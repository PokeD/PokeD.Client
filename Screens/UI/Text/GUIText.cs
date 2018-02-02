using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.Text
{
    public abstract class GUIText : GUIItem
    {
        protected string Text { get; }
        protected Color TextColor { get; }

        protected Rectangle TextRectangle { get; }


        protected GUIText(Screen screen, string text, Rectangle textRect, Color textColor) : base(screen, false)
        {
            Text = text;
            TextColor = textColor;
            TextRectangle = textRect;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
