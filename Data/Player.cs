using Microsoft.Xna.Framework;

using PokeD.BattleEngine.Trainer.Data;
using PokeD.BattleEngine.Trainer.Enums;

namespace PokeD.CPGL.Data
{
    public class Player //: IOpponentInfo, IOpponentTeamInfo, IOpponentBattleInfo
    {
        public int EntityID { get; }
        public short TrainerSprite { get; }
        public string Name { get; }

        public short TrainerID { get; }
        public Gender Gender { get; }

        public MonsterTeam MonsterTeam { get; }
        //public IMonsterBattleInfo MainMonster { get; }

        public Player(string name, Gender gender, MonsterTeam monsterTeam)
        {
            MonsterTeam = monsterTeam;
        }

        public void Update(GameTime gameTime)
        {

        }
    }
}