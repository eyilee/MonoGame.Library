using Microsoft.Xna.Framework;
using System;

namespace MonoGame.Library.Input;

internal class ExtendedButtonState
{
    public Point PressedPosition { get; private set; } = Point.Zero;

    public TimeSpan ReleasedTime { get; private set; } = TimeSpan.Zero;

    public bool IsDoubleClick { get; private set; }

    public bool IsDragBegin { get; private set; }

    public bool IsDrag { get; private set; }

    public bool IsDragEnd { get; private set; }

    public void Pressed (Point poition)
    {
        PressedPosition = poition;

        IsDragBegin = false;

        IsDrag = false;

        IsDragEnd = false;
    }

    public void Released (TimeSpan gameTime, int doubleClickedMilliseconds)
    {
        TimeSpan previousReleasedTime = ReleasedTime;

        ReleasedTime = gameTime;

        IsDoubleClick = (ReleasedTime - previousReleasedTime).TotalMilliseconds <= doubleClickedMilliseconds;

        if (IsDragBegin || IsDrag)
        {
            IsDragBegin = false;

            IsDrag = false;

            IsDragEnd = true;
        }
    }

    public void Moved (Point position, int dragThreshold)
    {
        if (!IsDrag)
        {
            if (!IsDragBegin)
            {
                int deltaPosition = Math.Abs (position.X - PressedPosition.X) + Math.Abs (position.Y - PressedPosition.Y);

                if (deltaPosition >= dragThreshold)
                {
                    IsDragBegin = true;
                }
            }
            else
            {
                IsDrag = true;
            }
        }
    }
}
