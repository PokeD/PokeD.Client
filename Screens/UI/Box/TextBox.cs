using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.Box
{
    public sealed class TextBox : GUIBox
    {
        public string Text { get; set; }

        private static Vector2 Size { get; set; }

        private static Vector2 FrameSize { get; } = new Vector2(2);

        private Rectangle FrameTopRectangle { get; }
        private Rectangle FrameBottomRectangle { get; }
        private Rectangle FrameLeftRectangle { get; }
        private Rectangle FrameRightRectangle { get; }

        private Rectangle DepthFrameRectangle { get; }
        private Rectangle DepthFrameRectangle2 { get; }


        private Texture2D FrameTexture { get; }

        private Texture2D DepthFrameTexture { get; }

        private Texture2D BlackTexture { get; }


        private Rectangle TextRectangle { get; }
        private Rectangle TextShadowRectangle { get; }


        public TextBox(Screen screen, Rectangle pos, Color style) : base(screen, pos, string.Empty, style)
        {
            Size = new Vector2(BoxRectangle.Width, BoxRectangle.Height);
            TextRectangle = new Rectangle((int) (BoxRectangle.X + 5 * Style.ResolutionScale), BoxRectangle.Y, (int) (BoxRectangle.Width - 10 * Style.ResolutionScale), BoxRectangle.Height);
            TextShadowRectangle = new Rectangle(TextRectangle.X + 1, TextRectangle.Y + 1, TextRectangle.Width, TextRectangle.Height);

            FrameTopRectangle = new Rectangle(BoxRectangle.X, BoxRectangle.Y, (int) Size.X, (int) FrameSize.Y);
            FrameBottomRectangle = new Rectangle(BoxRectangle.X, (int) (BoxRectangle.Y + Size.Y - FrameSize.Y), (int) Size.X, (int) FrameSize.Y);
            FrameLeftRectangle = new Rectangle(BoxRectangle.X, BoxRectangle.Y, (int) FrameSize.X, (int) Size.Y);
            FrameRightRectangle = new Rectangle((int) (BoxRectangle.X + Size.X - FrameSize.X), BoxRectangle.Y, (int) FrameSize.X, (int) Size.Y);

            DepthFrameRectangle = new Rectangle(FrameTopRectangle.X + 2, FrameTopRectangle.Y + 2, FrameTopRectangle.Width - 4, FrameTopRectangle.Height);
            DepthFrameRectangle2 = new Rectangle(FrameLeftRectangle.X + 2, FrameLeftRectangle.Y + 2, FrameLeftRectangle.Width, FrameLeftRectangle.Height - 4);


            BlackTexture = new Texture2D(GraphicsDevice, 1, 1);
            BlackTexture.SetData(new[] { Color.White });

            FrameTexture = new Texture2D(GraphicsDevice, 1, 1);
            FrameTexture.SetData(new[] { Color.Black });

            DepthFrameTexture = new Texture2D(GraphicsDevice, 1, 1);
            DepthFrameTexture.SetData(new[] { Color.Gray });
        }
        protected override void OnButtonPressed(object sender, EventArgs eventArgs) { }

        public override void Update(GameTime gameTime) {  }
        public override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);

            SpriteBatch.Draw(BlackTexture, BoxRectangle, Rectangle.Empty, UsingColor);

            SpriteBatch.Draw(FrameTexture, FrameTopRectangle, new Rectangle(0, 0, (int) Size.X, (int) FrameSize.Y), UsingColor);
            SpriteBatch.Draw(FrameTexture, FrameBottomRectangle, new Rectangle(0, 0, (int) Size.X, (int) FrameSize.Y), UsingColor);
            SpriteBatch.Draw(FrameTexture, FrameLeftRectangle, new Rectangle(0, 0, (int) Size.Y, (int) FrameSize.X), UsingColor);
            SpriteBatch.Draw(FrameTexture, FrameRightRectangle, new Rectangle(0, 0, (int) Size.Y, (int) FrameSize.X), UsingColor);

            SpriteBatch.Draw(DepthFrameTexture, DepthFrameRectangle, new Rectangle(0, 0, (int) Size.X, (int) FrameSize.Y), UsingColor);
            SpriteBatch.Draw(DepthFrameTexture, DepthFrameRectangle2, new Rectangle(0, 0, (int) Size.X, (int) FrameSize.Y), UsingColor);


            if (IsSelected /* && ShowInput */)
            {
                TextRenderer.DrawText(SpriteBatch, Text + "_", TextShadowRectangle, Color.LightGray);
                TextRenderer.DrawText(SpriteBatch, Text + "_", TextRectangle, Color.Black);
            }
            else
            {
                TextRenderer.DrawText(SpriteBatch, Text + " ", TextShadowRectangle, Color.LightGray);
                TextRenderer.DrawText(SpriteBatch, Text + " ", TextRectangle, Color.Black);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    FrameTexture?.Dispose();
                    DepthFrameTexture?.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
