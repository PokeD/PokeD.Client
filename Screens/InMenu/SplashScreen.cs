using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PCLExt.Thread;

using PokeD.CPGL.Extensions;
using PokeD.CPGL.Screens.UI.Image;
using PokeD.CPGL.Screens.UI.Text;

namespace PokeD.CPGL.Screens.InMenu
{
    public sealed class SplashScreen : ScreenMenu
    {
        private double DelayTime { get; set; }
        private Func<Screen> ScreenToLoad { get; }
        private DateTime CreationTime { get; } = DateTime.UtcNow;

        private IThread _loadThread;
        private bool _startedLoad;
        private Screen _screen;



        public SplashScreen(PortableGame game, Func<Screen> screenToLoad, double delayTime = 2.5d) : base(game)
        {
            ScreenToLoad = screenToLoad;
            DelayTime = delayTime;

            OnResize();


            Game.IsMouseVisible = false;
        }

        protected override void OnResize()
        {
            base.OnResize();

            var backgroundImage = new BackgroundImage(this, new Rectangle(
                0,
                0,
                ViewportAdapter.ViewportWidth,
                ViewportAdapter.ViewportHeight
                ), Color.Black);

            var logoTexture = ((Texture2D) ContentFolder.TextureFolder.GetTextureFile("Logo_256")).Copy();
            //var logoTexture = ContentManager.Load<Texture2D>("Textures/Logo_256").Copy();
            var logoGrid = new BaseImage(this, new Rectangle(
                    ViewportAdapter.Center.X - logoTexture.Bounds.Center.X,
                    ViewportAdapter.Center.Y - logoTexture.Bounds.Center.Y,
                    logoTexture.Width,
                    logoTexture.Height
                    ), logoTexture);

            var text = new BaseCenteredText(this, "PokeD is not affiliated with Nintendo, Creatures Inc. or GAME FREAK Inc.", new Rectangle(
                0,
                ViewportAdapter.ViewportHeight - ViewportAdapter.ViewportHeight / 8,
                ViewportAdapter.ViewportWidth,
                Style.FontNormalSize
                ), Color.White);


            AddGUIItem(backgroundImage);
            AddGUIItem(logoGrid);
            AddGUIItem(text);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*
            var dt = gameTime.ElapsedGameTime.TotalSeconds * 60D;
            if (!_startedLoad)
            {
                _loadThread = Thread.Create(() => _screen = ScreenToLoad());
                _loadThread.Start();
                _startedLoad = true;
            }
            if (!_loadThread.IsRunning)
            {
                if (DelayTime < 0.1D)
                {
                    CloseScreen();
                    SwitchScreen(_screen);
                }
            }
            DelayTime -= 0.1D * dt;
            */

            if (DateTime.UtcNow - CreationTime < TimeSpan.FromSeconds(DelayTime))
                return;
            else
            {
                AddScreen(ScreenToLoad());
                CloseScreen();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    
                }

                _loadThread?.Abort();
            }

            base.Dispose(disposing);
        }
    }
}
