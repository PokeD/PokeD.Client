namespace PokeD.CPGL.Data
{
    /// <summary>
    /// The sequence mode of a gif.
    /// </summary>
    public enum SequenceMode
    {
        /// <summary>
        /// Cycle through all frames.
        /// </summary>
        Normal,

        /// <summary>
        /// Select a random frame.
        /// </summary>
        Random,

        /// <summary>
        /// Ping-pong.
        /// </summary>
        ReverseAtEnd
    }
}