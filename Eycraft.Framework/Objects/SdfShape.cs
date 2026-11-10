namespace Eycraft.Framework.Objects;

public abstract class SdfShape (Entity entity) : Shape (entity)
{
    protected float _thickness = 1f;

    public float Thickness
    {
        get => _thickness;
        set
        {
            if (_thickness != value)
            {
                _thickness = value;
                _dirty = true;
            }
        }
    }
}
