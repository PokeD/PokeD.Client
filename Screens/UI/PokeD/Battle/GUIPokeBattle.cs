/*
using System;

using Microsoft.Xna.Framework;

using PokeD.CPGL.Screens.GUI;

namespace PokeD.CPGL.Screens.PokeGUI.Battle
{
    [Flags]
    public enum MonsterMetadata
    {
        Front = 0,
        Back = 1,
        Info = 2,

        First = 4,
        Second = 8,
        Third = 16,
        Fourth = 32,

        MoveOne = 64,
        MoveTwo = 128,
        MoveThree = 256,
        MoveFour = 512,
        MoveFive = 1024
    }

    public abstract class GUIPokeBattle : GUIItem
    {
        protected Vector2 Vector2 { get; }
        protected MonsterMetadata MonsterMetadata { get; }
        protected IMonsterBattleInfo MonsterBattleInfo { get; private set; }
        

        public GUIPokeBattle(MonoGameClient game, Screen screen, Vector2 vector2, MonsterMetadata monsterMetadata, IOpponentBattleInfo battleInfo) : base(game, screen, false)
        {
            Vector2 = vector2;
            MonsterMetadata = monsterMetadata;
            MonsterBattleInfo = battleInfo.MainMonster;
        }

        public void UpdateMonsterInfo(IMonsterBattleInfo monsterBattleInfo) { MonsterBattleInfo = monsterBattleInfo; }

        public override void Update(GameTime gameTime) { }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }

        public override void Dispose()
        {
            base.Dispose();

        }
    }
}
*/