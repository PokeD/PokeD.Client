using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using PokeD.CPGL.Components;
using PokeD.CPGL.Components.Screens;
using PokeD.CPGL.Components.ViewportAdapters;
using PokeD.CPGL.Screens.UI;
using PokeD.CPGL.Screens.UI.GamePad;
using PokeD.CPGL.Storage.Folders.GameFolders;

namespace PokeD.CPGL.Screens
{
    public struct GUIItemSize
    {
        public int Width { get; }
        public int Height { get; }

        public Point Center { get; }

        public GUIItemSize(int value)
        {
            Width = value;
            Height = value;
            Center = new Point((int) (Width * 0.5f), (int) (Height * 0.5f));
        }
        public GUIItemSize(int width, int height)
        {
            Width = width;
            Height = height;
            Center = new Point((int) (Width * 0.5f), (int) (Height * 0.5f));
        }

        public GUIItemSize Scale(float value) => new GUIItemSize((int) (Width * value), (int) (Height * value));

        public GUIItemSize ScaleN(float value) => new GUIItemSize((int) (Width * value + (value - 1) * 35), (int) (Height * value));
    }

    public interface IScreenStyle
    {
        float ResolutionScale { get; }

        int FontSmallSize { get; }
        int FontNormalSize { get; }
        int FontBigSize { get; }

        GUIItemSize BoxSize { get; }
        GUIItemSize ButtonSize { get; }
        GUIItemSize ButtonHalfSize { get; }

        Color MainBackgroundColor { get; }
        Color SecondaryBackgroundColor { get; }

        void OnResize();
    }
    public sealed class DefaultScreenStyle : IScreenStyle
    {
        private static Vector2 DefaultResolution { get; } = PortableGame.DefaultResolution.ToVector2();

        private static int DefaultFontSmallSize { get; } = 16;
        private static int DefaultFontNormalSize { get; } = 32;
        private static int DefaultFontBigSize { get; } = 48;

        private static GUIItemSize DefaultBoxSize { get; } = new GUIItemSize(260, 250);
        private static GUIItemSize DefaultButtonSize { get; } = new GUIItemSize(400, 40);
        private static GUIItemSize DefaultButtonHalfSize { get; } = new GUIItemSize(200, 40);

        private ViewportAdapter ViewportAdapter { get; }

        private Vector2 CurrentResolution { get; set; }
        private float ResolutionScaleX { get; set; }
        private float ResolutionScaleY { get; set; }
        public float ResolutionScale { get; private set; }

        public int FontSmallSize { get; private set; }
        public int FontNormalSize { get; private set; }
        public int FontBigSize { get; private set; }

        public GUIItemSize BoxSize { get; private set; }
        public GUIItemSize ButtonSize { get; private set; }
        public GUIItemSize ButtonHalfSize { get; private set; }

        public Color MainBackgroundColor { get; } = new Color(30, 30, 30, 255);
        public Color SecondaryBackgroundColor { get; } = new Color(75, 75, 75, 255);

        public DefaultScreenStyle(ViewportAdapter viewportAdapter) { ViewportAdapter = viewportAdapter; OnResize(); }

        public void OnResize()
        {
            CurrentResolution = new Vector2(ViewportAdapter.ViewportWidth, ViewportAdapter.ViewportHeight);

            ResolutionScaleX = CurrentResolution.X / DefaultResolution.X;
            ResolutionScaleY = CurrentResolution.Y / DefaultResolution.Y;
            ResolutionScale = Math.Min(ResolutionScaleX, ResolutionScaleY);
            
            BoxSize = DefaultBoxSize.ScaleN(ResolutionScale);
            ButtonSize = DefaultButtonSize.Scale(ResolutionScale);
            ButtonHalfSize = DefaultButtonHalfSize.Scale(ResolutionScale);
            
            FontSmallSize = (int) (DefaultFontSmallSize * ResolutionScale);
            FontNormalSize = (int) (DefaultFontNormalSize * ResolutionScale);
            FontBigSize = (int) (DefaultFontBigSize * ResolutionScale);
        }
    }

    public abstract class Screen : DrawableComponent, IEqualityComparer<Screen>
    {
        private Guid ID { get; }

        public SpriteBatch SpriteBatch { get; private set; }

        private Daisywheel _daisywheel;
        public Daisywheel GetDaisywheel(string initialText = "")
        {
            if (_daisywheel != null && _daisywheel.Enabled && _daisywheel.Visible)
                return _daisywheel;

            _daisywheel = new Daisywheel(this, initialText);
            return _daisywheel;
        }
        //private Daisywheel _daisywheel;
        //public Daisywheel Daisywheel => _daisywheel != null && _daisywheel.Enabled && _daisywheel.Visible ? _daisywheel : (_daisywheel = new Daisywheel(this));

        public ContentFolder ContentFolder { get; private set; }

        private GUIItemMultiController GUIItemMultiController { get; set; }

        public IScreenStyle Style { get; }

