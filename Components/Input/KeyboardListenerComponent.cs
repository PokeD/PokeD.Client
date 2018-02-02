using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokeD.CPGL.Components.Input
{
    public class KeyboardListenerComponent : InputListenerComponent
    {
        public int InitialDelay { get; }
        public int RepeatDelay { get; }

        public BaseEventHandler<KeyboardEventArgs> KeyTyped = new CustomEventHandler<KeyboardEventArgs>();
        public BaseEventHandler<KeyboardEventArgs> KeyPressed = new CustomEventHandler<KeyboardEventArgs>();
        public BaseEventHandler<KeyboardEventArgs> KeyReleased = new CustomEventHandler<KeyboardEventArgs>();

        private bool _isInitial;
        private TimeSpan _lastPressTime;

        private Keys _previousKey;
        private KeyboardState _previousState;

        public KeyboardListenerComponent(PortableGame game) : this(game, new KeyboardListenerSettings()) { }
        public KeyboardListenerComponent(PortableGame game, KeyboardListenerSettings settings) : base(game)
        {
            InitialDelay = settings.InitialDelayMilliseconds;
            RepeatDelay = settings.RepeatDelayMilliseconds;
        }


        public override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            RaisePressedEvents(gameTime, currentState);
            RaiseReleasedEvents(currentState);
            RaiseRepeatEvents(gameTime, currentState);

            _previousState = currentState;
        }

        private void RaisePressedEvents(GameTime gameTime, KeyboardState currentState)
        {
            if (!currentState.IsKeyDown(Keys.LeftAlt) && !currentState.IsKeyDown(Keys.RightAlt))
            {
                var pressedKeys = Enum.GetValues(typeof(Keys))
                    .Cast<Keys>()
                    .Where(key => currentState.IsKeyDown(key) && _previousState.IsKeyUp(key));

                foreach (var key in pressedKeys)
                {
                    var args = new KeyboardEventArgs(key, currentState);

                    ((BaseEventHandlerWithInvoke<KeyboardEventArgs>) KeyPressed)?.Invoke(this, args);

                    if (args.Character.HasValue)
                        ((BaseEventHandlerWithInvoke<KeyboardEventArgs>) KeyTyped)?.Invoke(this, args);

                    _previousKey = key;
                    _lastPressTime = gameTime.TotalGameTime;
                    _isInitial = true;
                }
            }
        }

        private void RaiseReleasedEvents(KeyboardState currentState)
        {
            var releasedKeys = Enum.GetValues(typeof(Keys))
                .Cast<Keys>()
                .Where(key => currentState.IsKeyUp(key) && _previousState.IsKeyDown(key));

            foreach (var key in releasedKeys)
                ((BaseEventHandlerWithInvoke<KeyboardEventArgs>) KeyReleased)?.Invoke(this, new KeyboardEventArgs(key, currentState));
        }

        private void RaiseRepeatEvents(GameTime gameTime, KeyboardState currentState)
        {
            var elapsedTime = (gameTime.TotalGameTime - _lastPressTime).TotalMilliseconds;

            if (currentState.IsKeyDown(_previousKey) && (_isInitial && elapsedTime > InitialDelay || !_isInitial && elapsedTime > RepeatDelay))
            {
                var args = new KeyboardEventArgs(_previousKey, currentState);

                ((BaseEventHandlerWithInvoke<KeyboardEventArgs>) KeyPressed)?.Invoke(this, args);

                if (args.Character.HasValue)
                    ((BaseEventHandlerWithInvoke<KeyboardEventArgs>) KeyTyped)?.Invoke(this, args);

                _lastPressTime = gameTime.TotalGameTime;
                _isInitial = false;
            }
        }
    }
}