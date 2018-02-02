namespace PokeD.CPGL.Components.Input
{
    public class KeyboardListenerSettings
    {
        public KeyboardListenerSettings()
        {
            InitialDelayMilliseconds = 800;
            RepeatDelayMilliseconds = 50;
        }

        public int InitialDelayMilliseconds { get; }
        public int RepeatDelayMilliseconds { get; }
    }
}