using Microsoft.Xna.Framework;

namespace MonoGame.Library.Graphics.Shapes;

public abstract class Shape
{
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

    protected readonly Mesh _mesh = new ();

    protected Color _color = Color.White;

    protected float _depth = 0f;

    protected bool _dirty = true;

    protected abstract void PopulateMesh ();

    public abstract void Draw (RenderManager render);
}
