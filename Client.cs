using System;

using Microsoft.Xna.Framework;

namespace PokeD.Client
{
    public sealed class Client : Game
    {
        private GraphicsDeviceManager Graphics { get; set; }


        public Client(Action<Game> platformCode, bool fullscreen = false)
        {
            Graphics = new GraphicsDeviceManager(this);
            //Graphics.SynchronizeWithVerticalRetrace = false;
            Graphics.IsFullScreen = fullscreen;
            Graphics.ApplyChanges();

            //IsFixedTimeStep = false;
            //TargetElapsedTime = new TimeSpan((long)(1000f / 60f * TimeSpan.TicksPerMillisecond));

            Content.RootDirectory = "Content";

            platformCode?.Invoke(this);
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
