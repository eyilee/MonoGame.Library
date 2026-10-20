using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Library.Graphics;
using System.Collections.Generic;

namespace MonoGame.Library.Content.Readers;

public sealed class SpriteSheetReader : ContentTypeReader<SpriteSheet>
{
    protected override SpriteSheet Read (ContentReader input, SpriteSheet existingInstance)
    {
        SpriteSheet spriteSheet = existingInstance ?? new SpriteSheet ();

        spriteSheet.Name = input.ReadString ();
        ReadRegions (input, spriteSheet);
        ReadAnimations (input, spriteSheet);
        spriteSheet.Texture = input.ReadObject<Texture2D> ();

        return spriteSheet;
    }

    private static void ReadRegions (ContentReader input, SpriteSheet spriteSheet)
    {
        int count = input.ReadInt32 ();

        for (int i = 0; i < count; i++)
        {
            string name = input.ReadString ();

            spriteSheet.Regions.Add (name, new SpriteSheetRegion
            {
                Name = name,
                X = input.ReadInt32 (),
                Y = input.ReadInt32 (),
                Width = input.ReadInt32 (),
                Height = input.ReadInt32 ()
            });
        }
    }

    private static void ReadAnimations (ContentReader input, SpriteSheet spriteSheet)
    {
        int count = input.ReadInt32 ();

        for (int i = 0; i < count; i++)
        {
            string name = input.ReadString ();

            List<SpriteSheetAnimationFrame> frames = ReadFrames (input);

            spriteSheet.Animations.Add (name, new SpriteSheetAnimation
            {
                Name = name,
                Frames = frames
            });
        }
    }

    private static List<SpriteSheetAnimationFrame> ReadFrames (ContentReader input)
    {
        List<SpriteSheetAnimationFrame> frames = [];

        int count = input.ReadInt32 ();

        for (int j = 0; j < count; j++)
        {
            frames.Add (new SpriteSheetAnimationFrame
            {
                Region = input.ReadString (),
                Duration = input.ReadInt32 ()
            });
        }

        return frames;
    }
}
