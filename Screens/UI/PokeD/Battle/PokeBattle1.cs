/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.PokeGUI.Battle
{
    public sealed class PokeBattle1 : GUIPokeBattle
    {
        private Texture2D Texture { get; }

        public PokeBattle1(MonoGameClient game, Screen screen, Vector2 vector2) : base(game, screen, vector2, 0, null)
        {
            Texture = new Texture2D(GraphicsDevice, 1, 1);
            Texture.SetData(new[] { new Color(0, 0, 0, 170) });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(new Vector3(Vector2.Zero, 0.0f)) * // Position
                   Matrix.CreateRotationZ(0f)*
                   //Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                   Matrix.CreateTranslation(new Vector3(Vector2.Zero, 0.0f));  // Origin
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //Matrix projection = Matrix.CreateOrthographicOffCenter(0, ScreenRectangle.Width, ScreenRectangle.Height, 0, 0, 1);
            //Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            //Matrix projection = Matrix.CreateOrthographic(ScreenRectangle.Width, ScreenRectangle.Height, 0, 0);
           // Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);


            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);//, projection);
            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GetTransform());
            SpriteBatch.Draw(Texture, Vector2, new Rectangle(0, 0, 100, 20), Color.White);
            SpriteBatch.End();
        }

        public override void Dispose()
        {
            base.Dispose();

        }
    }
}
*/