using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Library;
using MonoGame.Library.Graphics;

namespace Test;

public class GameScene : Scene
{
    private Texture2D _pixelTexture = null!;

    private Texture2DResource _pixel = null!;

    private Sprite _boundary = null!;

    private SdfCircle _focus = null!;

    private SdfCircle _vertex = null!;

    private SdfParabola _parabola = null!;

    private float _rotation = 0f;

    private Vector2 _vertexPoint = new (400f, 300f);

    private float _angle = 0f;

    private readonly float _length = 20f;

    public override void Initialize ()
    {
        base.Initialize ();
    }

    public override void LoadContent ()
    {
        _pixelTexture = new Texture2D (GraphicsDevice, 1, 1);
        _pixelTexture.SetData ([Color.White]);

        _pixel = new Texture2DResource ("Pixel", _pixelTexture);

        _boundary = new Sprite (new TextureRegion (_pixel, new Rectangle (0, 0, 1, 1)))
        {
            Size = new Vector2 (100f, 100f),
            Position = _vertexPoint,
            Color = Color.Green,
            Rotation = _rotation,
            Origin = new Vector2 (50f, 50f)
        };

        Vector2 offset = Vector2.One * _length;
        offset.Rotate (_angle);

        Vector2 focusPoint = _vertexPoint + offset;

        _focus = new SdfCircle
        {
            Position = focusPoint,
            Thickness = 1f,
            Color = Color.Blue,
            Radius = 3f
        };

        _vertex = new SdfCircle
        {
            Position = _vertexPoint,
            Thickness = 1f,
            Color = Color.Red,
            Radius = 3f
        };

        _parabola = new SdfParabola
        {
            Position = _vertexPoint,
            Rotation = _rotation,
            Scale = new Vector2 (100f, 100f),
            Thickness = 3f,
            Color = Color.Yellow,
            Focus = focusPoint,
            Vertex = _vertexPoint
        };

        base.LoadContent ();
    }

    public override void UnloadContent ()
    {
        _pixelTexture?.Dispose ();

        base.UnloadContent ();
    }

    protected override void Dispose (bool disposing)
    {
        if (disposing)
        {
        }

        base.Dispose (disposing);
    }

    public override void Update (GameTime gameTime)
    {
        _rotation += (float.Pi / 4f * (float)gameTime.ElapsedGameTime.TotalSeconds);
        _angle += (float.Pi / 4f * (float)gameTime.ElapsedGameTime.TotalSeconds);

        Vector2 offset = -Vector2.UnitY * _length;
        offset.Rotate (_angle);

        Vector2 focusPoint = _vertexPoint + offset;

        _boundary.Rotation = _rotation;
        _parabola.Rotation = _rotation;
        _parabola.Focus = focusPoint;

        offset.Rotate (_rotation);

        _focus.Position = _vertexPoint + offset;

        base.Update (gameTime);
    }

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        _boundary.Draw (Render);
        _focus.Draw (Render);
        _vertex.Draw (Render);
        _parabola?.Draw (Render);

        base.Draw (gameTime);
    }
}
