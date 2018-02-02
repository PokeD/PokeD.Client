using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeD.CPGL.Components.Input;
using PokeD.CPGL.Screens.UI.Box;

namespace PokeD.CPGL.Screens.UI.GamePad
{
    public struct DaisywheelEntry
    {
        public struct CharRectangle
        {
            public ValueTuple<char, char, char> Char;
            public Rectangle Rectangle;
        }

        public Rectangle ButtonRectangle;
        public Rectangle BackgroundButtonRectangle;

        public CharRectangle[] Chars;

        public bool Selected;
    }

    public class DaisywheelCharEventArgs : EventArgs
    {
        public char Character { get; }

        public DaisywheelCharEventArgs(char character) { Character = character; }
    }
    public class DaisywheelDoneEventArgs : EventArgs
    {
        public string Text { get; }

        public DaisywheelDoneEventArgs(string text) { Text = text; }
    }

    /// <summary>
    /// Daisywheel implementation from Steam Big Picture
    /// </summary>
    public sealed class Daisywheel : GUIItem
    {
        public event EventHandler<DaisywheelCharEventArgs> CharReceived;
        public event EventHandler CharDeleted;

        public event EventHandler<DaisywheelDoneEventArgs> InputDone;


        Texture2D MainCircleTexture { get; }
        Rectangle MainCircleRectangle { get; }

        Texture2D ButtonCircleTexture { get; }

        Texture2D GamePadButtonsTexture { get; }

        DaisywheelEntry[] ButtonEntries { get; }

        //Color Color { get; } = new Color(255, 255, 255, 200);
        Color Color { get; } = new Color(255, 255, 255, 220);

        private TextBox TextBox { get; }

        private bool _disposing;

