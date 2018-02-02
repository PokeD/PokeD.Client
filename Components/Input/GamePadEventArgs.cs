using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokeD.CPGL.Components.Input
{
    public enum GamePadAnalogStickDirection { None, Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft }


    /// <summary>
    /// This class contains all information resulting from events fired by
    /// <see cref="GamePadListener" />.
    /// </summary>
    public class GamePadEventArgs : EventArgs
    {
        public GamePadEventArgs(GamePadState previousState, GamePadState currentState, TimeSpan elapsedTime, PlayerIndex playerIndex, Buttons? button = null, float triggerState = 0, Vector2? thumbStickState = null)
        {
            PlayerIndex = playerIndex;
            ElapsedTime = elapsedTime;
            if (button != null)
                Button = button.Value;
            TriggerState = triggerState;
            ThumbStick = thumbStickState ?? Vector2.Zero;
            ThumbStickDirection = GetAnalogStickDirection(ThumbStick);

            /*
            PreviousState = previousState;
            CurrentState = currentState;
            */
        }

        /// <summary>
        /// The index of the controller.
        /// </summary>
        public PlayerIndex PlayerIndex { get; }

        /// <summary>
        /// The state of the controller in the previous update.
        /// </summary>
        //public GamePadState PreviousState { get; }

        /// <summary>
        /// The state of the controller in this update.
        /// </summary>
        //public GamePadState CurrentState { get; }

        /// <summary>
        /// The button that triggered this event, if appliable.
        /// </summary>
        public Buttons Button { get; }

        /// <summary>
        /// The time elapsed since last event.
        /// </summary>
        public TimeSpan ElapsedTime { get; }

        /// <summary>
        /// If a TriggerMoved event, displays the responsible trigger's position.
        /// </summary>
        public float TriggerState { get; }

        /// <summary>
        /// If a ThumbStickMoved event, displays the responsible stick's position.
        /// </summary>
        public Vector2 ThumbStick { get; }


        public GamePadAnalogStickDirection ThumbStickDirection { get; }

        #region AnalogThumbStick stuff

        private const float PI = (float) Math.PI;

        // You could make this user-configurable.
        private const float Deadzone = 0.8f;
        private const float AliveAngle = PI / 4 - (PI / 16);

        // Remember PI = 180 degrees
        private const float AliveAngle1Start = PI * 0;
        private const float AliveAngle1End = PI * 0.5f - AliveAngle;

        private const float AliveAngle2Start = PI * 0.5f + AliveAngle;
        private const float AliveAngle2End = PI * 1f - AliveAngle;

        private const float AliveAngle3Start = PI * 1f + AliveAngle;
        private const float AliveAngle3End = PI * 1.5f - AliveAngle;

        private const float AliveAngle4Start = PI * 1.5f + AliveAngle;
        private const float AliveAngle4End = PI * 2f - AliveAngle;

        private static GamePadAnalogStickDirection GetAnalogStickDirection(Vector2 gamepadThumbStick)
        {
            // Get the length and prevent something from happening
            // if it's in our deadzone.
            var length = gamepadThumbStick.Length();
            if (length < Deadzone)
                return GamePadAnalogStickDirection.None;

            // Find the angle that the stick is at. see: http://en.wikipedia.org/wiki/File:Atan2_60.svg
            var angle = (float) Math.Atan2(gamepadThumbStick.Y, gamepadThumbStick.X);
            if (angle < 0)
                angle += PI * 2; // Simpify our checks.

            if (angle > AliveAngle4End)
                return GamePadAnalogStickDirection.Right;

            if (angle > AliveAngle4Start)
                return GamePadAnalogStickDirection.DownRight;

            if (angle > AliveAngle3End)
                return GamePadAnalogStickDirection.Down;

            if (angle > AliveAngle3Start)
                return GamePadAnalogStickDirection.DownLeft;

            if (angle > AliveAngle2End)
                return GamePadAnalogStickDirection.Left;

            if (angle > AliveAngle2Start)
                return GamePadAnalogStickDirection.UpLeft;

            if (angle > AliveAngle1End)
                return GamePadAnalogStickDirection.Up;

            if (angle > AliveAngle1Start)
                return GamePadAnalogStickDirection.UpRight;

            return GamePadAnalogStickDirection.Right;
        }

        #endregion
    }
}