using Microsoft.Xna.Framework;

namespace MonoGame.Library.Input;

public class InputManager
{
    public KeyboardListener Keyboard { get; }

    public MouseListener Mouse { get; }

    public InputManager ()
    {
        Keyboard = new KeyboardListener ();
        Mouse = new MouseListener ();
    }

    public void Update (GameTime gameTime)
    {
        Keyboard.Update (gameTime);
        Mouse.Update (gameTime);
    }
}
