using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Components.ViewportAdapters
{
    public abstract class ViewportAdapter : Component
    {
        public event EventHandler<EventArgs> OnResize;

        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public Viewport Viewport => GraphicsDevice.Viewport;

        public virtual int X => 0;
        public virtual int Y => 0;

        public abstract int VirtualWidth { get; }
        public abstract int VirtualHeight { get; }
        public abstract int ViewportWidth { get; }
        public abstract int ViewportHeight { get; }

        public Rectangle BoundingRectangle => new Rectangle(0, 0, VirtualWidth, VirtualHeight);
        public Point Center => BoundingRectangle.Center;

        protected ViewportAdapter(PortableGame game) : base(game) { }
        protected ViewportAdapter(PortableGame game, Action<EventHandler<EventArgs>> attach) : base(game) => attach((sender, args) => OnResize?.Invoke(sender, args));

        public abstract Matrix GetScaleMatrix();

        public Point PointToScreen(Point point) => PointToScreen(point.X, point.Y);
        public virtual Point PointToScreen(int x, int y)
        {
            var scaleMatrix = GetScaleMatrix();
            var invertedMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(new Vector2(x, y), invertedMatrix).ToPoint();
        }

        public virtual void Reset() { }

        public override void Update(GameTime gameTime) { }
    }
}