        public ViewportAdapter ViewportAdapter { get; }

        /*
        #region ScreenState

        private enum ScreenState
        {
            Active = 0,
            Background = 1,
            Hidden = 2,
            JustNowActive = 3
        }
        
        private ScreenState State { get; set; }

        public bool IsActive => State == ScreenState.Active;
        //protected internal bool IsJustNowActive => State == ScreenState.JustNowActive;
        public bool IsBackground => State == ScreenState.Background;
        public bool IsHidden => State == ScreenState.Hidden;

        public void ToActive()
        {
            State = ScreenState.Active;
            //State = IsJustNowActive ? ScreenState.Active : ScreenState.JustNowActive;
            Enabled = true;
            Visible = true;
        }
        public void ToBackground()
        {
            State = ScreenState.Background;
            Enabled = false;
            Visible = true;
        }
        public void ToHidden()
        {
            State = ScreenState.Hidden;
            Enabled = false;
            Visible = false;
        }

        #endregion ScreenState
        */

        #region ScreenManager

        private ScreenManagerComponent ScreenManager => Game.ScreenManager;

        //protected void AddScreenAndHideThis(Screen screen) { AssertNotDisposed(); ScreenManager.AddScreen(screen); Enabled = false; Visible = false; /* ToHidden(); */ }
        //protected void AddScreenAndCloseThis(Screen screen) { AssertNotDisposed(); CloseScreen(); ScreenManager.AddScreen(screen); }
        //protected void AddScreenAndCloseOthers(Screen screen) { AssertNotDisposed(); ScreenManager.AddScreen(screen); ScreenManager.CloseOtherScreens(screen); }

        protected void AddScreen(Screen screen) { AssertNotDisposed(); ScreenManager.AddScreen(screen); }
        protected void SetAtTopScreen(Screen screen)
        {
            AssertNotDisposed();

            AddScreen(screen);
            screen.Enabled = true;
            screen.Visible = true;
        }
        protected void SwitchScreen(Screen screen)
        {
            AssertNotDisposed();

            Enabled = false;
            Visible = false;

            AddScreen(screen);
            screen.Enabled = true;
            screen.Visible = true;
        }
        protected void CloseScreen()
        {
            AssertNotDisposed();

            Enabled = false;
            Visible = false;
            ScreenManager.RemoveScreen(this);
        }

        protected void Exit() { Game.Exit(); }

        #endregion ScreenManager

        protected Screen(PortableGame game, ViewportAdapter viewportAdapter, IScreenStyle style) : base(game)
        {
            ID = Guid.NewGuid();

            if (SpriteBatch == null)
                SpriteBatch = new SpriteBatch(GraphicsDevice);

            ContentFolder = new ContentFolder(new ContentManager(Game.Services));

            GUIItemMultiController = new GUIItemMultiController(this, SpriteBatch);

            Style = style;

            ViewportAdapter = viewportAdapter;
            ViewportAdapter.OnResize += ViewportAdapter_OnResize;
        }
        protected Screen(PortableGame game, ViewportAdapter viewportAdapter) : this(game, viewportAdapter, new DefaultScreenStyle(viewportAdapter)) { }
        protected Screen(PortableGame game) : this(game, game.ViewportAdapter) { }
        private void ViewportAdapter_OnResize(object sender, EventArgs e) { AssertNotDisposed(); OnResize(); }

        protected void AddGUIItem(GUIItem item) { AssertNotDisposed(); GUIItemMultiController.AddGUIItem(item); }
        protected void AddGUIItems(params GUIItem[] items) { GUIItemMultiController.AddGUIItems(items); }

        protected virtual void OnResize()
        {
            GUIItemMultiController.Clear();

            Style.OnResize();
        }

        public override void Update(GameTime gameTime) { AssertNotDisposed(); GUIItemMultiController.Update(gameTime); _daisywheel?.Update(gameTime); }
        public override void Draw(GameTime gameTime) { AssertNotDisposed(); GUIItemMultiController.Draw(gameTime); _daisywheel?.Draw(gameTime); }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Enabled = false;
                    Visible = false;

                    ViewportAdapter.OnResize -= ViewportAdapter_OnResize;

                    SpriteBatch.Dispose();
                    SpriteBatch = null;

                    _daisywheel?.Dispose();
                    _daisywheel = null;

                    GUIItemMultiController.Dispose();
                    GUIItemMultiController = null;

                    ContentFolder.Dispose();
                    ContentFolder = null;
                }
            }

            base.Dispose(disposing);
        }


        public override bool Equals(object obj)
        {
            var screen = obj as Screen;
            return !ReferenceEquals(screen, null) && Equals(this, screen);
        }
        public override int GetHashCode() => ID.GetHashCode();

        public bool Equals(Screen x, Screen y) => !ReferenceEquals(x, null) && !ReferenceEquals(y, null) && x.ID.Equals(y.ID);
        public int GetHashCode(Screen obj) => obj.GetHashCode();
    }
}
