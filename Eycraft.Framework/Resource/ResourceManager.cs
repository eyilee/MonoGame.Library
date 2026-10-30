using Microsoft.Xna.Framework.Content;
using System;
using System.IO;

namespace Eycraft.Framework.Resource;

internal class ResourceManager (IServiceProvider servicesProvider) : ContentManager (servicesProvider)
{
    private IResourceProvider? _resourceProvider;

    public IResourceProvider ResourceProvider
    {
        get => _resourceProvider ?? throw new InvalidOperationException ("Resource provider is not set.");
        set => _resourceProvider = value;
    }

    protected override Stream OpenStream (string assetName) => ResourceProvider.OpenStream (assetName);
}
