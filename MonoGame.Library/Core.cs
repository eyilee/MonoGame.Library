using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Library.Graphics;
using MonoGame.Library.Input;
using System;

namespace MonoGame.Library;

public class Core : Game
{
    public static Core Instance => _instance!;

    public static Scene? ActiveScene => _activeScene;

    public static Camera MainCamera
    {
        get => _mainCamera;
        set => _mainCamera = value ?? _defaultCamera;
    }

    public static GraphicsDeviceManager Graphics { get; private set; } = null!;

    public static new ContentManager Content { get; private set; } = null!;

    public static new GraphicsDevice GraphicsDevice { get; private set; } = null!;

    public static int ScreenWidth => Graphics.PreferredBackBufferWidth;

    public static int ScreenHeight => Graphics.PreferredBackBufferHeight;

    public static RenderManager Render { get; private set; } = null!;

    public static InputManager Input { get; private set; } = null!;

    public static bool ExitOnEscape { get; set; }

    internal static ResourceManager Resource { get; private set; } = null!;

    private static Core? _instance;

    private static Scene? _activeScene;

    private static Scene? _nextScene;

    private static Camera _mainCamera = null!;

    private static Camera _defaultCamera = null!;

    public Core (string title, int width, int height, bool isFullScreen)
    {
        if (_instance != null)
        {
            throw new InvalidOperationException ($"Only a single Core instance can be created");
        }

        _instance = this;

        Window.Title = title;

        Graphics = new GraphicsDeviceManager (this)
        {
            PreferredBackBufferWidth = width,
            PreferredBackBufferHeight = height,
            IsFullScreen = isFullScreen
        };

        Content = base.Content;
        Content.RootDirectory = "Content";

        IsMouseVisible = true;

        ExitOnEscape = true;

        Resource = new ResourceManager (Services);
    }

    protected override void Initialize ()
    {
        GraphicsDevice = base.GraphicsDevice;

        Render = new RenderManager ();

        Input = new InputManager ();

        _mainCamera = new Camera (GraphicsDevice);
        _defaultCamera = _mainCamera;

        base.Initialize ();
    }

    protected override void LoadContent ()
    {
        base.LoadContent ();
    }

    protected override void UnloadContent ()
    {
        base.UnloadContent ();
    }

    protected override void Update (GameTime gameTime)
    {
        Input.Update (gameTime);

        if (ExitOnEscape && Input.Keyboard.WasJustPressed (Keys.Escape))
        {
            Exit ();
        }

        if (_nextScene != null)
        {
            TransitionScene ();
        }

        _activeScene?.Update (gameTime);

        base.Update (gameTime);
    }

    protected override void Draw (GameTime gameTime)
    {
        _activeScene?.Draw (gameTime);

        Render.Draw ();

        base.Draw (gameTime);
    }

    public static void ChangeScene (Scene next)
    {
        if (_activeScene != next)
        {
            _nextScene = next;
        }
    }

    private static void TransitionScene ()
    {
        _activeScene?.Dispose ();

        GC.Collect ();

        _activeScene = _nextScene;

        _nextScene = null;

        _activeScene?.Initialize ();
    }
}
