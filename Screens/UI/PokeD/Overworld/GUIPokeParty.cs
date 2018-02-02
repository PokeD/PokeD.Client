using Microsoft.Xna.Framework;
using PokeD.BattleEngine.Trainer.Data;

namespace PokeD.CPGL.Screens.UI.PokeD.Overworld
{
    public abstract class GUIPokeParty : GUIItem
    {
        protected Vector2 Vector2 { get; }
        protected MonsterTeam TrainerTeam { get; }


        protected GUIPokeParty(Screen screen, Vector2 vector2, MonsterTeam monsterTeam) : base(screen, false)
        {
            Vector2 = vector2;
            TrainerTeam = monsterTeam;
        }

        public override void Update(GameTime gameTime) { }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
    }
}
