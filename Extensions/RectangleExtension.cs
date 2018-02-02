using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Extensions
{
    public static class RectangleExtension
    {
        public static Rectangle Multiply(this Rectangle rectangle, float value)
        {
            rectangle.X = (int) (rectangle.X * value);
            rectangle.Y = (int) (rectangle.Y * value);
            rectangle.Width = (int) (rectangle.Width * value);
            rectangle.Height = (int) (rectangle.Height * value);

            return rectangle;
        }

        public static Rectangle SetWidth(this Rectangle rectangle, float value)
        {
            rectangle.Width = (int) value;

            return rectangle;
        }
    }
}
