using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.PokeD.Pokeballs
{
    public abstract class GUIPokeball : GUIItem
    {
        protected Vector2 Vector2 { get; }
        protected int PokeballID { get; set; }


        protected GUIPokeball(Screen screen, Vector2 vector2, int pokeballID) : base(screen, false)
        {
            Vector2 = vector2;
            PokeballID = pokeballID;
        }

        public override void Update(GameTime gameTime) { }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
