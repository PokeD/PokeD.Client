using System;

using Microsoft.Xna.Framework;

using PokeD.CPGL.Components;
using PokeD.CPGL.Components.Debug;
using PokeD.CPGL.Components.ViewportAdapters;

namespace PokeD.CPGL.Screens
{
    public abstract class ScreenUI : DrawableComponent
    {
        protected Screen Screen { get; }
        protected IScreenStyle Style => Screen.Style;
        protected ViewportAdapter ViewportAdapter => Screen.ViewportAdapter;

        protected ScreenUI(Screen screen) : base(screen)
        {
            Screen = screen;

            Screen.EnabledChanged += Screen_EnabledChanged;
            Screen.VisibleChanged += Screen_VisibleChanged;
        }
        private void Screen_EnabledChanged(object sender, EventArgs e) => Enabled = ((Screen) sender).Enabled;
        private void Screen_VisibleChanged(object sender, EventArgs e) => Visible = ((Screen) sender).Visible;

        public override void Draw(GameTime gameTime)
        {
#if DEBUG
            DebugComponent.GUIItemsDrawCalls++;
#endif
        }

        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Screen.EnabledChanged -= Screen_EnabledChanged;
                    Screen.VisibleChanged -= Screen_VisibleChanged;
                }
            }

            base.Dispose(disposing);
        }
    }
}