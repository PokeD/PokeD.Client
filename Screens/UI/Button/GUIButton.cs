using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PokeD.CPGL.Components.Input;
using PokeD.CPGL.Extensions;

namespace PokeD.CPGL.Screens.UI.Button
{
    public abstract class GUIButton : GUIItem
    {
        public event EventHandler OnButtonPressed;

        protected Color TextureColor { get; }

        protected string ButtonText { get; }
        public Rectangle ButtonRectangle { get; protected set; }
        protected Rectangle ButtonRectangleShadow => new Rectangle(ButtonRectangle.X + 2, ButtonRectangle.Y + 2, ButtonRectangle.Width, ButtonRectangle.Height);

        private Point WidgetSize { get; } = new Point(430, 114);

        #region Button

        protected Rectangle ButtonPosition = new Rectangle(4, 42, 131, 27);
        protected Rectangle ButtonPressedPosition = new Rectangle(4, 42, 131, 27);
        protected Rectangle ButtonUnavailablePosition = new Rectangle(4, 42, 131, 27);

        #endregion Button

        #region HalfButton

        protected Rectangle ButtonFirstHalfPosition = new Rectangle(4, 42, 65, 27);
        protected Rectangle ButtonSecondHalfPosition = new Rectangle(70, 42, 65, 27);

        protected Rectangle ButtonPressedFirstHalfPosition = new Rectangle(4, 42, 65, 27);
        protected Rectangle ButtonPressedSecondHalfPosition = new Rectangle(70, 42, 65, 27);

        protected Rectangle ButtonUnavailableFirstHalfPosition = new Rectangle(4, 42, 65, 27);
        protected Rectangle ButtonUnavailableSecondHalfPosition = new Rectangle(70, 42, 65, 27);

        protected Rectangle ButtonRectangleFirstHalf;
        protected Rectangle ButtonRectangleSecondHalf;

        #endregion HalfButton

        protected Color ButtonColor = Color.LightGray;
        protected Color ButtonShadowColor = Color.Black;

        protected Color ButtonPressedColor = Color.Gold;
        protected Color ButtonPressedShadowColor = Color.Black;

        protected Color ButtonUnavailableColor = Color.Gray;


        protected Texture2D WidgetsTexture { get; }


        private SoundEffect ButtonEffect { get; }


        protected GUIButton(Screen screen, string text, EventHandler action, Color textureColor) : base(screen, true)
        {
            WidgetsTexture = screen.ContentFolder.TextureFolder.GetTextureFile("Button");
            ButtonEffect = screen.ContentFolder.SoundEffectsFolder.GetSoundEffectFile(@"menu_button");
            //WidgetsTexture = screen.ContentManager.Load<Texture2D>(@"Textures\Button");
            //ButtonEffect = screen.ContentManager.Load<SoundEffect>(@"Sounds\menu_button");

            #region Texture scaling

            if (WidgetSize.X != WidgetsTexture.Width)
            {
                var scale = (float) WidgetsTexture.Width / (float) WidgetSize.X;
            
                ButtonPosition = ButtonPosition.Multiply(scale);
                ButtonPressedPosition = ButtonPressedPosition.Multiply(scale);
                ButtonUnavailablePosition = ButtonUnavailablePosition.Multiply(scale);
            
                #region HalfButton
            
                ButtonFirstHalfPosition = ButtonFirstHalfPosition.Multiply(scale);
                ButtonSecondHalfPosition = ButtonSecondHalfPosition.Multiply(scale);
            
                ButtonPressedFirstHalfPosition = ButtonPressedFirstHalfPosition.Multiply(scale);
                ButtonPressedSecondHalfPosition = ButtonPressedSecondHalfPosition.Multiply(scale);
            
                ButtonUnavailableFirstHalfPosition = ButtonUnavailableFirstHalfPosition.Multiply(scale);
                ButtonUnavailableSecondHalfPosition = ButtonUnavailableSecondHalfPosition.Multiply(scale);
            
                #endregion HalfButton
            }

            #endregion Texture scaling

            TextureColor = textureColor;
            ButtonText = text;
            OnButtonPressed += action;

            MouseListener.MouseMoved += (this, MouseListener_MouseMoved);
            MouseListener.MouseClicked += (this, MouseListener_MouseClicked);
            MouseListener.MouseDoubleClicked += (this, MouseListener_MouseClicked);
            MouseListener.MouseDragEnd += (this, MouseListener_MouseClicked);
            TouchListener.TouchMoved += (this, TouchListener_TouchMoved);
            TouchListener.TouchEnded += (this, TouchListener_TouchEnded);
        }
        private void MouseListener_MouseMoved(object sender, MouseEventArgs e)
        {
            if (!IsNonPressable && ButtonRectangle.Intersects(new Rectangle(e.Position.X, e.Position.Y, 1, 1)))
                ToSelectedMouseHover();
            else if (!IsNonPressable && !IsSelected)
                ToActive();
        }
        private void MouseListener_MouseClicked(object sender, MouseEventArgs e)
        {
            if (!IsNonPressable && ButtonRectangle.Intersects(new Rectangle(e.Position.X, e.Position.Y, 1, 1)))
                PressButton();
        }
        private void TouchListener_TouchMoved(object sender, TouchEventArgs e)
        {
            if (!IsNonPressable)
            {
                if (ButtonRectangle.Contains(e.Position))
                    ToSelectedMouseHover();
                else
                    ToActive();
            }
        }
        private void TouchListener_TouchEnded(object sender, TouchEventArgs e)
        {
            if (!IsNonPressable && ButtonRectangle.Contains(e.Position))
                PressButton();
        }

        public void PressButton()
        {
            ButtonEffect.Play();

            OnButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }
        public override void Update(GameTime gameTime) { }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (OnButtonPressed != null)
                        foreach (var @delegate in OnButtonPressed.GetInvocationList())
                            OnButtonPressed -= (EventHandler)@delegate;

                    MouseListener.MouseMoved -= MouseListener_MouseMoved;
                    MouseListener.MouseClicked -= MouseListener_MouseClicked;
                    MouseListener.MouseDoubleClicked -= MouseListener_MouseClicked;
                    MouseListener.MouseDragEnd -= MouseListener_MouseClicked;
                    TouchListener.TouchMoved -= TouchListener_TouchMoved;
                    TouchListener.TouchEnded -= TouchListener_TouchEnded;
                }
            }

            base.Dispose(disposing);
        }
    }
}