        public Daisywheel(Screen screen, string initialText = "") : base(screen, false)
        {
            // http://stackoverflow.com/questions/5641579/xna-draw-a-filled-circle
            Texture2D CreateCircle(GraphicsDevice importedGraphicsDevice, int radius, Color color)
            {
                int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
                Texture2D texture = new Texture2D(importedGraphicsDevice, outerRadius, outerRadius);

                Color[] data = new Color[outerRadius * outerRadius];

                // Colour the entire texture transparent first.
                for (int i = 0; i < data.Length; i++)
                    data[i] = Color.Transparent;

                // Work out the minimum step necessary using trigonometry + sine approximation.
                double angleStep = 1f / radius;

                for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                {
                    // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                    int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                    int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                    data[y * outerRadius + x + 1] = color;
                }

                //width
                for (int i = 0; i < outerRadius; i++)
                {
                    int yStart = -1;
                    int yEnd = -1;


                    //loop through height to find start and end to fill
                    for (int j = 0; j < outerRadius; j++)
                    {
                        if (yStart == -1)
                        {
                            if (j == outerRadius - 1)
                            {
                                //last row so there is no row below to compare to
                                break;
                            }

                            //start is indicated by Color followed by Transparent
                            if (data[i + j * outerRadius] == color && data[i + (j + 1) * outerRadius] == Color.Transparent)
                            {
                                yStart = j + 1;
                                continue;
                            }
                        }
                        else if (data[i + j * outerRadius] == color)
                        {
                            yEnd = j;
                            break;
                        }
                    }

                    //if we found a valid start and end position
                    if (yStart != -1 && yEnd != -1)
                    {
                        //height
                        for (int j = yStart; j < yEnd; j++)
                        {
                            data[i + j * outerRadius] = color;
                        }
                    }
                }

                texture.SetData(data);
                return texture;
            }

            MainCircleTexture = CreateCircle(GraphicsDevice, (int) (Math.Min(ViewportAdapter.ViewportWidth * 0.75, ViewportAdapter.ViewportHeight * 0.75) * 0.5f), new Color(25, 45, 60, 255));
            MainCircleRectangle = new Rectangle(
                (int) (ViewportAdapter.Center.X - MainCircleTexture.Width * 0.5f),
                (int) (ViewportAdapter.Center.Y - MainCircleTexture.Height * 0.5f),
                MainCircleTexture.Width,
                MainCircleTexture.Height);

            ButtonCircleTexture = CreateCircle(GraphicsDevice, (int) (Math.Min(MainCircleRectangle.Width * 0.25, MainCircleRectangle.Height * 0.25) * 0.5f), new Color(18, 42, 60, 255));

            ButtonEntries = SteamDefault();

            GamePadButtonsTexture = screen.ContentFolder.TextureFolder.GetTextureFile("ControllerButtons");

            GamePadListener.ButtonDown += (this, GamePadListener_ButtonDown);
            GamePadListener.ThumbStickMoved += (this, GamePadListener_ThumbStickMoved);
            GamePadListener.TriggerMoved += (this, GamePadListener_TriggerMoved);


            var serverNameInputBoxRectangle = new Rectangle(
                ViewportAdapter.Center.X - 400 / 2,
                ViewportAdapter.Y + 20,
                400, 40);
            TextBox = new TextBox(Screen, serverNameInputBoxRectangle, Color.White) { Text = initialText };

            CharReceived += (sender, e) => TextBox.Text += e.Character;
            CharDeleted += (sender, e) => TextBox.Text = TextBox.Text.Length > 0 ? TextBox.Text.Remove(TextBox.Text.Length - 1, 1) : string.Empty;
        }
        private void GamePadListener_ThumbStickMoved(object sender, GamePadEventArgs e)
        {
            if (e.Button == Buttons.LeftStick)
            {
                ButtonEntries[0].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.Up;
                ButtonEntries[1].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.UpRight;
                ButtonEntries[2].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.Right;
                ButtonEntries[3].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.DownRight;
                ButtonEntries[4].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.Down;
                ButtonEntries[5].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.DownLeft;
                ButtonEntries[6].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.Left;
                ButtonEntries[7].Selected = e.ThumbStickDirection == GamePadAnalogStickDirection.UpLeft;
            }
        }
        private void GamePadListener_ButtonDown(object sender, GamePadEventArgs e)
        {
            var isSelected = ButtonEntries.Any(be => be.Selected);

            if (e.Button == Buttons.X && isSelected)
                foreach (var buttonEntry in ButtonEntries)
                    if (buttonEntry.Selected)
                        CharReceived?.Invoke(this, new DaisywheelCharEventArgs(Caps ? buttonEntry.Chars[0].Char.Item2 : (Numbers ? buttonEntry.Chars[0].Char.Item3 : buttonEntry.Chars[0].Char.Item1)));


            if (e.Button == Buttons.Y && isSelected)
                foreach (var buttonEntry in ButtonEntries)
                    if (buttonEntry.Selected)
                        CharReceived?.Invoke(this, new DaisywheelCharEventArgs(Caps ? buttonEntry.Chars[1].Char.Item2 : (Numbers ? buttonEntry.Chars[1].Char.Item3 : buttonEntry.Chars[1].Char.Item1)));


            if (e.Button == Buttons.B && isSelected)
                foreach (var buttonEntry in ButtonEntries)
                    if (buttonEntry.Selected)
                        CharReceived?.Invoke(this, new DaisywheelCharEventArgs(Caps ? buttonEntry.Chars[2].Char.Item2 : (Numbers ? buttonEntry.Chars[2].Char.Item3 : buttonEntry.Chars[2].Char.Item1)));


            if (e.Button == Buttons.A && isSelected)
                foreach (var buttonEntry in ButtonEntries)
                    if (buttonEntry.Selected)
                        CharReceived?.Invoke(this, new DaisywheelCharEventArgs(Caps ? buttonEntry.Chars[3].Char.Item2 : (Numbers ? buttonEntry.Chars[3].Char.Item3 : buttonEntry.Chars[3].Char.Item1)));


            if (e.Button == Buttons.B && !isSelected)
            {
                InputDone?.Invoke(this, new DaisywheelDoneEventArgs(TextBox.Text));
                Dispose();
            }

            if (e.Button == Buttons.RightShoulder)
                CharReceived?.Invoke(this, new DaisywheelCharEventArgs(' '));

            if (e.Button == Buttons.LeftShoulder)
                CharDeleted?.Invoke(this, EventArgs.Empty);
        }

