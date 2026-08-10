using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Library;
using MonoGame.Library.Graphics;

namespace Test;

public class GameScene : Scene
{
    private SdfRectangle _rectangle = null!;

    private SdfRectangle _filledRectangle = null!;

    public override void Initialize ()
    {
        base.Initialize ();
    }

    public override void LoadContent ()
    {
        _rectangle = new SdfRectangle ()
        {
            Position = new Vector2 (200, 300),
            Thickness = 5f,
            Color = Color.Red,
            Size = new Vector2 (100, 50)
        };

        _filledRectangle = new SdfRectangle ()
        {
            Position = new Vector2 (400, 300),
            Thickness = 5f,
            Color = Color.Blue,
            Filled = true,
            Size = new Vector2 (100, 50)
        };

        base.LoadContent ();
    }

    public override void Update (GameTime gameTime)
    {
        //float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        base.Update (gameTime);
    }

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        _rectangle.Draw (Render);
        _filledRectangle.Draw (Render);

        base.Draw (gameTime);
    }
}
