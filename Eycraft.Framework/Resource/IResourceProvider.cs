using System.IO;

namespace Eycraft.Framework.Resource;

internal interface IResourceProvider
{
    public virtual Stream OpenStream (string assetName) => GetType ().Assembly.GetManifestResourceStream (assetName) ?? throw new FileNotFoundException ();
}
