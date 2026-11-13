using Eycraft.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Eycraft.Framework.Objects;

public class SdfParabola (Entity entity) : SdfShape (entity)
{
    private Vector2 _offset = Vector2.Zero;

    public Vector2 Offset
    {
        get => _offset;
        set
        {
            if (_offset != value)
            {
                _offset = value;
                _dirty = true;
            }
        }
    }

    private float _rotation = 0f;

    public float Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _size = Vector2.Zero;

    public Vector2 Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                _dirty = true;
            }
        }
    }

    private Vector2 _vertex = Vector2.Zero;

    public Vector2 Vertex
    {
        get => _vertex;
        set
        {
            if (_vertex != value)
            {
                _vertex = value;
                _dirty = true;
            }
        }
    }

    private float _focalLength = 0f;

    public float FocalLength
    {
        get => _focalLength;
        set
        {
            if (_focalLength != value)
            {
                _focalLength = value;
                _dirty = true;
            }
        }
    }

    protected override void PopulateMesh ()
    {
        Vector2 position = Transform.Position + Vector2.Rotate (Offset, Transform.Rotation);
        float rotation = Transform.Rotation + Rotation;
        float k = 1f / 4f;

        if (FocalLength != 0f)
        {
            k = 1f / (4f * FocalLength);
        }

        _mesh.SetUVs ([new Vector3 (position, Depth)]);
        _mesh.SetUV1s ([new Vector4 (rotation, Size.X, Size.Y, Thickness)]);
        _mesh.SetUV2s ([new Vector4 (float.Pi / 2f, Vertex.X, Vertex.Y, k)]);
        _mesh.SetColors ([Color]);
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (RenderCommand.CreateSingleTexture (Material.SdfParabola, _mesh, SortingLayer, SortingOrder));
    }
}
