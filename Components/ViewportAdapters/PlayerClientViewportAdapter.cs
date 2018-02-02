using System;

using Microsoft.Xna.Framework;

using PokeD.CPGL.Screens.InGame;

namespace PokeD.CPGL.Components.ViewportAdapters
{
    public sealed class PlayerViewportAdapter : ViewportAdapter
    {
        private PlayerScreenIndex PlayerScreenIndex { get; }
        private Rectangle Rectangle { get; set; }

        public override int VirtualWidth => ViewportWidth;
        public override int VirtualHeight => ViewportHeight;
        public override int ViewportWidth => Rectangle.Width;
        public override int ViewportHeight => Rectangle.Height;

        public override int X => Rectangle.X;
        public override int Y => Rectangle.Y;

        public PlayerViewportAdapter(PortableGame game, PlayerScreenIndex playerScreenIndex) : base(game)
        {
            PlayerScreenIndex = playerScreenIndex;
            Rectangle = GetRectangle();
        }
        public PlayerViewportAdapter(PortableGame game, PlayerScreenIndex playerScreenIndex, Action<EventHandler<EventArgs>> attach) : base(game, attach)
        {
            PlayerScreenIndex = playerScreenIndex;

            Rectangle = GetRectangle();

            OnResize += PlayerViewportAdapter_OnResize;
        }
        private void PlayerViewportAdapter_OnResize(object sender, EventArgs e) => Rectangle = GetRectangle();
        private Rectangle GetRectangle()
        {
            var fullWidth = (int) (Game.ViewportAdapter.ViewportWidth * 1.0f);
            var fullHeight = (int) (Game.ViewportAdapter.ViewportHeight * 1.0f);
            var halfWidth = (int) (Game.ViewportAdapter.ViewportWidth * 0.5f);
            var halfHeight = (int) (Game.ViewportAdapter.ViewportHeight * 0.5f);

            switch (PlayerScreenIndex)
            {
                case PlayerScreenIndex.PlayerOne:
                    return new Rectangle(0, 0, fullWidth, fullHeight);


                case PlayerScreenIndex.PlayerOneHalf:
                    return new Rectangle(0, 0, fullWidth, halfHeight);

                case PlayerScreenIndex.PlayerTwoHalf:
                    return new Rectangle(0, halfHeight, fullWidth, halfHeight);


                default:
                    return Rectangle.Empty;
            }
        }

        public override Matrix GetScaleMatrix() => Matrix.Identity;

        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    OnResize -= PlayerViewportAdapter_OnResize;
                }
            }

            base.Dispose(disposing);
        }
    }
}