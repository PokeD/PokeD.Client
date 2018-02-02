using Microsoft.Xna.Framework;

namespace PokeD.CPGL.Screens.UI.PokeD.Battle
{
    public abstract class GUIBattleGround : GUIItem
    {
        protected int BattleGroundID { get; set; }
        protected Rectangle BattleGroundRectangle { get; set; }


        public GUIBattleGround(Screen screen, int battleGroundID) : base(screen, false)
        {
            BattleGroundID = battleGroundID;
        }

        public override void Update(GameTime gameTime) { }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
