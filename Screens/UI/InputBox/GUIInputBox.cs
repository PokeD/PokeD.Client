using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeD.CPGL.Components.Input;

namespace PokeD.CPGL.Screens.UI.InputBox
{
    public abstract class GUIInputBox : GUIItem
    {
        public event Action<string> EnterPressed;
        public event Action<GUIInputBox> Focused;
        public event Action<GUIInputBox> Unfocused;


        protected Color UsingColor { get; }
        protected bool ShowInput { get; set; }

        protected Rectangle InputBoxRectangle { get; }


        protected string Text { get; private set; } = "";

        protected Color TextColor = Color.White;
        protected Color TextShadowColor = Color.Gray;
       
        protected Texture2D BlackTexture { get; set; }


        private const int CycleNumb = 40;
        private int CycleCount = CycleNumb;


        protected GUIInputBox(Screen screen, Rectangle pos, Action<string> onEnterPressed, Color style) : base(screen, true)
        {
            InputBoxRectangle = pos;
            UsingColor = style;
            EnterPressed += onEnterPressed;

            KeyboardListener.KeyTyped += (this, KeyboardListener_KeyTyped);
            MouseListener.MouseClicked += (this, MouseListener_MouseClicked);
            TouchListener.TouchEnded += (this, TouchListener_TouchEnded);
            GamePadListener.ButtonDown += (this, GamePadListener_ButtonDown);
        }
        private void KeyboardListener_KeyTyped(object sender, KeyboardEventArgs e)
        {
            if (IsSelected)
            {
                switch (e.Key)
                {
                    case Keys.Back:
                        if (Text.Length == 0)
                            break;

                        Text = Text.Remove(Text.Length - 1, 1);
                        break;

                    case Keys.Enter:
                        EnterPressed?.Invoke(Text);
                        break;

                    default:
                        Text += e.Character;
                        break;
                }
            }
        }
        private void MouseListener_MouseClicked(object sender, MouseEventArgs e)
        {
            if (!IsNonPressable && InputBoxRectangle.Intersects(new Rectangle(e.Position.X, e.Position.Y, 1, 1)))
            {
                Focused?.Invoke(this);

                ToSelected();
            }
            else if (!IsNonPressable)
            {
                Unfocused?.Invoke(this);

                ToActive();
            }
        }
        private void TouchListener_TouchEnded(object sender, TouchEventArgs e)
        {
            if (!IsNonPressable)
            {
                if (InputBoxRectangle.Contains(e.Position))
                {
                    Focused?.Invoke(this);

                    ToSelected();
                }
                else
                {
                    Unfocused?.Invoke(this);

                    ToActive();
                }
            }
        }
        private void GamePadListener_ButtonDown(object sender, GamePadEventArgs e)
        {
            if(!IsSelected)
                return;

            switch (e.Button)
            {
                    case Buttons.A:
                        Screen.GetDaisywheel(Text).InputDone += (s, args) =>
                        {
                            Text = args.Text;
                        };

                        //Screen.Daisywheel.OnCharReceived += character => Text += character;
                        //Screen.Daisywheel.OnCharDeleted += () => { if (Text.Length == 0) return; Text = Text.Remove(Text.Length - 1, 1); };
                    break;
            }
        }

        public void OnEnterPressed()
        {
            EnterPressed?.Invoke(Text);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsSelected)
            {
                if (CycleCount > CycleNumb)
                {
                    ShowInput = !ShowInput;
                    CycleCount = 0;
                }

                CycleCount++;
            }

            #region Mouse handling

            /*
            else if (!IsNonPressable)
            {


                foreach (var gesture in InputManager.Gestures)
                    if (gesture.GestureType == GestureType.Tap)
                    {
                        if (OnUnFocused != null)
                            OnUnFocused(this);

                        ToActive();

                        InputManager.HideKeyboard();

                        break;
                    }
            }
            */

            /*
            if (!IsNonPressable && !InputBoxRectangle.Intersects(new Rectangle(InputManager.CurrentMouseState.X, InputManager.CurrentMouseState.Y, 1, 1)))
            {
                if (InputManager.MouseLeftClicked)
                {
                    if (OnUnFocused != null)
                        OnUnFocused(this);

                    ToActive();
                }
            }
            else if (!IsNonPressable && !InputManager.CurrentTouchCollection.Any(location => InputBoxRectangle.Contains(location.Position)))
            {
                foreach (var gesture in InputManager.Gestures)
                    if (gesture.GestureType == GestureType.Tap && !InputBoxRectangle.Contains(gesture.Position))
                    {
                        if (OnUnFocused != null)
                            OnUnFocused(this);

                        ToActive();

                        InputManager.HideKeyboard();

                        break;
                    }
            }
            */
            #endregion
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (EnterPressed != null)
                foreach (var @delegate in EnterPressed.GetInvocationList())
                    EnterPressed -= (Action<string>) @delegate;

            if (Focused != null)
                foreach (var @delegate in Focused.GetInvocationList())
                    Focused -= (Action<GUIInputBox>) @delegate;

            if (Unfocused != null)
                foreach (var @delegate in Unfocused.GetInvocationList())
                    Unfocused -= (Action<GUIInputBox>) @delegate;

            KeyboardListener.KeyTyped -= KeyboardListener_KeyTyped;
            MouseListener.MouseClicked -= MouseListener_MouseClicked;
            TouchListener.TouchEnded -= TouchListener_TouchEnded;
            GamePadListener.ButtonDown -= GamePadListener_ButtonDown;

            BlackTexture?.Dispose();

            base.Dispose(disposing);
        }
    }
}
