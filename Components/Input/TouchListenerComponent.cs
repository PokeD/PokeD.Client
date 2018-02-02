using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

using PokeD.CPGL.Components.ViewportAdapters;

namespace PokeD.CPGL.Components.Input
{
    public class TouchListenerComponent : InputListenerComponent
    {
        public ViewportAdapter ViewportAdapter { get; set; }

        public BaseEventHandler<TouchEventArgs> TouchStarted = new CustomEventHandler<TouchEventArgs>();
        public BaseEventHandler<TouchEventArgs> TouchEnded = new CustomEventHandler<TouchEventArgs>();
        public BaseEventHandler<TouchEventArgs> TouchMoved = new CustomEventHandler<TouchEventArgs>();
        public BaseEventHandler<TouchEventArgs> TouchCancelled = new CustomEventHandler<TouchEventArgs>();

        public TouchListenerComponent(PortableGame game) : this(game, new TouchListenerSettings()) { }
        public TouchListenerComponent(PortableGame game, ViewportAdapter viewportAdapter) : this(game, new TouchListenerSettings()) { ViewportAdapter = viewportAdapter; }
        public TouchListenerComponent(PortableGame game, TouchListenerSettings settings) : base(game) { ViewportAdapter = settings.ViewportAdapter; }

        public override void Update(GameTime gameTime)
        {
            var touchCollection = TouchPanel.GetState();

            foreach (var touchLocation in touchCollection)
            {
                switch (touchLocation.State)
                {
                    case TouchLocationState.Pressed:
                        ((BaseEventHandlerWithInvoke<TouchEventArgs>) TouchStarted)?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation));
                        break;
                    case TouchLocationState.Moved:
                        ((BaseEventHandlerWithInvoke<TouchEventArgs>) TouchMoved)?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation));
                        break;
                    case TouchLocationState.Released:
                        ((BaseEventHandlerWithInvoke<TouchEventArgs>) TouchEnded)?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation));
                        break;
                    case TouchLocationState.Invalid:
                        ((BaseEventHandlerWithInvoke<TouchEventArgs>) TouchCancelled)?.Invoke(this, new TouchEventArgs(ViewportAdapter, gameTime.TotalGameTime, touchLocation));
                        break;
                }
            }
        }
    }
}