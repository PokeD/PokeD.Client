using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.PokeD.Items
{
    public sealed class PokeItem : GUIPokeItem
    {
        private Rectangle ItemRectangle { get; }
        private Texture2D ItemsTexture { get; }


        public PokeItem(Screen screen, Vector2 vector2, int itemID) : base(screen, vector2, itemID)
        {
            ItemID--;

            int WidthItems = 26;
            int RowIndex = (int) Math.Floor((float) ItemID / (float) WidthItems);
            if (RowIndex > 0) RowIndex--;
            int CollumnIndex = ItemID - (RowIndex * WidthItems);

            ItemRectangle = new Rectangle(CollumnIndex * 32, RowIndex * 32, 32, 32);


            ItemsTexture = Screen.ContentFolder.TextureFolder.GetTextureFile("Items");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            SpriteBatch.Draw(ItemsTexture, Vector2, ItemRectangle, Color.White);
            SpriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            ItemsTexture.Dispose();

            base.Dispose(disposing);
        }
    }
}
