using System;
using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.Button
{
    public sealed class ButtonMenu : GUIButton
    {
        public ButtonMenu(Screen screen, string text, Rectangle pos, EventHandler action, Color textureColor) : base(screen, text, action, textureColor)
        {
            ButtonRectangle = pos;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            if (IsNonPressable)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangle, ButtonUnavailablePosition, TextureColor);
                TextRenderer.DrawTextCentered(SpriteBatch, ButtonText, ButtonRectangle, ButtonUnavailableColor);
            }

            if (IsSelected || IsSelectedMouseHover)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangle, ButtonPressedPosition, TextureColor);
                TextRenderer.DrawTextCentered(SpriteBatch, ButtonText, ButtonRectangleShadow, ButtonPressedShadowColor);
                TextRenderer.DrawTextCentered(SpriteBatch, ButtonText, ButtonRectangle, ButtonPressedColor);
            }

            if (IsActive)
            {
                SpriteBatch.Draw(WidgetsTexture, ButtonRectangle, ButtonPosition, TextureColor);
                TextRenderer.DrawTextCentered(SpriteBatch, ButtonText, ButtonRectangleShadow, ButtonShadowColor);
                TextRenderer.DrawTextCentered(SpriteBatch, ButtonText, ButtonRectangle, ButtonColor);
            }

            //SpriteBatch.End();
        }
    }
}
