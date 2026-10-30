using System;

namespace Eycraft.Framework.Input;

[Flags]
public enum KeyboardModifiers
{
    None = 0,
    Control = 1,
    Shift = 2,
    Alt = 4,
}
