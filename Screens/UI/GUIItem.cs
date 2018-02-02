using Microsoft.Xna.Framework.Graphics;
using PokeD.CPGL.BMFont;

namespace PokeD.CPGL.Screens.UI
{
    public abstract class GUIItem : ScreenUI
    {
        protected SpriteBatch SpriteBatch => Screen.SpriteBatch;

        protected IFontRenderer TextRenderer { get; }
        //protected static Effect SpriteBatchEffect { get; }

        public bool CanBeSelected { get; }

        #region ItemState

        private enum GUIItemState
        {
            Active,
            JustNowActive,
            Selected,
            SelectedMouseHover,
            NonPressable,
            Hidden
        }

        private GUIItemState State { get; set; }

        public bool IsActive => State == GUIItemState.Active;
        //protected internal bool IsJustNowActive => State == GUIItemState.JustNowActive;
        public bool IsSelected => State == GUIItemState.Selected;
        public bool IsSelectedMouseHover => State == GUIItemState.SelectedMouseHover;
        public bool IsNonPressable => State == GUIItemState.NonPressable;
        public bool IsHidden => State == GUIItemState.Hidden;

        public void ToActive()
        {
            //State = IsJustNowActive ? GUIItemState.Active : GUIItemState.JustNowActive;
            State = GUIItemState.Active;
            Enabled = true;
            Visible = true;
        }
        public void ToSelected()
        {
            State = GUIItemState.Selected;
            Enabled = true;
            Visible = true;
        }
        public void ToSelectedMouseHover()
        {
            State = GUIItemState.SelectedMouseHover;
            Enabled = true;
            Visible = true;
        }
        public void ToNonPressable()
        {
            State = GUIItemState.NonPressable;
            Enabled = true;
            Visible = true;
        }
        public void ToHidden()
        {
            State = GUIItemState.Hidden;
            Enabled = false;
            Visible = false;
        }

        #endregion ItemState

        protected GUIItem(Screen screen, bool canBeSelected) : base(screen)
        {
            if (TextRenderer == null)
            {
                TextRenderer = new FontScalableRenderer(Screen.ContentFolder.FontFolder.GetFontFile("PixelUnicode_64"));
                //TextRenderer = new FontRenderer(Screen.ContentFolder.FontFolder.GetFontFiles("PixelUnicode"));
                //TextRenderer = new FontRenderer(GraphicsDevice, "PixelUnicode", 16, 32, 48);
            }

            CanBeSelected = canBeSelected;
        }

        /// <summary>Shuts down the component.</summary>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    ToHidden();
                }
            }

            base.Dispose(disposing);
        }
    }
}
