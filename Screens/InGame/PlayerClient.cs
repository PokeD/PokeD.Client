using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PCLExt.FileStorage;
using PCLExt.Network;

using PokeD.CPGL.Components;
using PokeD.CPGL.Components.Camera;
using PokeD.CPGL.Components.ViewportAdapters;
using PokeD.CPGL.Data;
using PokeD.CPGL.Extensions;
using PokeD.CPGL.Screens.UI.GamePad;
using PokeD.CPGL.Storage.Folders;
using PokeD.CPGL.Storage.Folders.GameFolders;
using PokeD.CPGL.Tiled;

using TMXParserPCL;

namespace PokeD.CPGL.Screens.InGame
{
    public enum PlayerScreenIndex
    {
        PlayerOne,

        PlayerOneHalf,
        PlayerTwoHalf,
    }

    public sealed class PlayerScreen : Screen
    {
        private PlayerScreenIndex PlayerScreenIndex { get; }
        private bool HandleInput { get; set; } = true;

        private GameOptionScreen GameOptionScreen { get; set; }
        private MapWrapper World { get; set; }
        private Camera2DComponent Camera { get; }
        private PlayerSprite PlayerSprite { get; }
        private PokeDClient Client { get; }

        private RenderTarget2D ColorRT { get; set; }

        public PlayerScreen(PortableGame game, PlayerScreenIndex playerScreenIndex) : base(game, new PlayerViewportAdapter(game, playerScreenIndex, handler => game.ViewportAdapter.OnResize += handler))
        {
            PlayerScreenIndex = playerScreenIndex;

            Camera = new Camera2DComponent(ViewportAdapter);

            PlayerSprite = new PlayerSprite(Camera, new AnimatedTile(ContentFolder.TextureFolder.GetTextureFile("Player"), 10, new Point(64, 64)), new Rectangle(0, 0, 64, 64));


            Client = new PokeDClient(OnMapLoaded, PlayerScreenIndex.ToString());
            Client.Connect("192.168.1.238");

            Camera.SetPositionChangedAction(Client);

            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);

            GameOptionScreen = new GameOptionScreen(Game, this) { Enabled = false, Visible = false };

            ViewportAdapter.OnResize += (sender, args) => OnResize();
            OnResize();
        }
        private void KeyboardListener_KeyPressed(object sender, Components.Input.KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.D2:
                case Keys.D3:
                    HandleInput = !HandleInput;
                    break;
            }

            if (!HandleInput)
                return;

