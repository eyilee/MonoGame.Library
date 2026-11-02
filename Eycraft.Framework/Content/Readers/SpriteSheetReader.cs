using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Eycraft.Framework.Content.Readers;

public sealed class SpriteSheetReader : ContentTypeReader<SpriteSheet>
{
    protected override SpriteSheet Read (ContentReader input, SpriteSheet existingInstance)
    {
        SpriteSheet spriteSheet = existingInstance ?? new SpriteSheet ();

        spriteSheet.Name = input.ReadString ();
        spriteSheet.Texture = new (spriteSheet.Name, input.ReadObject<Texture2D> ());
        spriteSheet.Regions = [];
        spriteSheet.Animations = [];

        ReadRegions (input, spriteSheet);
        ReadAnimations (input, spriteSheet);

        return spriteSheet;
    }

    private static void ReadRegions (ContentReader input, SpriteSheet spriteSheet)
    {
        int count = input.ReadInt32 ();

        for (int i = 0; i < count; i++)
        {
            string name = input.ReadString ();
            int x = input.ReadInt32 ();
            int y = input.ReadInt32 ();
            int width = input.ReadInt32 ();
            int height = input.ReadInt32 ();
            spriteSheet.Regions.Add (name, new TextureRegion (spriteSheet.Texture, x, y, width, height));
        }
    }

    private static void ReadAnimations (ContentReader input, SpriteSheet spriteSheet)
    {
        int count = input.ReadInt32 ();

        for (int i = 0; i < count; i++)
        {
            string name = input.ReadString ();
            List<SpriteAnimationFrame> frames = ReadFrames (input, spriteSheet);
            spriteSheet.Animations.Add (name, new SpriteAnimation (name, frames));
        }
    }

    private static List<SpriteAnimationFrame> ReadFrames (ContentReader input, SpriteSheet spriteSheet)
    {
        List<SpriteAnimationFrame> frames = [];

        int count = input.ReadInt32 ();

        for (int i = 0; i < count; i++)
        {
            string name = input.ReadString ();
            int duration = input.ReadInt32 ();

            if (!spriteSheet.Regions.TryGetValue (name, out TextureRegion? region))
            {
                throw new ContentLoadException ($"Region '{name}' not found in sprite sheet '{spriteSheet.Name}'.");
            }

            frames.Add (new SpriteAnimationFrame (region, duration));
        }

        return frames;
    }
}
