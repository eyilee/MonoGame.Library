using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Library.Content.Readers;
using MonoGame.Library.Graphics;

namespace MonoGame.Library.Content.Pipeline.Writers;

[ContentTypeWriter]
public sealed class SpriteSheetWriter : ContentTypeWriter<SpriteSheetDocument>
{
    protected override void Write (ContentWriter output, SpriteSheetDocument value)
    {
        output.Write (value.Sprite);
        WriteRegions (output, value);
        WriteAnimations (output, value);
        output.WriteObject (value.Texture);
    }

    private static void WriteRegions (ContentWriter output, SpriteSheetDocument value)
    {
        output.Write (value.Regions.Count);

        foreach ((string name, SpriteSheetRegionDocument region) in value.Regions)
        {
            output.Write (name);
            output.Write (region.X);
            output.Write (region.Y);
            output.Write (region.Width);
            output.Write (region.Height);
        }
    }

    private static void WriteAnimations (ContentWriter output, SpriteSheetDocument value)
    {
        output.Write (value.Animations.Count);

        foreach ((string name, SpriteSheetAnimationDocument animation) in value.Animations)
        {
            output.Write (name);
            WriteFrames (output, animation);
        }
    }

    private static void WriteFrames (ContentWriter output, SpriteSheetAnimationDocument animation)
    {
        output.Write (animation.Frames.Count);

        foreach (SpriteSheetAnimationFrameDocument frame in animation.Frames)
        {
            output.Write (frame.Region);
            output.Write (frame.Duration);
        }
    }

    public override string GetRuntimeType (TargetPlatform targetPlatform)
    {
        return typeof (SpriteSheet).AssemblyQualifiedName!;
    }

    public override string GetRuntimeReader (TargetPlatform targetPlatform)
    {
        return typeof (SpriteSheetReader).AssemblyQualifiedName!;
    }
}
