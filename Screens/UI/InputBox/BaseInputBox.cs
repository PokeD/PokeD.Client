using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokeD.CPGL.Screens.UI.InputBox
{
    public sealed class BaseInputBox : GUIInputBox
    {
        private static Vector2 Size { get; set; }

        private static Vector2 FrameSize { get; } = new Vector2(2);

        private Rectangle FrameTopRectangle { get; }
        private Rectangle FrameBottomRectangle { get; }
        private Rectangle FrameLeftRectangle { get; }
        private Rectangle FrameRightRectangle { get; }

        private Rectangle DepthFrameRectangle { get; }


        private Texture2D FrameTexture { get; }
        private Texture2D DepthFrameTexture { get; }


        private Rectangle TextRectangle { get; }
        private Rectangle TextShadowRectangle { get; }


        public BaseInputBox(Screen screen, Rectangle pos, Action<string> onEnter, Color style) : base(screen, pos, onEnter, style)
        {
            Size = new Vector2(InputBoxRectangle.Width, InputBoxRectangle.Height);
            TextRectangle = new Rectangle((int) (InputBoxRectangle.X + 5 * Style.ResolutionScale), InputBoxRectangle.Y, (int) (InputBoxRectangle.Width - 10 * Style.ResolutionScale), InputBoxRectangle.Height);
            TextShadowRectangle = new Rectangle(TextRectangle.X + 1, TextRectangle.Y + 1, TextRectangle.Width, TextRectangle.Height);

            FrameTopRectangle = new Rectangle(InputBoxRectangle.X, InputBoxRectangle.Y, (int) Size.X, (int) FrameSize.Y);
            FrameBottomRectangle = new Rectangle(InputBoxRectangle.X, (int) (InputBoxRectangle.Y + Size.Y - FrameSize.Y), (int) Size.X, (int) FrameSize.Y);
            FrameLeftRectangle = new Rectangle(InputBoxRectangle.X, InputBoxRectangle.Y, (int) FrameSize.X, (int) Size.Y);
            FrameRightRectangle = new Rectangle((int) (InputBoxRectangle.X + Size.X - FrameSize.X), InputBoxRectangle.Y, (int) FrameSize.X, (int) Size.Y);

            DepthFrameRectangle = new Rectangle(FrameTopRectangle.X + 2, FrameTopRectangle.Y + 2, FrameTopRectangle.Width - 2 - 2, FrameTopRectangle.Height);


            BlackTexture = new Texture2D(GraphicsDevice, 1, 1);
            BlackTexture.SetData(new[] { Color.LightGray });

            FrameTexture = new Texture2D(GraphicsDevice, 1, 1);
            FrameTexture.SetData(new[] { Color.Black });

            DepthFrameTexture = new Texture2D(GraphicsDevice, 1, 1);
            DepthFrameTexture.SetData(new[] { Color.Gray });
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);

            SpriteBatch.Draw(BlackTexture, InputBoxRectangle, Rectangle.Empty, UsingColor);

            SpriteBatch.Draw(FrameTexture, FrameTopRectangle, new Rectangle(0, 0, (int) Size.X, (int) FrameSize.Y), UsingColor);
            SpriteBatch.Draw(FrameTexture, FrameBottomRectangle, new Rectangle(0, 0, (int) Size.X, (int) FrameSize.Y), UsingColor);
            SpriteBatch.Draw(FrameTexture, FrameLeftRectangle, new Rectangle(0, 0, (int) Size.Y, (int) FrameSize.X), UsingColor);
            SpriteBatch.Draw(FrameTexture, FrameRightRectangle, new Rectangle(0, 0, (int) Size.Y, (int) FrameSize.X), UsingColor);

            SpriteBatch.Draw(DepthFrameTexture, DepthFrameRectangle, new Rectangle(0, 0, (int)Size.X, (int)FrameSize.Y), UsingColor);


            if (IsSelected && ShowInput)
            {
                TextRenderer.DrawText(SpriteBatch, Text + "_", TextShadowRectangle, TextShadowColor);
                TextRenderer.DrawText(SpriteBatch, Text + "_", TextRectangle, TextColor);
            }
            else
            {
                TextRenderer.DrawText(SpriteBatch, Text + " ", TextShadowRectangle, TextShadowColor);
                TextRenderer.DrawText(SpriteBatch, Text + " ", TextRectangle, TextColor);
            }

            //SpriteBatch.End();
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
