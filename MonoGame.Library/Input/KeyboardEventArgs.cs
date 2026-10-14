using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame.Library.Input;

public class KeyboardEventArgs : EventArgs
{
    public Keys Key { get; }

    public KeyboardModifiers Modifiers { get; }

    public KeyboardEventArgs (Keys key, KeyboardState keyboardState)
    {
        Key = key;

        Modifiers = KeyboardModifiers.None;

        if (keyboardState.IsKeyDown (Keys.LeftControl) || keyboardState.IsKeyDown (Keys.RightControl))
        {
            Modifiers |= KeyboardModifiers.Control;
        }

        if (keyboardState.IsKeyDown (Keys.LeftShift) || keyboardState.IsKeyDown (Keys.RightShift))
        {
            Modifiers |= KeyboardModifiers.Shift;
        }

        if (keyboardState.IsKeyDown (Keys.LeftAlt) || keyboardState.IsKeyDown (Keys.RightAlt))
        {
            Modifiers |= KeyboardModifiers.Alt;
        }
    }
}
