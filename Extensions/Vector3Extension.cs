using Aragas.Network.Data;

namespace PokeD.CPGL.Extensions
{
    public static class Vector3Extension
    {
        public static Vector3 ToAragasVector3(this Microsoft.Xna.Framework.Vector3 vector) => new Vector3(vector.X, vector.Y, vector.Z);
        public static Microsoft.Xna.Framework.Vector3 ToXNAVector3(this Vector3 vector) => new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
    }
}
