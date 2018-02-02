using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.Box
{
    public class ChatBox : GUIBox
    {
        List<string> ChatHistory { get; } = new List<string>();

        public ChatBox(Screen screen, Action<string> onMessage, Rectangle rect, Color style) : base(screen, rect, null, style)
        {
            onMessage += OnMessage;
        }

        protected override void OnButtonPressed(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnMessage(string message) { ChatHistory.Add(message); }
    }
}
