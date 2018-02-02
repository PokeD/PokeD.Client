using System;
using Microsoft.Xna.Framework;
using PokeD.CPGL.Screens.UI.Grid;

namespace PokeD.CPGL.Screens.UI.Box
{
    public sealed class BoxMultiplayer : GUIBox
    {
        private event EventHandler OnButton;

        public BoxMultiplayer(Screen screen, Rectangle rect, EventHandler onButton, Color style) : base(screen, rect, "Servers", style)
        {
            OnButton += onButton;


            var GridRectangle = new Rectangle(
                BoxRectangle.X + BoxGrid.OffsetX,
                BoxRectangle.Y + BoxGrid.OffsetY,
                BoxGrid.Width, BoxGrid.Height - Style.ButtonSize.Height);
            var Grid = new BaseGrid(Screen, GridRectangle);
            AddGUIItem(Grid);
        }
        protected override void OnButtonPressed(object sender, EventArgs eventArgs)
        {
            OnButton?.Invoke(this, EventArgs.Empty);
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
                    if (OnButton != null)
                        foreach (var @delegate in OnButton.GetInvocationList())
                            OnButton -= (EventHandler)@delegate;
                }
            }

            base.Dispose(disposing);
        }
    }
}
