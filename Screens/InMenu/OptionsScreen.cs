using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokeD.CPGL.Screens.InMenu
{
    public sealed class OptionsScreen : ScreenMenu
    {
        Screen BackScreen { get; }
        Texture2D MainBackgroundTexture { get; }
        
        public OptionsScreen(PortableGame game, Screen backScreen) : base(game)
        {
            BackScreen = backScreen;

            MainBackgroundTexture = ContentFolder.TextureFolder.GetTextureFile("MainMenuBackground");

            Game.IsMouseVisible = true;

            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);
            GamePadListener.ButtonDown += (this, GamePadListener_ButtonDown);
        }
        private void KeyboardListener_KeyPressed(object sender, Components.Input.KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Escape:
                    SwitchScreen(BackScreen);
                    CloseScreen();
                    break;
            }
        }
        private void GamePadListener_ButtonDown(object sender, Components.Input.GamePadEventArgs e)
        {
            switch (e.Button)
            {
                case Buttons.B:
                    SwitchScreen(BackScreen);
                    CloseScreen();
                    break;
            }
        }

        protected override void OnResize()
        {
            base.OnResize();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            SpriteBatch.Draw(MainBackgroundTexture, ViewportAdapter.BoundingRectangle, Style.SecondaryBackgroundColor);
            //SpriteBatch.Draw(MainBackgroundTexture, Vector2.Zero, ViewportAdapter.BoundingRectangle, Style.SecondaryBackgroundColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;
                    GamePadListener.ButtonDown -= GamePadListener_ButtonDown;
                }
            }

            base.Dispose(disposing); 
        }
    }
}