            switch (e.Key)
            {
                case Keys.Escape:
                    if (!GameOptionScreen.Enabled)
                        SetAtTopScreen(GameOptionScreen);
                    break;
            }
        }
        private void OnMapLoaded(Map map) => World = new MapWrapper(Game, map, 5, PlayerSprite);
        protected override void OnResize()
        {
            base.OnResize();

            ColorRT = new RenderTarget2D(GraphicsDevice, ViewportAdapter.ViewportWidth, ViewportAdapter.ViewportHeight, false, GraphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.Depth24);

            AddGUIItem(new Daisywheel(this));
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;

                    ColorRT?.Dispose();

                    Camera.Dispose();

                    PlayerSprite.Dispose();

                    //Client.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }

    /*
    public sealed class PlayerClient : ScreenGUIComponent
    {
        private PlayerViewportAdapter PlayerViewportAdapter { get; set; }

        private GameOptionScreen GameOptionScreen { get; }
        private MapWrapper World { get; set; }
        private Camera2DComponent Camera { get; }
        private PlayerSprite PlayerSprite { get; }
        private PokeDClient Client { get; }

        private SpriteBatch SpriteBatch { get; }



        public RenderTarget2D ColorRT { get; private set; }
        //Rectangle PlayerRectangle { get; set; }



        bool HandleInput { get; set; } = true;
        PlayerScreenIndex PlayerIndex { get; }


        const string DefaulConnectSettings = "DefaultConnect.json";

        public PlayerClient(GameScreen screen, PlayerScreenIndex player, SpriteBatch spriteBatch, IPPort server) : base(screen)
        {
            PlayerIndex = player;
            SpriteBatch = spriteBatch;

            Game.ViewportAdapter.OnResize += ViewportAdapter_OnResize;

            FillPlayerRenderTarget();


            Camera = new Camera2DComponent(PlayerViewportAdapter); // TODO: Add ViewportAdapter

            PlayerSprite = new PlayerSprite(Camera, new AnimatedTile(screen.ContentFolder.TextureFolder.GetTextureFile("Player"), 10, new Point(64, 64)), new Rectangle(0, 0, 64, 64));
            //PlayerSprite = new PlayerSprite(Camera, new AnimatedTile(screen.ContentManager.Load<Texture2D>(@"Textures/Player").Copy(), 10, new Point(64, 64)), new Rectangle(0, 0, 64, 64));
            //PlayerSprite = new PlayerSprite(Camera,
            //    new AnimatedTile(Textures.GetFileAsync("Player.png").Result.OpenAsync(FileAccess.Read).Result,
            //    10, GraphicsDevice, new Point(64, 64)), new Rectangle(0, 0, 64, 64));


            Client = new PokeDClient(OnMapLoaded, PlayerIndex.ToString());
            Client.Connect("127.0.0.1");

            Camera.SetPositionChangedAction(Client);

            KeyboardListener.KeyPressed += KeyboardListener_KeyPressed;

            GameOptionScreen = new GameOptionScreen(Game, screen)
            {
                Enabled = false,
                Visible = false
            };
            Game.ScreenManager.AddScreen(GameOptionScreen);
        }
        private void ViewportAdapter_OnResize(object sender, System.EventArgs e) => FillPlayerRenderTarget();
        private void KeyboardListener_KeyPressed(object sender, Components.Input.KeyboardEventArgs e)
        {
            if(!Enabled)
                return;

            switch (e.Key)
            {
                case Keys.D2:
                case Keys.D3:
                    HandleInput = !HandleInput;
                    break;
            }

            if (!HandleInput)
                return;

            switch (e.Key)
            {
                case Keys.Escape:
                    Sw
                    //Game.ScreenManager.AddScreen(new GameOptionScreen(Game, Screen as GameScreen));
                    break;
            }
        }
        private void OnMapLoaded(Map map) => World = new MapWrapper(Game, map, 5, PlayerSprite);

        private void FillPlayerRenderTarget()
        {
            var fullWidth = (int) (ViewportAdapter.ViewportWidth * 1.0f);
            var fullHeight = (int) (ViewportAdapter.ViewportHeight * 1.0f);
            var halfWidth = (int) (ViewportAdapter.ViewportWidth * 0.5f);
            var halfHeight = (int) (ViewportAdapter.ViewportHeight * 0.5f);

            switch (PlayerIndex)
            {
                case PlayerIndex.PlayerOne:
                    PlayerViewportAdapter = new PlayerViewportAdapter(Game, new Rectangle(0, 0, fullWidth, fullHeight), handler => Game.ViewportAdapter.OnResize += handler);
                    break;


                case PlayerIndex.PlayerOneHalf:
                    PlayerViewportAdapter = new PlayerViewportAdapter(Game, new Rectangle(0, 0, fullWidth, halfHeight), handler => Game.ViewportAdapter.OnResize += handler);
                    break;

                case PlayerIndex.PlayerTwoHalf:
                    PlayerViewportAdapter = new PlayerViewportAdapter(Game, new Rectangle(0, halfHeight, fullWidth, halfHeight), handler => Game.ViewportAdapter.OnResize += handler);
                    break;
            }

            ColorRT = new RenderTarget2D(GraphicsDevice, PlayerViewportAdapter.ViewportWidth, PlayerViewportAdapter.ViewportHeight, false, GraphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.Depth24);
        }

        public override void Update(GameTime gameTime)
        {
            if (!HandleInput)
                return;

            Client.Update();

            Camera.Update(gameTime);
            World?.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            var preDepthStencilState = GraphicsDevice.DepthStencilState;

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GraphicsDevice.SetRenderTarget(ColorRT);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            World?.Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);

            //SpriteBatch.Draw(ColorRT, Vector2.Zero, Color.White);
            //SpriteBatch.Draw(ColorRT, PlayerRectangle, Color.White);
            SpriteBatch.Draw(ColorRT, new Vector2(PlayerViewportAdapter.X, PlayerViewportAdapter.Y), Color.White);

            GraphicsDevice.DepthStencilState = preDepthStencilState;


            base.Draw(gameTime);
        }



        protected override void Dispose(bool disposing)
        {
            Game.ViewportAdapter.OnResize -= ViewportAdapter_OnResize;
            KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;

            //_cancellationToken?.Cancel();

            //Camera?.Dispose();

            //Client?.Dispose();

            base.Dispose(disposing);
        }
    }
    */
}
