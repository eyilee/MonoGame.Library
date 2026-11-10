using Eycraft.Framework;
using Eycraft.Framework.Graphics;
using Eycraft.Framework.Objects;
using Microsoft.Xna.Framework;

namespace Playground.Game;

public class ViewPoint : Entity
{
    private SdfCircle _shape;

    private Vector2 _position = Vector2.Zero;

    private Vector2 _velocity = Vector2.Zero;

    private float _targetLength = 0f;

    private float _damping = 0.1f;

    public ViewPoint ()
    {
        _shape = AddComponent<SdfCircle> ();
        _shape.Radius = 3f;
        _shape.Color = Color.Yellow;
    }

    public void Initialize (Vector2 position, float targetLength = 0f, float damping = 0.1f)
    {
        _position = position;
        _targetLength = targetLength;
        _damping = damping;

        Camera.Main.LookAt (_position);
    }

    public void Update (Vector2 target, float deltaTime)
    {
        float length = Vector2.Distance (_position, target);

        if (float.Abs (length - _targetLength) < 0.01f)
        {
            _position = target;
            _velocity = Vector2.Zero;
            return;
        }

        Vector2 acceleration = (target - _position) / length * (length - _targetLength) * 4f;
        _velocity *= _damping;
        _velocity += acceleration;
        _position += _velocity * deltaTime;

        _shape.Offset = _position;
        Camera.Main.LookAt (_position);
    }

    public override void Draw (RenderManager render)
    {
        _shape.Draw (render);
    }
}
