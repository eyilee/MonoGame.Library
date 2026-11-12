using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class AnimatedSprite (Entity entity) : Sprite (entity)
{
    private SpriteSheet? _spriteSheet;

    public SpriteSheet? SpriteSheet
    {
        get => _spriteSheet;
        set
        {
            if (!ReferenceEquals (_spriteSheet, value))
            {
                _spriteSheet = value;
                _currentAnimation = null;
                _currentFrame = 0;
                _elapsedTime = 0;
            }
        }
    }

    private SpriteAnimation? _currentAnimation;

    private int _currentFrame = 0;

    private float _elapsedTime = 0f;

    public void PlayAnimation (string animationName, int time = 0)
    {
        if (SpriteSheet == null)
        {
            return;
        }

        SpriteAnimation? animation = SpriteSheet.GetAnimation (animationName);
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
        if (_currentAnimation == null || _currentAnimation.Frames.Count <= 1)
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