        private bool Caps { get; set; }
        private bool Numbers { get; set; }
        private void GamePadListener_TriggerMoved(object sender, GamePadEventArgs e)
        {
            switch (e.Button)
            {
                case Buttons.LeftTrigger:
                    Caps = e.TriggerState > 0f;
                    break;

                case Buttons.RightTrigger:
                    Numbers = e.TriggerState > 0f;
                    break;
            }
        }

        private DaisywheelEntry[] SteamDefault()
        {
            var bigCircleRadius         = Math.Max(MainCircleRectangle.Width, MainCircleRectangle.Height) / 2;
            var mediumCircleRadius      = (int) (bigCircleRadius * 0.700f);
            var smallCircleRadius       = (int) (bigCircleRadius * 0.225f);
            var smallestCircleRadius    = (int) (smallCircleRadius * 0.400f);

            double mul = Math.Sqrt(2D) / 2D;

            var topCenter           = new Point(MainCircleRectangle.Center.X, MainCircleRectangle.Center.Y - mediumCircleRadius);
            var topLeftCenter       = new Point(MainCircleRectangle.Center.X - (int) (mediumCircleRadius * mul), MainCircleRectangle.Center.Y - (int) (mediumCircleRadius * mul));
            var topRightCenter      = new Point(MainCircleRectangle.Center.X + (int) (mediumCircleRadius * mul), MainCircleRectangle.Center.Y - (int) (mediumCircleRadius * mul));

            var leftCenter          = new Point(MainCircleRectangle.Center.X - mediumCircleRadius, MainCircleRectangle.Center.Y);
            var rightCenter         = new Point(MainCircleRectangle.Center.X + mediumCircleRadius, MainCircleRectangle.Center.Y);

            var bottomCenter        = new Point(MainCircleRectangle.Center.X, MainCircleRectangle.Center.Y + mediumCircleRadius);
            var bottomLeftCenter    = new Point(MainCircleRectangle.Center.X - (int) (mediumCircleRadius * mul), MainCircleRectangle.Center.Y + (int) (mediumCircleRadius * mul));
            var bottomRightCenter   = new Point(MainCircleRectangle.Center.X + (int) (mediumCircleRadius * mul), MainCircleRectangle.Center.Y + (int) (mediumCircleRadius * mul));

            Rectangle CreateRectangle(Point center, int radius) => new Rectangle(center.X - radius, center.Y - radius, radius * 2, radius * 2);
            Rectangle CreateBackgroundRectangle(Rectangle rectangle) => new Rectangle(rectangle.X - 10, rectangle.Y - 10, rectangle.Width + 20, rectangle.Height + 20);
            DaisywheelEntry.CharRectangle[] CreateCharRectangle(Rectangle rectangle, ValueTuple<char, char, char> []chars, int radius)
            {
                var arrs = new DaisywheelEntry.CharRectangle[chars.Length];
                var xOffset = (int) (rectangle.Width / 2f - rectangle.Width / 5f);
                var yOffset = (int) (rectangle.Height / 2f - rectangle.Height / 5f);

                // Left
                arrs[0] = new DaisywheelEntry.CharRectangle();
                arrs[0].Char = chars[0];
                arrs[0].Rectangle = new Rectangle(
                    rectangle.Center.X - xOffset - radius,
                    rectangle.Center.Y - radius,
                    radius * 2,
                    radius * 2);

                // Top
                arrs[1] = new DaisywheelEntry.CharRectangle();
                arrs[1].Char = chars[1];
                arrs[1].Rectangle = new Rectangle(
                    rectangle.Center.X - radius,
                    rectangle.Center.Y - yOffset - radius,
                    radius * 2,
                    radius * 2);

                // Right
                arrs[2] = new DaisywheelEntry.CharRectangle();
                arrs[2].Char = chars[2];
                arrs[2].Rectangle = new Rectangle(
                    rectangle.Center.X + xOffset - radius,
                    rectangle.Center.Y - radius,
                    radius * 2,
                    radius * 2);

                // Bottom
                arrs[3] = new DaisywheelEntry.CharRectangle();
                arrs[3].Char = chars[3];
                arrs[3].Rectangle = new Rectangle(
                    rectangle.Center.X - radius,
                    rectangle.Center.Y + yOffset - radius,
                    radius * 2,
                    radius * 2);

                return arrs;
            }

            var buttonEntries = new DaisywheelEntry[8];

            // Top
            buttonEntries[0] = new DaisywheelEntry();
            buttonEntries[0].ButtonRectangle = CreateRectangle(topCenter, smallCircleRadius);
            buttonEntries[0].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[0].ButtonRectangle);
            buttonEntries[0].Chars = CreateCharRectangle(buttonEntries[0].ButtonRectangle, new [] { ('a','A', ' '), ('b', 'B', ' '), ('c', 'C', ' '), ('d', 'D', ' ') }, smallestCircleRadius);

