using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.PokeD.Pokeballs
{
    public sealed class Pokeball : GUIPokeball
    {
        private Rectangle ItemRectangle { get; set; }
        private Texture2D PokeballsTexture { get; }


        public Pokeball(Screen screen, Vector2 vector2, int pokeballID) : base(screen, vector2, pokeballID)
        {
            PokeballsTexture = Screen.ContentFolder.TextureFolder.GetTextureFile("Pokeballs");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            SpriteBatch.Draw(PokeballsTexture, Vector2, ItemRectangle, Color.White);
            SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            PokeballsTexture.Dispose();

            base.Dispose(disposing);
        }
    }
}
