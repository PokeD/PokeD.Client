using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PokeD.CPGL.Components.Input;
using PokeD.CPGL.Screens.UI.Button;
using PokeD.CPGL.Screens.UI.InputBox;

namespace PokeD.CPGL.Screens.UI
{
    /// <summary>
    /// Controlls many GUIItems as one entity.
    /// </summary>
    public sealed class GUIItemMultiController : ScreenUI
    {
        private SpriteBatch SpriteBatch { get; }
        private List<GUIItem> Items { get; set; } = new List<GUIItem>();

        public GUIItemMultiController(Screen screen, SpriteBatch spriteBatch) : base(screen)
        {
            SpriteBatch = spriteBatch; 
            
            KeyboardListener.KeyPressed += (this, KeyboardListener_KeyPressed);
            GamePadListener.ButtonDown += (this, GamePadListener_ButtonDown);
        }
        private void KeyboardListener_KeyPressed(object sender, KeyboardEventArgs keyboardEventArgs)
        {
            switch (keyboardEventArgs.Key)
            {
                case Keys.Up:
                    OnUp();
                    break;

                case Keys.Down:
                    OnDown();
                    break;

                case Keys.Enter:
                    foreach (var guiItem in Items)
                    {
                        if (guiItem.IsSelected)
                        {
                            (guiItem as GUIButton)?.PressButton();
                            (guiItem as GUIInputBox)?.OnEnterPressed();
                        }
                    }
                    break;
            }
        }
        private void GamePadListener_ButtonDown(object sender, GamePadEventArgs e)
        {
            switch (e.Button)
            {
                case Buttons.DPadUp:
                    OnUp();
                    break;

                case Buttons.DPadDown:
                    OnDown();
                    break;

                case Buttons.A:
                    foreach (var guiItem in Items)
                    {
                        if (guiItem.IsSelected)
                        {
                            (guiItem as GUIButton)?.PressButton();
                            (guiItem as GUIInputBox)?.OnEnterPressed();
                        }
                    }
                    break;
            }
        }

        private void OnUp()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var itemCurrent = Items[i];
                if (!itemCurrent.CanBeSelected)
                    continue;

                if (itemCurrent.IsSelected)
                {
                    GUIItem itemPrevious = null;

                    #region Find previous CanBeSelected item

                    for (var j = i - 1; j >= 0; j--)
                    {
                        var item = Items[j];

                        if (item.CanBeSelected)
                        {
                            itemPrevious = item; // Previous
                            break;
                        }
                    }
                    #endregion

                    if (itemPrevious == null)
                        return; // Nothing found

                    itemCurrent.ToActive();

                    itemPrevious.ToSelected();
                    return;
                }
                else if (i == Items.Count - 1)
                {
                    #region Find last CanBeSelected item
                    for (var j = Items.Count - 1; j >= 0; j--)
                    {
                        var item = Items[j];
                        if (item.CanBeSelected)
                        {
                            item.ToSelected();
                            break;
                        }
                    }
                    #endregion

                    return;
                }
            }
        }
        private void OnDown()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var itemCurrent = Items[i];
                if (!itemCurrent.CanBeSelected)
                    continue;

                if (itemCurrent.IsSelected)
                {
                    GUIItem itemNext = null;

                    #region Find next CanBeSelected item
                    for (var j = i + 1; j < Items.Count; j++)
                    {
                        var item = Items[j];
                        if (item.CanBeSelected)
                        {
                            itemNext = item; // Next
                            break;
                        }
                    }
                    #endregion

                    if (itemNext == null)
                        return; // Nothing found

                    itemCurrent.ToActive();

                    itemNext.ToSelected();
                    return;
                }
                else if (i == Items.Count - 1)
                {
                    #region Find first CanBeSelected item
                    for (var j = 0; j < Items.Count; j++)
                    {
                        var item = Items[j];
                        if (item.CanBeSelected)
                        {
                            item.ToSelected();
                            break;
                        }
                    }
                    #endregion
                    /*
                    #region Find last CanBeSelected item
                    for (var j = Items.Count - 1; j >= 0; j--)
                    {
                        var item = Items[j];
                        if (item.CanBeSelected)
                        {
                            item.ToSelected();
                            break;
                        }
                    }
                    #endregion
                    */

                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (var i = Items.Count - 1; i >= 0; i--)
                Items[i].Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap);
            foreach (var guiItem in Items)
                guiItem.Draw(gameTime);
            SpriteBatch.End();
        }

        public void AddGUIItem(GUIItem item) { Items.Add(item); }
        public void AddGUIItems(params GUIItem[] item) { Items.AddRange(item); }

        public void Clear() { Items.Clear(); }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    KeyboardListener.KeyPressed -= KeyboardListener_KeyPressed;
                    GamePadListener.ButtonDown -= GamePadListener_ButtonDown;

                    for (var i = 0; i < Items.Count; i++)
                        Items[i]?.Dispose();
                    Items = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
