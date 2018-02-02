using System;

using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Physics.Collision
{
    public interface IPosition2DChanged
    {
        Action<Vector2> Position2DChangedAction { get; }
    }
}