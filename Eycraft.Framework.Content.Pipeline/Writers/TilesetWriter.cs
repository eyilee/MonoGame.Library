using Eycraft.Framework.Content.Readers;
using Eycraft.Framework.Tilemap;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Eycraft.Framework.Content.Pipeline.Writers;

[ContentTypeWriter]
public sealed class TilesetWriter : ContentTypeWriter<TilesetDocument>
{
    protected override void Write (ContentWriter output, TilesetDocument value)
    {
        output.Write (value.Name);
        output.WriteObject (value.Texture);
        output.Write (value.TileWidth);
        output.Write (value.TileHeight);
    }

    public override string GetRuntimeType (TargetPlatform targetPlatform)
    {
        return typeof (Tileset).AssemblyQualifiedName!;
    }

    public override string GetRuntimeReader (TargetPlatform targetPlatform)
    {
        return typeof (TilesetReader).AssemblyQualifiedName!;
    }
}
