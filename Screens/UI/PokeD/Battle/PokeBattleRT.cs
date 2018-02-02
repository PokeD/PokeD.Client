/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PokeD.Core.Data.PokeD.Trainer.Interfaces;

using PokeD.CPGL.Data;

namespace PokeD.CPGL.Screens.PokeGUI.Battle
{
    public sealed class PokeBattleRT : GUIPokeBattle
    {
        private Rectangle RenderBounds { get; }
        private Point TextureBounds { get; } = new Point(192, 192);
        private RenderTarget2D RenderTarget { get; }

        private AnimatedGif MonsterTexture { get; }

        public PokeBattleRT(MonoGameClient game, Screen screen, Vector2 vector2, MonsterMetadata monsterMetadata,
            IOpponentBattleInfo battleInfo) : base(game, screen, vector2, monsterMetadata, battleInfo)
        {
            MonsterTexture = MonsterTextureLoader.LoadMonster(GraphicsDevice, MonsterBattleInfo.Species, MonsterMetadata);

            RenderBounds = new Rectangle(0, 0, (int)(ScreenRectangle.Width / 3), (int)(ScreenRectangle.Height / 3));
            //RenderBounds = new Rectangle(0, 0, TextureBounds.X, TextureBounds.Y);
            RenderTarget = new RenderTarget2D(GraphicsDevice, RenderBounds.Width, RenderBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            SpriteBatch.Draw(MonsterTexture, Vector2, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, SpriteBatchEffect);
            SpriteBatch.Draw(RenderTarget, ScreenRectangle, RenderBounds, Color.White);
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