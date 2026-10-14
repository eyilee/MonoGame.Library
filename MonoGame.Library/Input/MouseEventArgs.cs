using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame.Library.Input;

public class MouseEventArgs (MouseButtons button, MouseState currentState, MouseState previousState) : EventArgs
{
    public MouseButtons Button { get; init; } = button;

    public Point Position { get; init; } = new Point (currentState.X, currentState.Y);

    public Point PositionDelta { get; init; } = new Point (currentState.X - previousState.X, currentState.Y - previousState.Y);

    public int ScrollWheelDelta { get; init; } = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
}
