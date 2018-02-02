using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PokeD.CPGL.Screens.UI.Grid;
using PokeD.CPGL.Screens.UI.Text;

namespace PokeD.CPGL.Screens.UI.Box
{
    public struct LastServer
    {
        public Texture2D Image { get; set; }
        public string Name { get; set; }
        public string LastPlayed { get; set; }
    }
    public sealed class BoxLastServer : GUIBox
    {
        private event EventHandler<ConnectionEventArgs> OnConnection;

        private LastServer Server { get; }


        public BoxLastServer(Screen screen, Rectangle rect, EventHandler<ConnectionEventArgs> onButton, LastServer server, Color style) : base(screen, rect, "Continue", style)
        {
            Server = server;
            OnConnection += onButton;


            var yOffset = 0;

            var serverNameTextRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += BoxRectangle.Y + BoxGrid.OffsetY,
                BoxGrid.Width, Style.FontNormalSize);
            var ServerNameText = new BaseText(Screen, Server.Name, serverNameTextRectangle, Color.White);
            AddGUIItem(ServerNameText);

            var GridRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Style.FontNormalSize,
                BoxGrid.Width, BoxGrid.Height - Style.FontNormalSize - Style.ButtonSize.Height - Style.FontSmallSize);
            var Grid = new BaseGrid(Screen, GridRectangle);
            AddGUIItem(Grid);
            
            var lastPlayedTextRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Grid.BackgroundRectangle.Height,
                BoxGrid.Width, Style.FontSmallSize);
            var LastPlayedText = new BaseText(Screen, "Last Played: " + Server.LastPlayed, lastPlayedTextRectangle, Color.LightGray);
            AddGUIItem(LastPlayedText);
        }
        protected override void OnButtonPressed(object sender, EventArgs eventArgs)
        {
            OnConnection?.Invoke(this, new ConnectionEventArgs());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (OnConnection != null)
                        foreach (var @delegate in OnConnection.GetInvocationList())
                            OnConnection -= (EventHandler<ConnectionEventArgs>) @delegate;
                }
            }

            base.Dispose(disposing);
        }
    }
}
