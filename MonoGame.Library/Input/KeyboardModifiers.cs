using System;

namespace MonoGame.Library.Input;

[Flags]
public enum KeyboardModifiers
{
    None = 0,
    Control = 1,
    Shift = 2,
    Alt = 4,
}
