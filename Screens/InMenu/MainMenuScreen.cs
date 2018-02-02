using System;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeD.CPGL.Screens.InGame;
using PokeD.CPGL.Screens.UI.Box;
using PokeD.CPGL.Screens.UI.Button;

namespace PokeD.CPGL.Screens.InMenu
{
    public sealed class MainMenuScreen : ScreenMenu
    {
        Texture2D MainBackgroundTexture { get; }
        //Texture2D _xboxController;

        public MainMenuScreen(PortableGame game) : base(game)
        {
            MainBackgroundTexture = ContentFolder.TextureFolder.GetTextureFile("MainMenuBackground");

            OnResize();

            Game.IsMouseVisible = true;
        }

        private void OnDirectConnectButton(object sender, ConnectionEventArgs args) { /* AddScreenAndCloseThis(new BattleScreen(Game, Player, new BattleInfo2x2())); */ }

        private void OnLastServerConnectButton(object sender, ConnectionEventArgs args) { SwitchScreen(new GameScreen(Game, null)); CloseScreen(); }

        private void OnMultiplayerButtonPressed(object sender, EventArgs args)
        {
        //    AddScreenAndHideThis(new ServerListScreen(Game, this));
        }
        private void OnOptionButtonPressed(object sender, EventArgs args) { SwitchScreen(new OptionsScreen(Game, this));}
        private void OnLanguageButtonPressed(object sender, EventArgs args) { SwitchScreen(new LanguageScreen(Game, this)); }
        private void OnExitButtonPressed(object sender, EventArgs args) { Task.Delay(200).Wait(); Exit(); }

        protected override void OnResize()
        {
            base.OnResize();

            int boxCount = 3;
            int buttonCount = 3;
            int boxInterval = (ViewportAdapter.ViewportWidth - Style.BoxSize.Width * boxCount) / (boxCount + 1);
            int buttonInterval = (ViewportAdapter.Center.Y - Style.BoxSize.Center.Y - Style.ButtonSize.Height * boxCount) / (buttonCount + 1);

            int xOffset = 0;
            int yOffset = 0;

            #region Boxes
            // Left
            var lastServerBoxRectangle = new Rectangle(
                xOffset += ViewportAdapter.X + boxInterval,
                yOffset = ViewportAdapter.Center.Y - Style.BoxSize.Center.Y,
                Style.BoxSize.Width, Style.BoxSize.Height);
            var server = new LastServer { Image = null, Name = "Shitty Server", LastPlayed = "Never" };
            var LastServerBox = new BoxLastServer(this, lastServerBoxRectangle, OnLastServerConnectButton, server, Style.SecondaryBackgroundColor);

            // Center
            var connectBoxRectangle = new Rectangle(
                xOffset += Style.BoxSize.Width + boxInterval,
                yOffset = ViewportAdapter.Center.Y - Style.BoxSize.Center.Y,
                Style.BoxSize.Width, Style.BoxSize.Height);
            var ConnectBox = new BoxDirectConnect(this, connectBoxRectangle, OnDirectConnectButton, Style.SecondaryBackgroundColor);

            // Right
            var multiplayerBoxRectangle = new Rectangle(
                xOffset += Style.BoxSize.Width + boxInterval,
                yOffset = ViewportAdapter.Center.Y - Style.BoxSize.Center.Y,
                Style.BoxSize.Width, Style.BoxSize.Height);
            var MultiplayerBox = new BoxMultiplayer(this, multiplayerBoxRectangle, OnMultiplayerButtonPressed, Style.SecondaryBackgroundColor);

            AddGUIItems(LastServerBox.GetGUIItems());
            AddGUIItems(ConnectBox.GetGUIItems());
            AddGUIItems(MultiplayerBox.GetGUIItems());
            #endregion Boxes

            #region Buttons
            // First
            var languageButtonRectangle = new Rectangle(
                xOffset = ViewportAdapter.Center.X - Style.ButtonSize.Center.X,
                yOffset += Style.BoxSize.Height + buttonInterval,
                Style.ButtonSize.Width, Style.ButtonSize.Height);
            var LanguageButton = new ButtonMenu(this, "Language", languageButtonRectangle, OnLanguageButtonPressed, Style.SecondaryBackgroundColor);

            // Second
            var optionsButtonRectangle = new Rectangle(
                xOffset = ViewportAdapter.Center.X - Style.ButtonSize.Center.X,
                yOffset += Style.ButtonSize.Height + buttonInterval,
                Style.ButtonSize.Width, Style.ButtonSize.Height);
            var OptionsButton = new ButtonMenu(this, "Options", optionsButtonRectangle, OnOptionButtonPressed, Style.SecondaryBackgroundColor);

            // Third
            var exitButtonRectangle = new Rectangle(
                xOffset = ViewportAdapter.Center.X - Style.ButtonSize.Center.X,
                yOffset += Style.ButtonSize.Height + buttonInterval,
                Style.ButtonSize.Width, Style.ButtonSize.Height);
            var ExitButton = new ButtonMenu(this, "Exit", exitButtonRectangle, OnExitButtonPressed, Style.SecondaryBackgroundColor);

            AddGUIItem(LanguageButton);
            AddGUIItem(OptionsButton);
            AddGUIItem(ExitButton);
            #endregion Buttons
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //if (InputManager.IsOncePressed(Keys.Escape))
            //    Exit();
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            SpriteBatch.Draw(MainBackgroundTexture, ViewportAdapter.BoundingRectangle, Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
