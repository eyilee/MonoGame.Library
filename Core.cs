using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Library.Graphics;
using MonoGame.Library.Input;
using MonoGameGum;
using System;

namespace MonoGame.Library;

public class Core : Game
{
    private static Core? _instance;

    public static Core Instance => _instance!;

    private static Scene? _activeScene;

    private static Scene? _nextScene;

    public static Scene? ActiveScene => _activeScene;

    private static Camera _defaultCamera = null!;

    public static Camera MainCamera { get; private set; } = null!;

    public static GraphicsDeviceManager Graphics { get; private set; } = null!;

    public static new ContentManager Content { get; private set; } = null!;

    public static new GraphicsDevice GraphicsDevice { get; private set; } = null!;

    public static int ScreenWidth => Graphics.PreferredBackBufferWidth;

    public static int ScreenHeight => Graphics.PreferredBackBufferHeight;

    public static SpriteBatch SpriteBatch { get; private set; } = null!;

    public static RenderManager Render { get; private set; } = null!;

    public static InputManager Input { get; private set; } = null!;

    public static bool ExitOnEscape { get; set; }

    public static GumService GumUI => GumService.Default;

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
    }

    protected override void Initialize ()
    {
        GraphicsDevice = base.GraphicsDevice;

        SpriteBatch = new SpriteBatch (GraphicsDevice);

        Render = new RenderManager (GraphicsDevice);

        Input = new InputManager ();

        InitializeUI ();

        base.Initialize ();
    }

    private void InitializeUI ()
    {
        GumUI.Initialize (this, DefaultVisualsVersion.V3);

        if (GumUI.ContentLoader != null)
        {
            GumUI.ContentLoader.XnaContentManager = Content;
        }

        GumUI.UseKeyboardDefaults ();

        GumUI.UseGamepadDefaults ();

        FrameworkElement.TabReverseKeyCombos.Add (new KeyCombo () { PushedKey = Keys.Up });

        FrameworkElement.TabKeyCombos.Add (new KeyCombo () { PushedKey = Keys.Down });
    }

    protected override void LoadContent ()
    {
        _defaultCamera = new Camera (GraphicsDevice);

        SetCamera (_defaultCamera);

        base.LoadContent ();
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

        GumUI.Update (gameTime);

        base.Update (gameTime);
    }

    protected override void Draw (GameTime gameTime)
    {
        _activeScene?.Draw (gameTime);

        Render.Draw ();

        GumUI.Draw ();

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

    public static void SetCamera (Camera? camera)
    {
        MainCamera = camera ?? _defaultCamera;
    }
}
