using System;

using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Components.ViewportAdapters
{
    public class DefaultViewportAdapter : ViewportAdapter
    {
        public override int VirtualWidth => GraphicsDevice.Viewport.Width;
        public override int VirtualHeight => GraphicsDevice.Viewport.Height;
        public override int ViewportWidth => GraphicsDevice.Viewport.Width;
        public override int ViewportHeight => GraphicsDevice.Viewport.Height;

        public DefaultViewportAdapter(PortableGame game) : base(game) { }
        public DefaultViewportAdapter(PortableGame game, Action<EventHandler<EventArgs>> attach) : base(game, attach) { }

        public override Matrix GetScaleMatrix() => Matrix.Identity;
    }
}