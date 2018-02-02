using System;
using Microsoft.Xna.Framework;
using PokeD.BattleEngine.Trainer.Data;
using PokeD.CPGL.Screens.UI.Grid;
using PokeD.CPGL.Screens.UI.PokeD.Monsters;

namespace PokeD.CPGL.Screens.UI.PokeD.Overworld
{
    public sealed class PokeParty : GUIPokeParty
    {
        private Vector2 IconX { get; } = new Vector2(GUIPokeIcon.MonsterSize.Width, 0);
        private Vector2 IconY { get; } = new Vector2(0, GUIPokeIcon.MonsterSize.Height);

        private GUIPokeIcon PokeIcon_1 { get; }
        private GUIPokeIcon PokeIcon_2 { get; }
        private GUIPokeIcon PokeIcon_3 { get; }
        private GUIPokeIcon PokeIcon_4 { get; }
        private GUIPokeIcon PokeIcon_5 { get; }
        private GUIPokeIcon PokeIcon_6 { get; }

        private GUIGrid Grid { get; }


        public PokeParty(Screen screen, Vector2 vector2, MonsterTeam monsterTeam) : base(screen, vector2, monsterTeam)
        {
            PokeIcon_1 = new PokeIcon(Screen, GetMonsterVector2(0), TrainerTeam.List[0].StaticData.ID);
            PokeIcon_2 = new PokeIcon(Screen, GetMonsterVector2(1), TrainerTeam.List[1].StaticData.ID);
            PokeIcon_3 = new PokeIcon(Screen, GetMonsterVector2(2), TrainerTeam.List[2].StaticData.ID);
            PokeIcon_4 = new PokeIcon(Screen, GetMonsterVector2(3), TrainerTeam.List[3].StaticData.ID);
            PokeIcon_5 = new PokeIcon(Screen, GetMonsterVector2(4), TrainerTeam.List[4].StaticData.ID);
            PokeIcon_6 = new PokeIcon(Screen, GetMonsterVector2(5), TrainerTeam.List[5].StaticData.ID);


            var vec = IconX * Style.ResolutionScale + IconY * 6 * Style.ResolutionScale;
            var rec = new Rectangle((int) vector2.X, (int) vector2.Y, (int) vec.X, (int) vec.Y);
            Grid = new ColoredGrid(Screen, rec, Color.Gray);
        }
        private Vector2 GetMonsterVector2(byte index)
        {
            if(index > 6)
                throw new ArgumentOutOfRangeException(nameof(index));

            return Vector2 + IconY * index * Style.ResolutionScale;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            Grid.Update(gameTime);

            PokeIcon_1.Update(gameTime);
            PokeIcon_2.Update(gameTime);
            PokeIcon_3.Update(gameTime);
            PokeIcon_4.Update(gameTime);
            PokeIcon_5.Update(gameTime);
            PokeIcon_6.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            Grid.Draw(gameTime);

            PokeIcon_1.Draw(gameTime);
            PokeIcon_2.Draw(gameTime);
            PokeIcon_3.Draw(gameTime);
            PokeIcon_4.Draw(gameTime);
            PokeIcon_5.Draw(gameTime);
            PokeIcon_6.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            Grid.Dispose();

            PokeIcon_1.Dispose();
            PokeIcon_2.Dispose();
            PokeIcon_3.Dispose();
            PokeIcon_4.Dispose();
            PokeIcon_5.Dispose();
            PokeIcon_6.Dispose();


            base.Dispose(disposing);
        }
    }
}
