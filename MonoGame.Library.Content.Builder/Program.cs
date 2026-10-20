using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Framework.Content.Pipeline.Builder;
using System;

namespace MonoGame.Library.Content.Builder;

internal class Program
{
    private static int Main (string[] args)
    {
        ContentBuilderParams contentCollectionArgs = new ()
        {
            Mode = ContentBuilderMode.Builder,
            WorkingDirectory = $"{AppContext.BaseDirectory}../../", // path to where your content folder can be located
            SourceDirectory = "Assets", // Not actually needed as this is the default, but added for reference
            Platform = TargetPlatform.DesktopGL
        };

        Builder builder = new ();

        if (args is not null && args.Length > 0)
        {
            builder.Run (args);
        }
        else
        {
            builder.Run (contentCollectionArgs);
        }

        return builder.FailedToBuild > 0 ? -1 : 0;
    }
}
