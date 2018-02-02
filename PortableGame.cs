using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using PCLExt.FileStorage;
using PCLExt.Network;
using PokeD.CPGL.Components;
using PokeD.CPGL.Components.Camera;
using PokeD.CPGL.Components.Debug;
using PokeD.CPGL.Components.Input;
using PokeD.CPGL.Components.Screens;
using PokeD.CPGL.Components.ViewportAdapters;
using PokeD.CPGL.Data;
using PokeD.CPGL.Screens.InGame;
using PokeD.CPGL.Screens.InMenu;
using PokeD.CPGL.Storage.Folders.GameFolders;
using PokeD.CPGL.Tiled;

using TMXParserPCL;

namespace PokeD.CPGL
{
    public sealed class PortableGame : Game
    {
        public static Point DefaultResolution => new Point(800, 640);

        public ViewportAdapter ViewportAdapter { get; }
        public ScreenManagerComponent ScreenManager { get; private set; }
        public Camera2DComponent Camera { get; private set; }

        public KeyboardListenerComponent KeyboardListener { get; private set; }
        public MouseListenerComponent MouseListener { get; private set; }
        public GamePadListenerComponent GamePadListener { get; private set; }
        public TouchListenerComponent TouchListener { get; private set; }

        private GraphicsDeviceManager Graphics { get; }

        private event EventHandler<EventArgs> WindowSizeChanged;

        public PortableGame(bool borderless = false, bool fullscreen = false)
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = fullscreen,
                SynchronizeWithVerticalRetrace = false,
                SupportedOrientations = DisplayOrientation.Default
            };
            Graphics.ApplyChanges();

            ViewportAdapter = new DefaultViewportAdapter(this, handler => WindowSizeChanged += handler);
            ViewportAdapter.OnResize += (sender, args) =>
            {
                if (Graphics.GraphicsDevice.Viewport.Width < DefaultResolution.X || Graphics.GraphicsDevice.Viewport.Height < DefaultResolution.Y)
                    ResizeBackBuffer(DefaultResolution);
            };
            Window.IsBorderless = borderless;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += WindowSizeChanged;
            //Window.Position = new Point(0, 0);

            IsMouseVisible = true;

            Resize(DefaultResolution);
        }

        protected override void Initialize()
        {
            KeyboardListener = new KeyboardListenerComponent(this);
            MouseListener = new MouseListenerComponent(this, ViewportAdapter);
            GamePadListener = new GamePadListenerComponent(this);
            TouchListener = new TouchListenerComponent(this, ViewportAdapter);

            Camera = new Camera2DComponent(ViewportAdapter);
            Components.Add(Camera);

            ScreenManager = new ScreenManagerComponent(this);
            Components.Add(ScreenManager);


            base.Initialize();


#if DEBUG
            Components.Add(new DebugComponent(this));
#endif

            // Input components should be disposed at the very end.
            Components.Add(KeyboardListener);
            Components.Add(MouseListener);
            Components.Add(GamePadListener);
            Components.Add(TouchListener);
        }

        protected override void LoadContent()
        {
            /*
            using (var stream = FileSystem.GetFileFromPath(@"E:\GitHub\PokeD Project\PokeD Client\bin\Debug\Content\Maps\0.0.tmx").Open(FileAccess.Read))
            {
                Camera.MoveTo(new Vector2(1000 * 4, 800 * 4), 0f);
                var ps = new PlayerSprite(Camera, new AnimatedTile(new ContentFolder(new ContentManager(Services)).TextureFolder.GetTextureFile(@"Player"), 10, new Point(64, 64)), new Rectangle(0, 0, 64, 64));
                var map = new MapWrapper(this, Map.Load(stream), 4, ps);
                Components.Add(map);
            }
            */

            //ScreenManager.AddScreen(new OverworldScreen(this));
            //ScreenManager.AddScreen(new BattleScreen(this, new Player("Shit", Gender.Male, new MonsterTeam() { List = { new Monster(1), new Monster(2), new Monster(3), new Monster(4), new Monster(5), new Monster(6) } }), null));
            //ScreenManager.AddScreen(new SplashScreen(this, () => new MainMenuScreen(this)));
            ScreenManager.AddScreen(new SplashScreen(this, () => new PlayerScreen(this, PlayerScreenIndex.PlayerOne)));
            //ScreenManager.AddScreen(new GameScreen(this, new IPPort("127.0.0.1", 15130)));

            base.LoadContent();
        }

        public void Resize(Point size)
        {
            ResizeBackBuffer(size);
            WindowSizeChanged(null, EventArgs.Empty);
        }
        private void ResizeBackBuffer(Point size)
        {
            if (size.X < DefaultResolution.X || size.Y < DefaultResolution.Y)
                return;

            Graphics.PreferredBackBufferWidth = size.X;
            Graphics.PreferredBackBufferHeight = size.Y;

            Graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            //if(!IsActive)
            //    return;

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive)
                return;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (WindowSizeChanged != null)
                foreach (var @delegate in WindowSizeChanged.GetInvocationList())
                    WindowSizeChanged -= (EventHandler<EventArgs>) @delegate;

            ViewportAdapter.Dispose();

            base.Dispose(disposing);
        }
    }
}