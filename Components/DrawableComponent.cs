using System;

using Microsoft.Xna.Framework;

using PokeD.CPGL.Components.Camera;
using PokeD.CPGL.Components.Input;

namespace PokeD.CPGL.Components
{
    public abstract class DrawableComponent : DrawableGameComponent
    {
        public new PortableGame Game { get; }
        protected Camera2DComponent Camera => Game.Camera;

        protected bool _isDisposed { get; private set; }

        protected KeyboardListenerComponent KeyboardListener => Game.KeyboardListener;
        protected MouseListenerComponent MouseListener => Game.MouseListener;
        protected GamePadListenerComponent GamePadListener => Game.GamePadListener;
        protected TouchListenerComponent TouchListener => Game.TouchListener;

        protected DrawableComponent(PortableGame game) : base(game) { Game = game; }
        protected DrawableComponent(Component component) : this(component.Game) { }
        protected DrawableComponent(DrawableComponent component) : this(component.Game) { }

        public abstract override void Update(GameTime gameTime);
        public abstract override void Draw(GameTime gameTime);

        [System.Diagnostics.DebuggerNonUserCode]
        protected void AssertNotDisposed()
        {
            if (_isDisposed)
            {
                var name = GetType().Name;
                throw new ObjectDisposedException(name, $"The {name} object was used after being Disposed.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _isDisposed = true;
                }
            }

            base.Dispose(disposing);
        }
    }
}