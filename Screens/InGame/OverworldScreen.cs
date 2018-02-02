using Microsoft.Xna.Framework;

using PCLExt.FileStorage;

using PokeD.CPGL.Components;
using PokeD.CPGL.Data;
using PokeD.CPGL.Storage.Folders;
using PokeD.CPGL.Storage.Folders.GameFolders;
using PokeD.CPGL.Tiled;

using TMXParserPCL;

namespace PokeD.CPGL.Screens.InGame
{
    public sealed class OverworldScreen : Screen
    {
        private static readonly IFolder Textures = new OldContentFolder().GetFolder("Textures");

        private MapWrapper World { get; set; }
        private PlayerSprite PlayerSprite { get; }
        private PokeDClient Client { get; }


        /*
        private static QuadRenderer QuadRenderer { get; set; }
        public RenderTarget2D RenderTargetMinScale { get; set; }
        public RenderTarget2D ScreenRenderTarget { get; set; }
        public RenderTarget2D RenderTargetMaxScale { get; set; }
        //public ScaleHQx4Effect Effect { get; set; }
        //public BRx5Effect Effect { get; set; }
        public BRx5Effect Effect { get; set; }
        */

        public OverworldScreen(PortableGame game) : base(game)
        {
            Game.IsMouseVisible = false;

            Camera.MoveTo(new Vector2(9 * 16, 13 * 16) * 4, 0.0f);

            //Camera = new Camera2DComponent(game, new Vector2(9 * 16, 13 * 16) * 4, 0.0f, 200.0f, new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height));

            PlayerSprite = new PlayerSprite(Camera, new AnimatedTile(Textures.GetFile("Player.png").Open(FileAccess.Read), 10, GraphicsDevice, new Point(64, 64)), new Rectangle(0, 0, 64, 64));
            

            Client = new PokeDClient(OnMapLoaded, "OLOLO");
            //Client.Connect("93.80.161.99");
            //Client.Connect("192.168.1.53");
            Client.Connect("127.0.0.1");

            Camera.SetPositionChangedAction(Client);

            /*
            QuadRenderer = new QuadRenderer(Game);
            Effect = BRx5Effect.Load(Game.Content);
            */
        }
        private void OnMapLoaded(Map map) => World = new MapWrapper(Game, map, 4, PlayerSprite);

        protected override void OnResize()
        {
            base.OnResize();

            //Camera.OnResize(new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height));

            //RenderTargetSmall?.Dispose();
            //RenderTargetSmall = new RenderTarget2D(GraphicsDevice, (int) (GraphicsDevice.Viewport.Bounds.Width * 0.5f), (int) (GraphicsDevice.Viewport.Bounds.Height * 0.5f),
            //    false, GraphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.None);

            /*
            RenderTargetMinScale?.Dispose();
            RenderTargetMinScale = new RenderTarget2D(GraphicsDevice, (int) (GraphicsDevice.Viewport.Bounds.Width / 4), (int) (GraphicsDevice.Viewport.Bounds.Height / 4),
                false, GraphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.None);

            ScreenRenderTarget?.Dispose();
            ScreenRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Bounds.Width, GraphicsDevice.Viewport.Bounds.Height,
                false, GraphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.None);

            RenderTargetMaxScale?.Dispose();
            RenderTargetMaxScale = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Bounds.Width * 4, GraphicsDevice.Viewport.Bounds.Height * 4,
                false, GraphicsDevice.Adapter.CurrentDisplayMode.Format, DepthFormat.None);
            */
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Client.Update();

            Camera.Update(gameTime);
            World?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            /*
            GraphicsDevice.SetRenderTarget(ScreenRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.GetViewMatrix());
            World?.Draw(gameTime);
            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.SetRenderTarget(RenderTargetMinScale);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            SpriteBatch.Draw(ScreenRenderTarget, new Rectangle(0, 0, RenderTargetMinScale.Width, RenderTargetMinScale.Height), Color.White);
            //SpriteBatch.Draw(ScreenRenderTarget, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            
            Effect.TextureSize = new Vector2(ScreenRenderTarget.Width, ScreenRenderTarget.Height);
            GraphicsDevice.SetRenderTarget(RenderTargetMaxScale);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            SpriteBatch.Draw(ScreenRenderTarget, new Rectangle(0, 0, RenderTargetMaxScale.Width, RenderTargetMaxScale.Height), Color.White);
            //SpriteBatch.Draw(ScreenRenderTarget, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 4f, SpriteEffects.None, 0.0f);
            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, Effect, null);
            SpriteBatch.Draw(RenderTargetMaxScale, new Rectangle(0, 0, ScreenRenderTarget.Width, ScreenRenderTarget.Height), Color.White);
            SpriteBatch.End();
            //*/

            /*
            Effect.TextureSize = new Vector2(RenderTargetSmall.Width, RenderTargetSmall.Height);
            GraphicsDevice.SetRenderTarget(RenderTarget);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, Effect, null);
            SpriteBatch.Draw(RenderTargetSmall, new Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height), Color.White);
            SpriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
            SpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, RenderTarget.Width, RenderTarget.Height), Color.White);
            SpriteBatch.End();
            */

            //SpriteBatch.Begin();
            //SpriteBatch.Draw(RenderTargetScaled, Vector2.Zero, Color.White);
            //SpriteBatch.End();

            //GraphicsDevice.SetRenderTarget(RenderTarget1);
            //Effect.TextureSize = new Vector2(RenderTarget.Width, RenderTarget.Height);
            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, Effect, null);
            //SpriteBatch.Draw(RenderTargetScaled, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            //SpriteBatch.End();
            //GraphicsDevice.SetRenderTarget(null);



            World?.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
