using System;
using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.Box
{
    public sealed class BoxPlayerInfo : GUIBox
    {
        public BoxPlayerInfo(Screen screen, Rectangle rect, Color style) : base(screen, rect, null, style) { }
        protected override void OnButtonPressed(object sender, EventArgs eventArgs) { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
