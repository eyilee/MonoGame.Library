using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public abstract class Shape (Entity entity) : EntityComponent (entity)
{
    protected readonly Mesh _mesh = new ();

    protected Color _color = Color.White;

    public Color Color
    {
        get => _color;
        set
        {
            if (_color != value)
            {
                _color = value;
                _dirty = true;
            }
        }
    }

    protected float _depth = 0f;

    public float Depth
    {
        get => _depth;
        set
        {
            if (_depth != value)
            {
                _depth = value;
                _dirty = true;
            }
        }
    }

    protected bool _dirty = true;

    public override void Awake ()
    {
        Transform.OnTransformChanged += OnTransformChanged;
    }

    public override void Destroy ()
    {
        Transform.OnTransformChanged -= OnTransformChanged;
    }

    protected virtual void OnTransformChanged (Transform.ChangedFlags changedFlags)
    {
        _dirty = true;
    }

    protected abstract void PopulateMesh ();

    public abstract void Draw (RenderManager render);
}
