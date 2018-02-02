/*
using Microsoft.Xna.Framework;

using PokeD.CPGL.Data;
using PokeD.CPGL.Screens.PokeGUI.Overworld;
using PokeD.CPGL.Screens.PokeGUI.Monsters;

namespace PokeD.CPGL.Screens.InGame
{
    public sealed class BattleScreen : Screens.GameScreen
    {
        private IBattleInfo BattleInfo { get; }
        

        public BattleScreen(PortableGame game, Player player, IBattleInfo battleInfo) : base(game, player)
        {
            BattleInfo = battleInfo;

            Game.IsMouseVisible = true;

            OnResize();
        }

        protected override void OnResize()
        {
            base.OnResize();

            var vec = new Vector2(ViewportAdapter.ViewportWidth - GUIPokeIcon.MonsterSize.Width * Style.ResolutionScale, ViewportAdapter.Center.Y);
            AddGUIItem(new PokeParty(this, vec, Player.MonsterTeam));
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game.IsActive)
                return;

            base.Update(gameTime);

            Player.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            
            var player = Player as IOpponentBattleInfo;

            var type = BattleInfo.GetType();
            if (type == typeof(BattleInfo1x1))
            {
                var battle = BattleInfo as BattleInfo1x1;

            }
            else if (type == typeof(BattleInfo2x2))
            {
                var battle = BattleInfo as BattleInfo2x2;

            }
            else if (type == typeof(BattleInfo3x3))
            {
                var battle = BattleInfo as BattleInfo3x3;

            }
            

            base.Draw(gameTime);
        }
    }

    public interface IBattleInfo
    {
    }
}
*/