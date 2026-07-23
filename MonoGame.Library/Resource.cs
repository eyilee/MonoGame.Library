using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.Reflection;

namespace MonoGame.Library;

public class ResourceManager (IServiceProvider servicesProvider) : ContentManager (servicesProvider)
{
    private readonly Assembly _assembly = typeof (Core).Assembly;

    protected override Stream OpenStream (string assetName) => _assembly.GetManifestResourceStream (assetName) ?? throw new FileNotFoundException ();
}
