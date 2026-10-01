using Microsoft.Xna.Framework;
using MonoGame.Library.Utilities;
using System.Collections.Generic;

namespace MonoGame.Library.Graphics.Shapes;

public class PolygonShape : Shape
{
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

    public List<Vector2> Vertices => _vertices;

    protected float _rotation = 0f;

    protected readonly List<Vector2> _vertices = [];

    protected Vector2 _centroid = Vector2.Zero;

    public void SetVertices (List<Vector2> vertices)
    {
        _centroid = CalculateCentroid (vertices);

        _vertices.Clear ();
        _vertices.Capacity = vertices.Count;

        foreach (Vector2 vertex in vertices)
        {
            _vertices.Add (vertex - _centroid);
        }

        _dirty = true;
    }

    protected override void PopulateMesh ()
    {
        CalculateIndices ();
        CalculateVertices ();
        CalculateUVs ();
        CalculateColors ();
    }

    public override void Draw (RenderManager render)
    {
        if (_dirty)
        {
            PopulateMesh ();
            _dirty = false;
        }

        render.Enqueue (new RenderCommand (Materials.Standard, _mesh, Textures.Pixel, _depth));
    }

    private static Vector2 CalculateCentroid (List<Vector2> vertices)
    {
        Vector2 centroid = Vector2.Zero;
        float area = 0f;

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 p0 = vertices[i];
            Vector2 p1 = vertices[(i + 1) % vertices.Count];

            float cross = p0.X * p1.Y - p1.X * p0.Y;

            area += cross;

            centroid.X += (p0.X + p1.X) * cross;
            centroid.Y += (p0.Y + p1.Y) * cross;
        }

        area *= 0.5f;

        if (float.Abs (area) < float.Epsilon)
        {
            return Vector2.Zero;
        }

        centroid /= 6f * area;

        return centroid;
    }

    private void CalculateIndices ()
    {
        List<ushort> triangles = [];
        List<ushort> indices = new (_vertices.Count);

        for (int i = 0; i < _vertices.Count; i++)
        {
            indices.Add ((ushort)i);
        }

        while (indices.Count > 3)
        {
            if (!FindEarIndex (_vertices, indices, out int earIndex))
            {
                break;
            }

            ushort i0 = indices[(earIndex - 1 + indices.Count) % indices.Count];
            ushort i1 = indices[earIndex];
            ushort i2 = indices[(earIndex + 1) % indices.Count];

            float z = (_vertices[i1] - _vertices[i0]).Cross (_vertices[i2] - _vertices[i0]);
            if (z <= 0f)
            {
                (i1, i2) = (i2, i1);
            }

            triangles.AddRange ([i0, i1, i2]);
            indices.RemoveAt (earIndex);
        }

        if (indices.Count == 3)
        {
            ushort i0 = indices[0];
            ushort i1 = indices[1];
            ushort i2 = indices[2];

            float z = (_vertices[i1] - _vertices[i0]).Cross (_vertices[i2] - _vertices[i0]);
            if (z <= 0f)
            {
                (i1, i2) = (i2, i1);
            }

            triangles.AddRange ([i0, i1, i2]);
        }

        _mesh.SetIndices ([.. triangles]);
    }

    private static bool FindEarIndex (List<Vector2> vertices, List<ushort> indices, out int index)
    {
        for (index = 0; index < indices.Count; index++)
        {
            ushort i0 = indices[(index - 1 + indices.Count) % indices.Count];
            ushort i1 = indices[index];
            ushort i2 = indices[(index + 1) % indices.Count];
            Vector2 v0 = vertices[i0];
            Vector2 v1 = vertices[i1];
            Vector2 v2 = vertices[i2];

            float z = (v1 - v0).Cross (v2 - v1);
            if (z <= 0)
            {
                continue;
            }

            bool contains = false;

            for (int other = 0; other < indices.Count; other++)
            {
                ushort io = indices[other];
                Vector2 vo = vertices[io];

                if (io == i0 || io == i1 || io == i2)
                {
                    continue;
                }

                bool z1 = (v0 - vo).Cross (v1 - vo) >= 0;
                bool z2 = (v1 - vo).Cross (v2 - vo) >= 0;
                bool z3 = (v2 - vo).Cross (v0 - vo) >= 0;

                if (z1 == z2 && z2 == z3)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                return true;
            }
        }

        return false;
    }

    private void CalculateVertices ()
    {
        List<Vector3> vertices = new (_vertices.Count);

        foreach (Vector2 vertex in _vertices)
        {
            vertices.Add (new Vector3 (_centroid + Vector2.Rotate (vertex, _rotation), 0f));
        }

        _mesh.SetVertices ([.. vertices]);
    }

    private void CalculateUVs ()
    {
        Vector2 min = _vertices[0];
        Vector2 max = min;

        foreach (Vector2 vertex in _vertices)
        {
            min.X = float.Min (min.X, vertex.X);
            min.Y = float.Min (min.Y, vertex.Y);
            max.X = float.Max (max.X, vertex.X);
            max.Y = float.Max (max.Y, vertex.Y);
        }

        float width = max.X - min.X;
        float height = max.Y - min.Y;

        List<Vector2> uvs = new (_vertices.Count);

        foreach (Vector2 vertex in _vertices)
        {
            float u = width < float.Epsilon ? 0f : (vertex.X - min.X) / width;
            float v = height < float.Epsilon ? 0f : (vertex.Y - min.Y) / height;
            uvs.Add (new Vector2 (u, v));
        }

        _mesh.SetUVs ([.. uvs]);
    }

    private void CalculateColors ()
    {
        List<Color> colors = new (_vertices.Count);

        for (int i = 0; i < _vertices.Count; i++)
        {
            colors.Add (_color);
        }

        _mesh.SetColors ([.. colors]);
    }
}
