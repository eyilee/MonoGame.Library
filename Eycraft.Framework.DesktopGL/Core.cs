namespace Eycraft.Framework.DesktopGL;

public class Core : Framework.Core
{
    public Core (string title, int width, int height, bool isFullScreen)
        : base (title, width, height, isFullScreen)
    {
        Resource.ResourceProvider = new ResourceProvider ();
    }
}
