using Eycraft.Framework.DesktopGL;

namespace Playground.Game;

public class Main : Core
{
    private const int WindowWidth = 800;

    private const int WindowHeight = 600;

    public Main ()
        : base ("Playground", WindowWidth, WindowHeight, false)
    {
    }

    protected override void BeginRun ()
    {
        ChangeScene (new GameScene ());
    }
}
