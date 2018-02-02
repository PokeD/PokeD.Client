using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.PokeD.Monsters
{
    public abstract class GUIPokeIcon : GUIItem
    {
        public static GUIItemSize MonsterSize { get; } = new GUIItemSize(40, 32);
        protected Vector2 Vector2 { get; }
        protected int MonsterID { get; set; }


        protected GUIPokeIcon(Screen screen, Vector2 vector2, int monsterID) : base(screen, false)
        {
            Vector2 = vector2;
            MonsterID = monsterID;
        }

        public override void Update(GameTime gameTime) { }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
