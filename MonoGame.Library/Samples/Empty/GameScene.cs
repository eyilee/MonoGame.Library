using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Library;

namespace Empty;

public class GameScene : Scene
{
    public override void Initialize ()
    {
        base.Initialize ();
    }

    public override void LoadContent ()
    {
        base.LoadContent ();
    }

    public override void UnloadContent ()
    {
        base.UnloadContent ();
    }

    protected override void Dispose (bool disposing)
    {
        if (disposing)
        {
        }

        base.Dispose (disposing);
    }

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        base.Draw (gameTime);
    }
}
