using Aragas.Network.Data;

namespace PokeD.CPGL.Extensions
{
    public static class Vector2Extension
    {
        public static Vector2 ToAragasVector2(this Microsoft.Xna.Framework.Vector2 vector) => new Vector2(vector.X, vector.Y);
        public static Microsoft.Xna.Framework.Vector2 ToXNAVector2(this Vector2 vector) => new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
    }
}