            // Right Top
            buttonEntries[1] = new DaisywheelEntry();
            buttonEntries[1].ButtonRectangle = CreateRectangle(topRightCenter, smallCircleRadius);
            buttonEntries[1].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[1].ButtonRectangle);
            buttonEntries[1].Chars = CreateCharRectangle(buttonEntries[1].ButtonRectangle, new[] { ('e', 'E', ' '), ('f', 'F', ' '), ('g', 'G', ' '), ('h', 'H', ' ') }, smallestCircleRadius);

            // Right
            buttonEntries[2] = new DaisywheelEntry();
            buttonEntries[2].ButtonRectangle = CreateRectangle(rightCenter, smallCircleRadius);
            buttonEntries[2].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[2].ButtonRectangle);
            buttonEntries[2].Chars = CreateCharRectangle(buttonEntries[2].ButtonRectangle, new[] { ('i', 'I', ' '), ('j', 'J', ' '), ('k', 'K', ' '), ('l', 'L', ' ') }, smallestCircleRadius);

            // Right Bottom
            buttonEntries[3] = new DaisywheelEntry();
            buttonEntries[3].ButtonRectangle = CreateRectangle(bottomRightCenter, smallCircleRadius);
            buttonEntries[3].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[3].ButtonRectangle);
            buttonEntries[3].Chars = CreateCharRectangle(buttonEntries[3].ButtonRectangle, new[] { ('m', 'M', ' '), ('n', 'N', ' '), ('o', 'O', ' '), ('p', 'P', ' ') }, smallestCircleRadius);

            // Bottom
            buttonEntries[4] = new DaisywheelEntry();
            buttonEntries[4].ButtonRectangle = CreateRectangle(bottomCenter, smallCircleRadius);
            buttonEntries[4].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[4].ButtonRectangle);
            buttonEntries[4].Chars = CreateCharRectangle(buttonEntries[4].ButtonRectangle, new[] { ('q', 'Q', ' '), ('r', 'R', ' '), ('s', 'S', ' '), ('t', 'T', ' ') }, smallestCircleRadius);

            // Left Bottom
            buttonEntries[5] = new DaisywheelEntry();
            buttonEntries[5].ButtonRectangle = CreateRectangle(bottomLeftCenter, smallCircleRadius);
            buttonEntries[5].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[5].ButtonRectangle);
            buttonEntries[5].Chars = CreateCharRectangle(buttonEntries[5].ButtonRectangle, new[] { ('u', 'U', ' '), ('v', 'V', ' '), ('w', 'W', ' '), ('x', 'X', ' ') }, smallestCircleRadius);

            // Left
            buttonEntries[6] = new DaisywheelEntry();
            buttonEntries[6].ButtonRectangle = CreateRectangle(leftCenter, smallCircleRadius);
            buttonEntries[6].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[6].ButtonRectangle);
            buttonEntries[6].Chars = CreateCharRectangle(buttonEntries[6].ButtonRectangle, new[] { ('y', 'Y', ' '), ('z', 'Z', ' '), (',', '<', ' '), ('.', '>', ' ') }, smallestCircleRadius);

            // Left Top
            buttonEntries[7] = new DaisywheelEntry();
            buttonEntries[7].ButtonRectangle = CreateRectangle(topLeftCenter, smallCircleRadius);
            buttonEntries[7].BackgroundButtonRectangle = CreateBackgroundRectangle(buttonEntries[7].ButtonRectangle);
            buttonEntries[7].Chars = CreateCharRectangle(buttonEntries[7].ButtonRectangle, new[] { (':', '?', ' '), ('/', '?', ' '), ('@', '?', ' '), ('-', '?', ' ') }, smallestCircleRadius);

            return buttonEntries;
        }


        public override void Update(GameTime gameTime)
        {
            if (_disposing)
                return;

            TextBox.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if(_disposing)
                return;

            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp);

            TextBox.Draw(gameTime);

            SpriteBatch.Draw(MainCircleTexture, MainCircleRectangle, Color);

            foreach (var buttonEntry in ButtonEntries)
            {
                SpriteBatch.Draw(ButtonCircleTexture, buttonEntry.BackgroundButtonRectangle, Color);
                if (buttonEntry.Selected)
                    SpriteBatch.Draw(GamePadButtonsTexture, buttonEntry.ButtonRectangle, Color);

                TextRenderer.DrawTextCentered(SpriteBatch, Caps ? buttonEntry.Chars[0].Char.Item2.ToString() : (Numbers ? buttonEntry.Chars[0].Char.Item3.ToString() : buttonEntry.Chars[0].Char.Item1.ToString()), buttonEntry.Chars[0].Rectangle, Color);
                TextRenderer.DrawTextCentered(SpriteBatch, Caps ? buttonEntry.Chars[1].Char.Item2.ToString() : (Numbers ? buttonEntry.Chars[1].Char.Item3.ToString() : buttonEntry.Chars[1].Char.Item1.ToString()), buttonEntry.Chars[1].Rectangle, Color);
                TextRenderer.DrawTextCentered(SpriteBatch, Caps ? buttonEntry.Chars[2].Char.Item2.ToString() : (Numbers ? buttonEntry.Chars[2].Char.Item3.ToString() : buttonEntry.Chars[2].Char.Item1.ToString()), buttonEntry.Chars[2].Rectangle, Color);
                TextRenderer.DrawTextCentered(SpriteBatch, Caps ? buttonEntry.Chars[3].Char.Item2.ToString() : (Numbers ? buttonEntry.Chars[3].Char.Item3.ToString() : buttonEntry.Chars[3].Char.Item1.ToString()), buttonEntry.Chars[3].Rectangle, Color);
            }

            //SpriteBatch.End();
        }


        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposing)
            {
                if (disposing)
                {
                    if (CharDeleted != null)
                        foreach (var @delegate in CharDeleted.GetInvocationList())
                            CharDeleted -= (EventHandler)@delegate;

                    if (CharReceived != null)
                        foreach (var @delegate in CharReceived.GetInvocationList())
                            CharReceived -= (EventHandler<DaisywheelCharEventArgs>)@delegate;

                    if (InputDone != null)
                        foreach (var @delegate in InputDone.GetInvocationList())
                            InputDone -= (EventHandler<DaisywheelDoneEventArgs>)@delegate;

                    GamePadListener.ButtonDown -= GamePadListener_ButtonDown;
                    GamePadListener.ThumbStickMoved -= GamePadListener_ButtonDown;

                    MainCircleTexture?.Dispose();

                    TextBox.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
