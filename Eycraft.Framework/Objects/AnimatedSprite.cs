using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class AnimatedSprite : Sprite
{
    private SpriteSheet? _spriteSheet;

    public SpriteSheet? SpriteSheet
    {
        get { return _spriteSheet; }
        set
        {
            if (!ReferenceEquals (_spriteSheet, value))
            {
                SetSpriteSheet (value);
            }
        }
    }

    private SpriteAnimation? _currentAnimation;

    private int _currentFrame;

    private float _elapsedTime;

    private void SetSpriteSheet (SpriteSheet? spriteSheet)
    {
        _spriteSheet = spriteSheet;
        _currentAnimation = null;
        _currentFrame = 0;
        _elapsedTime = 0;
    }

    public void PlayAnimation (string animationName, int time = 0)
    {
        if (_spriteSheet == null)
        {
            return;
        }

        SpriteAnimation? animation = _spriteSheet.GetAnimation (animationName);
        if (animation == null || animation.Frames.Count == 0)
        {
            return;
        }

        _currentAnimation = animation;

        int index = animation.EvaluteFrame (time);
        _currentFrame = index >= 0 ? index : 0;

        SpriteAnimationFrame frame = animation.Frames[_currentFrame];
        _elapsedTime = (float)frame.Duration.TotalMilliseconds;

        Region = frame.Region;
    }

    public void Update (GameTime gameTime)
    {
        if (_currentAnimation == null)
        {
            return;
        }

        _elapsedTime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_elapsedTime <= 0)
        {
            _currentFrame = (_currentFrame + 1) % _currentAnimation.Frames.Count;

            SpriteAnimationFrame frame = _currentAnimation.Frames[_currentFrame];
            _elapsedTime += (float)frame.Duration.TotalMilliseconds;

            Region = frame.Region;
        }
    }
}
