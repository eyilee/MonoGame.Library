using System;
using System.Collections.Generic;

namespace Eycraft.Framework.Graphics;

public class SpriteAnimation
{
    public string Name { get; }

    public List<SpriteAnimationFrame> Frames { get; }

    public TimeSpan Duration { get; }

    public SpriteAnimation (string name, List<SpriteAnimationFrame> frames)
    {
        Name = name;
        Frames = frames;
        Duration = TimeSpan.Zero;

        foreach (SpriteAnimationFrame frame in frames)
        {
            Duration += frame.Duration;
        }
    }

    public int EvaluteFrame (float elapsedMiliseconds)
    {
        elapsedMiliseconds = elapsedMiliseconds % (float)Duration.TotalMilliseconds;

        for (int i = 0; i < Frames.Count; i++)
        {
            elapsedMiliseconds -= (float)Frames[i].Duration.TotalMilliseconds;

            if (elapsedMiliseconds <= 0)
            {
                return i;
            }
        }

        return -1;
    }
}
