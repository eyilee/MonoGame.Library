using System.Collections.Generic;

namespace Eycraft.Framework.Graphics;

public class SpriteAnimation
{
    public string Name { get; }

    public List<SpriteAnimationFrame> Frames { get; }

    public int Duration { get; }

    public SpriteAnimation (string name, List<SpriteAnimationFrame> frames)
    {
        Name = name;
        Frames = frames;
        Duration = 0;

        foreach (var frame in frames)
        {
            Duration += frame.Duration;
        }
    }

    public SpriteAnimationFrame? EvaluteFrame (int time)
    {
        time %= Duration;

        int elapsed = 0;

        foreach (SpriteAnimationFrame frame in Frames)
        {
            elapsed += frame.Duration;

            if (time < elapsed)
            {
                return frame;
            }
        }

        return null;
    }
}
