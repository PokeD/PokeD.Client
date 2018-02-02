using PokeD.CPGL.Components.ViewportAdapters;

namespace PokeD.CPGL.Components.Input
{
    public class MouseListenerSettings
    {
        public MouseListenerSettings()
        {
            // initial values are windows defaults
            DoubleClickMilliseconds = 500;
            DragThreshold = 2;
        }

        public int DragThreshold { get; set; }
        public int DoubleClickMilliseconds { get; set; }
        public ViewportAdapter ViewportAdapter { get; set; }
    }
}