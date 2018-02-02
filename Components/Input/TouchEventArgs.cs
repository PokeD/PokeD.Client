using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

using PokeD.CPGL.Components.ViewportAdapters;

namespace PokeD.CPGL.Components.Input
{
    public class TouchEventArgs : EventArgs
    {
        public ViewportAdapter ViewportAdapter { get; }
        public TouchLocation RawTouchLocation { get; }
        public TimeSpan Time { get; }
        public Point Position { get; }

        public TouchEventArgs(ViewportAdapter viewportAdapter, TimeSpan time, TouchLocation location)
        {
            ViewportAdapter = viewportAdapter;
            RawTouchLocation = location;
            Time = time;
            Position = viewportAdapter?.PointToScreen((int) location.Position.X, (int) location.Position.Y) ?? location.Position.ToPoint();
        }

        public override bool Equals(object other)
        {
            var args = other as TouchEventArgs;

            if (args == null)
                return false;

            return ReferenceEquals(this, args) || RawTouchLocation.Id.Equals(args.RawTouchLocation.Id);
        }

        public override int GetHashCode() => RawTouchLocation.Id.GetHashCode();
    }
}