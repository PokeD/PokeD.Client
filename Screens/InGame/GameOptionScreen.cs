using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeD.CPGL.Screens.InMenu;
using PokeD.CPGL.Screens.UI.Button;

namespace PokeD.CPGL.Screens.InGame
{
    public sealed class GameOptionScreen : Screen
    {
        PlayerScreen GameScreen { get; }
        Texture2D MainBackgroundTexture { get; }

        public GameOptionScreen(PortableGame game, PlayerScreen gameScreen) : base(game)
        {
            GameScreen = gameScreen;

            MainBackgroundTexture = new Texture2D(GraphicsDevice, 1, 1);
            MainBackgroundTexture.SetData(new[] { new Color(0, 0, 0, 170) });

            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);

            Game.IsMouseVisible = true;
            OnResize();
        }

        private void KeyboardListener_KeyPressed(object sender, Components.Input.KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.E:
                    Game.IsMouseVisible = false;

                    //CloseScreen();
                    SwitchScreen(GameScreen);
                    /*
                    if (IsActive)
                    {
                        Game.IsMouseVisible = false;

                        ToHidden();
                        GameScreen.ToActive();
                    }
                    */
                    break;
            }
        }

        private void OnReturnToGameButtonPressed(object sender, EventArgs eventArgs)
        {
            Game.IsMouseVisible = false;

            //CloseScreen();
            SwitchScreen(GameScreen);

            /*
            ToHidden();
            GameScreen.ToActive();
            */
        }
        private void OnOptionButtonPressed(object sender, EventArgs eventArgs)
        {
            //SetAtTopScreen(new OptionsScreen(Game, this));
        }
        private void OnSaveButtonPressed(object sender, EventArgs eventArgs)
        {
        }

        protected override void OnResize()
        {
            base.OnResize();

            var xOffset = 0;
            var yOffset = 0;

            var returnToGameRectangle = new Rectangle(
                xOffset = ViewportAdapter.Center.X - Style.ButtonSize.Center.X,
                yOffset += ViewportAdapter.Center.Y - (Style.ButtonSize.Center.Y + 15) * 3,
                Style.ButtonSize.Width, Style.ButtonSize.Height);
            var ReturnToGameButton = new ButtonMenu(this, "Return to Game", returnToGameRectangle, OnReturnToGameButtonPressed, Style.SecondaryBackgroundColor);

            var optionsRectangle = new Rectangle(
                xOffset = ViewportAdapter.Center.X - Style.ButtonSize.Center.X,
                yOffset += -Style.ButtonSize.Center.Y - (Style.ButtonSize.Center.Y + 15) * 2,
                Style.ButtonSize.Width, Style.ButtonSize.Height);
            var OptionsButton = new ButtonMenu(this, "Options", optionsRectangle, OnOptionButtonPressed, Style.SecondaryBackgroundColor);

            var saveRectangle = new Rectangle(
                xOffset = ViewportAdapter.Center.X - Style.ButtonSize.Center.X,
                yOffset += -Style.ButtonSize.Center.Y - (Style.ButtonSize.Center.Y + 15) * 1,
                Style.ButtonSize.Width, Style.ButtonSize.Height);
            var SaveButton = new ButtonMenu(this, "Save", saveRectangle, OnSaveButtonPressed, Style.SecondaryBackgroundColor);

            AddGUIItem(ReturnToGameButton);
            AddGUIItem(OptionsButton);
            AddGUIItem(SaveButton);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            SpriteBatch.Draw(MainBackgroundTexture, ViewportAdapter.BoundingRectangle, Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            MainBackgroundTexture?.Dispose();

            KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;

            base.Dispose(disposing);
        }
    }
}
