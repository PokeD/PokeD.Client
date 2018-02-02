using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.PokeD.Battle
{
    public sealed class BattleGround : GUIBattleGround
    {
        private Texture2D BattleGroundTexture { get; }


        public BattleGround(Screen screen, int battleGroundID) : base(screen, battleGroundID)
        {
            BattleGroundID--;

            int WidthItems = 2;
            int RowIndex = (int) Math.Floor((float) BattleGroundID / (float) WidthItems);
            //if (RowIndex > 0) RowIndex--;
            int CollumnIndex = BattleGroundID - (RowIndex * WidthItems);

            BattleGroundRectangle = new Rectangle((CollumnIndex + 1) * 2 + CollumnIndex * 320, (RowIndex + 1) * 2 + RowIndex * 240, 320, 240);


            BattleGroundTexture = Screen.ContentFolder.TextureFolder.GetTextureFile("Backgrounds");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            SpriteBatch.Draw(BattleGroundTexture, ViewportAdapter.BoundingRectangle, BattleGroundRectangle, Color.White);
            SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            BattleGroundTexture.Dispose();

            base.Dispose(disposing);
        }
    }
}
