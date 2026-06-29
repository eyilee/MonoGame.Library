using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Library;
using MonoGame.Library.Graphics;

namespace Test;

public class GameScene : Scene
{
    private Sprite? _slimeSprite;
    private Sprite? _batSprite;
    private Texture2DResource? _pixelTexture;
    private Sprite? _pixelSprite;
    private Canvas? _canvas;

    public override void Initialize ()
    {
        base.Initialize ();
    }

    public override void LoadContent ()
    {
        TextureAtlas atlas = TextureAtlas.FromFile (Content, "atlas-definition.xml");

        if (atlas.TryGetRegion ("slime", out TextureRegion? slimeRegion) && slimeRegion != null)
        {
            _slimeSprite = new Sprite (slimeRegion)
            {
                Position = new Vector2 (100, 100),
            };
        }

        if (atlas.TryGetRegion ("bat", out TextureRegion? batRegion) && batRegion != null)
        {
            _batSprite = new Sprite (batRegion)
            {
                Position = new Vector2 (200, 100),
            };
        }

        Texture2D texture = new (GraphicsDevice, 1, 1);
        texture.SetData ([Color.White]);
        _pixelTexture = new Texture2DResource ("pixel", texture);

        _pixelSprite = new Sprite (new TextureRegion (_pixelTexture, new Rectangle (0, 0, 1, 1)))
        {
            Size = new Vector2 (2, 2),
            Position = new Vector2 (100, 100),
            Color = Color.White,
        };

        _canvas = new Canvas (GraphicsDevice, "canvas1", 0, 0, 64, 64);
        _canvas.Clear (Color.Red);

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                _canvas.SetPixel (i, j, Color.Blue);
            }
        }

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

    public override void Draw (GameTime gameTime)
    {
        GraphicsDevice.Clear (Color.CornflowerBlue);

        _slimeSprite?.Draw (Render);
        _batSprite?.Draw (Render);
        _pixelSprite?.Draw (Render);
        _canvas?.Draw (Render);

        base.Draw (gameTime);
    }
}
