using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.PokeD.Items
{
    public abstract class GUIPokeItem : GUIItem
    {
        protected Vector2 Vector2 { get; }
        protected int ItemID { get; set; }


        protected GUIPokeItem(Screen screen, Vector2 vector2, int itemID) : base(screen, false)
        {
            Vector2 = vector2;
            ItemID = itemID;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
