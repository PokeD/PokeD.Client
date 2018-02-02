using System;
using Microsoft.Xna.Framework;
using PokeD.CPGL.Screens.UI.InputBox;
using PokeD.CPGL.Screens.UI.Text;

namespace PokeD.CPGL.Screens.UI.Box
{
    public sealed class BoxDirectConnect : GUIBox
    {
        private event EventHandler<ConnectionEventArgs> OnConnection;
        
        private string ServerName { get; } = "Server Name:";      
        private string ServerHost { get; } = "Server Host:";
        private string ServerProtocol { get; } = "Server Protocolg:";

        // CheckBox


        public BoxDirectConnect(Screen screen, Rectangle rect, EventHandler<ConnectionEventArgs> onButton, Color style) : base(screen, rect, "Connect", style)
        {
            OnConnection += onButton;


            var yOffset = 0;

            var serverNameTextRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += BoxRectangle.Y + BoxGrid.OffsetY,
                BoxGrid.Width, Style.FontNormalSize);
            var ServerNameText = new BaseText(Screen, ServerName, serverNameTextRectangle, Color.White);
            AddGUIItem(ServerNameText);

            var serverNameInputBoxRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Style.FontNormalSize,
                BoxGrid.Width, Style.FontNormalSize);
            var ServerNameInputBox = new BaseInputBox(Screen, serverNameInputBoxRectangle, null, UsingColor);
            AddGUIItem(ServerNameInputBox);
            
            var serverHostTextRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Style.FontNormalSize,
                BoxGrid.Width, Style.FontNormalSize);
            var ServerHostText = new BaseText(Screen, ServerHost, serverHostTextRectangle, Color.White);
            AddGUIItem(ServerHostText);

            var serverHostInputBoxRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Style.FontNormalSize,
                BoxGrid.Width, Style.FontNormalSize);
            var ServerHostInputBox = new BaseInputBox(Screen, serverHostInputBoxRectangle, null, UsingColor);
            AddGUIItem(ServerHostInputBox);
            
            var serverProtocolTextRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Style.FontNormalSize,
                BoxGrid.Width, Style.FontNormalSize);
            var ServerProtocolText = new BaseText(Screen, ServerProtocol, serverProtocolTextRectangle, Color.White);
            AddGUIItem(ServerProtocolText);

            var serverProtocolListRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                yOffset += Style.FontNormalSize,
                BoxGrid.Width, Style.FontNormalSize);
            //var ServerHostInputBox = new BaseInputBox(Game, Screen, serverHostInputBoxRectangle, null, true);
            //AddGUIItem(ServerHostInputBox);
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
