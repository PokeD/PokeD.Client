using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PCLExt.Network;

namespace PokeD.CPGL.Screens.InGame
{
    public sealed class GameScreen : Screen
    {
        //private Player Player { get; }
        private readonly List<PlayerScreen> _clients = new List<PlayerScreen>();
        private readonly List<Screen> _screens = new List<Screen>();


        public GameScreen(PortableGame game, IPPort server) : base(game)
        {
            _clients.Add(new PlayerScreen(game, PlayerScreenIndex.PlayerOne));
            AddScreen(_clients[0]);

            //_clients.Add(new PlayerClient(this, PlayerScreenIndex.PlayerOneHalf, SpriteBatch, server));
            //_clients.Add(new PlayerClient(this, PlayerScreenIndex.PlayerTwoHalf, SpriteBatch, server));

            Game.IsMouseVisible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //foreach (var client in _clients)
            //    client.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            //foreach (var client in _clients)
            //    client.Draw(gameTime);
            //SpriteBatch.End();

            base.Draw(gameTime);
        }


        protected override void Dispose(bool disposing)
        {
            foreach (var client in _clients)
                client.Dispose();

            base.Dispose(disposing);
        }
    }
}
