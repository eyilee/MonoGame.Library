using Eycraft.Framework;
using Microsoft.Xna.Framework;

namespace Playground.Game.Character;

public class TrackingCamera (Entity entity) : EntityComponent (entity)
{
    public Camera? Camera { get; set; }

    public Vector2 Target { get; set; } = Vector2.Zero;

    private Vector2 _position = Vector2.Zero;

    private Vector2 _velocity = Vector2.Zero;

    private readonly float _damping = 0.1f;

    public void Update (GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float length = Vector2.Distance (_position, Target);

        if (float.Abs (length) < 0.01f)
        {
            _position = Target;
            _velocity = Vector2.Zero;
            return;
        }

        Vector2 acceleration = (Target - _position) * 4f;
        _velocity *= _damping;
        _velocity += acceleration;
        _position += _velocity * deltaTime;

        Camera?.LookAt (_position);
    }
}
