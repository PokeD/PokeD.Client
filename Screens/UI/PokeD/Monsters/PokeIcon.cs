using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.PokeD.Monsters
{
    public sealed class PokeIcon : GUIPokeIcon
    {
        private Rectangle ItemRectangle { get; }
        private static Texture2D IconsTexture { get; set; }

        
        public PokeIcon(Screen screen, Vector2 vector2, int monsterID) : base(screen, vector2, monsterID)
        {
            MonsterID--;

            int WidthItemsCount = 32;
            int RowIndex = (int) Math.Floor((float) MonsterID / (float) WidthItemsCount);
            if (RowIndex > 0) RowIndex--;
            int CollumnIndex = MonsterID - (RowIndex * WidthItemsCount);

            ItemRectangle = new Rectangle(CollumnIndex * MonsterSize.Width, RowIndex * MonsterSize.Height, MonsterSize.Width, MonsterSize.Height);


            if(IconsTexture == null)
                IconsTexture = Screen.ContentFolder.TextureFolder.GetTextureFile("Pokemon Icons");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            SpriteBatch.Draw(IconsTexture, Vector2, ItemRectangle, Color.White, 0f, Vector2.Zero, Style.ResolutionScale, SpriteEffects.None, 0f);
            SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            //IconsTexture.Dispose();

            base.Dispose(disposing);
        }
    }
}
