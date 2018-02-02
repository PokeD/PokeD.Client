using System;

using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Physics.Collision
{
    public interface IPosition2DConverter
    {
        Func<Vector2, Vector2> Position2DConverterFunc { get; }
    }
}