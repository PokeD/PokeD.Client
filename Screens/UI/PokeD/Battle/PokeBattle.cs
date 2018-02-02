/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.Core.Data.PokeD.Trainer.Interfaces;

using PokeD.CPGL.Data;

namespace PokeD.CPGL.Screens.PokeGUI.Battle
{
    public sealed class PokeBattle : GUIPokeBattle
    {
        private AnimatedGif MonsterTexture { get; }


        public PokeBattle(MonoGameClient game, Screen screen, Vector2 vector2, MonsterMetadata monsterMetadata, IOpponentBattleInfo battleInfo) : base(game, screen, vector2, monsterMetadata, battleInfo)
        {
            MonsterTexture = MonsterTextureLoader.LoadMonster(GraphicsDevice, MonsterBattleInfo.Species, MonsterMetadata);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            SpriteBatch.Draw(MonsterTexture, Vector2, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            SpriteBatch.End();
        }

        public override void Dispose()
        {
            base.Dispose();

            MonsterTexture.Dispose();
        }
    }
}
*/