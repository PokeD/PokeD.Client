using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using PokeD.CPGL.Screens;

namespace PokeD.CPGL.Components.Screens
{
    public class ScreenManagerComponent : DrawableComponent
    {
        private IList<Screen> Screens { get; } = new List<Screen>();

        public ScreenManagerComponent(PortableGame game) : base(game) { }


        public void AddScreen(Screen screen)
        {
            AssertNotDisposed();

            if (!Screens.Contains(screen))
                Screens.Add(screen);
        }
        public void RemoveScreen(Screen screen)
        {
            AssertNotDisposed();

            Screens.Remove(screen);
            screen.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            AssertNotDisposed();

            foreach (var screen in Screens.ToList()) // TODO: Check if we should dispose it somehow manually
            {
                if(!screen.Enabled)
                    continue;

                screen.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            AssertNotDisposed();

            foreach (var screen in Screens.ToList()) // TODO: Check if we should dispose it somehow manually
            {
                if (!screen.Visible)
                    continue;

                screen.Draw(gameTime);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    for (var i = 0; i < Screens.Count; i++)
                        (Screens[i] as IDisposable)?.Dispose();
                    Screens.Clear();
                }
            }

            base.Dispose(disposing);
        }
    }
}
