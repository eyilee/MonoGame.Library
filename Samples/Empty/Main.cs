using MonoGame.Library;

namespace Empty;

public class Main : Core
{
    private const int WindowWidth = 800;

    private const int WindowHeight = 600;

    public Main ()
        : base ("Empty", WindowWidth, WindowHeight, false)
    {
    }

    protected override void Initialize ()
    {
        ChangeScene (new GameScene ());

        base.Initialize ();
    }
}
