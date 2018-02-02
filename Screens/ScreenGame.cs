using PokeD.CPGL.Data;

namespace PokeD.CPGL.Screens
{
    public abstract class ScreenGame : Screen
    {
        protected Player Player { get; }


        protected ScreenGame(PortableGame game, Player player) : base(game)
        {
            Player = player;
        }
    }

    /*
    // Game
    // ScreenManager
    // UIOverlayScreen
    // Screen
    //

    public abstract class UIScreen : Screen
    {
        protected Screen Child { get; }


        protected UIScreen(PortableGame game, Screen child) : base(game)
        {
            Child = child;
        }
    }
    */
}
