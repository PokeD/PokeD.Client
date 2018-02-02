using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PokeD.CPGL.Components.ViewportAdapters;

namespace PokeD.CPGL.Components.Input
{
    public class MouseListenerComponent : InputListenerComponent
    {
        public ViewportAdapter ViewportAdapter { get; }

        public int DoubleClickMilliseconds { get; }
        public int DragThreshold { get; }

        public bool HasMouseMoved => (_previousState.X != _currentState.X) || (_previousState.Y != _currentState.Y);

        public BaseEventHandler<MouseEventArgs> MouseDown = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseUp = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseClicked = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseDoubleClicked = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseMoved = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseWheelMoved = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseDragStart = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseDrag = new CustomEventHandler<MouseEventArgs>();
        public BaseEventHandler<MouseEventArgs> MouseDragEnd = new CustomEventHandler<MouseEventArgs>();

        private MouseState _currentState;
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private MouseEventArgs _mouseDownArgs;
        private MouseEventArgs _previousClickArgs;
        private MouseState _previousState;

        public MouseListenerComponent(PortableGame game) : this(game, new MouseListenerSettings()) { }
        public MouseListenerComponent(PortableGame game, ViewportAdapter viewportAdapter) : this(game, new MouseListenerSettings()) { ViewportAdapter = viewportAdapter; }
        public MouseListenerComponent(PortableGame game, MouseListenerSettings settings) : base(game)
        {
            ViewportAdapter = settings.ViewportAdapter;
            DoubleClickMilliseconds = settings.DoubleClickMilliseconds;
            DragThreshold = settings.DragThreshold;
        }

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Pressed) && (getButtonState(_previousState) == ButtonState.Released))
            {
                var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

                ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseDown)?.Invoke(this, args);
                _mouseDownArgs = args;

                if (_previousClickArgs != null)
                {
                    // If the last click was recent
                    var clickMilliseconds = (args.Time - _previousClickArgs.Time).TotalMilliseconds;

                    if (clickMilliseconds <= DoubleClickMilliseconds)
                    {
                        ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseDoubleClicked)?.Invoke(this, args);
                        _hasDoubleClicked = true;
                    }

                    _previousClickArgs = null;
                }
            }
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Released) && (getButtonState(_previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                    // If the mouse hasn't moved much between mouse down and mouse up
                    if (clickMovement < DragThreshold)
                    {
                        if (!_hasDoubleClicked)
                            ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseClicked)?.Invoke(this, args);
                    }
                    else // If the mouse has moved between mouse down and mouse up
                    {
                        ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseDragEnd)?.Invoke(this, args);
                        _dragging = false;
                    }
                }

                ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseUp)?.Invoke(this, args);

                _hasDoubleClicked = false;
                _previousClickArgs = args;
            }
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) == ButtonState.Pressed) && (getButtonState(_previousState) == ButtonState.Pressed))
            {
                var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

                if (_mouseDownArgs.Button == args.Button)
                {
                    if (_dragging)
                        ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseDrag)?.Invoke(this, args);
                    else
                    {
                        // Only start to drag based on DragThreshold
                        var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                        if (clickMovement > DragThreshold)
                        {
                            _dragging = true;
                            ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseDragStart)?.Invoke(this, args);
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _currentState = Mouse.GetState();

            CheckButtonPressed(s => s.LeftButton, MouseButton.Left);
            CheckButtonPressed(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonPressed(s => s.RightButton, MouseButton.Right);
            CheckButtonPressed(s => s.XButton1, MouseButton.XButton1);
            CheckButtonPressed(s => s.XButton2, MouseButton.XButton2);

            CheckButtonReleased(s => s.LeftButton, MouseButton.Left);
            CheckButtonReleased(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonReleased(s => s.RightButton, MouseButton.Right);
            CheckButtonReleased(s => s.XButton1, MouseButton.XButton1);
            CheckButtonReleased(s => s.XButton2, MouseButton.XButton2);

            // Check for any sort of mouse movement.
            if (HasMouseMoved)
            {
                ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseMoved)?.Invoke(this, new MouseEventArgs(ViewportAdapter, gameTime.TotalGameTime, _previousState, _currentState));

                CheckMouseDragged(s => s.LeftButton, MouseButton.Left);
                CheckMouseDragged(s => s.MiddleButton, MouseButton.Middle);
                CheckMouseDragged(s => s.RightButton, MouseButton.Right);
                CheckMouseDragged(s => s.XButton1, MouseButton.XButton1);
                CheckMouseDragged(s => s.XButton2, MouseButton.XButton2);
            }

            // Handle mouse wheel events.
            if (_previousState.ScrollWheelValue != _currentState.ScrollWheelValue)
                ((BaseEventHandlerWithInvoke<MouseEventArgs>) MouseWheelMoved)?.Invoke(this, new MouseEventArgs(ViewportAdapter, gameTime.TotalGameTime, _previousState, _currentState));

            _previousState = _currentState;
        }

        private static int DistanceBetween(Point a, Point b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}