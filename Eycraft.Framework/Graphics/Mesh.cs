using Microsoft.Xna.Framework;
using System;

namespace Eycraft.Framework.Graphics;

public class Mesh
{
    public ReadOnlySpan<ushort> Indices => GetArray (_indices, _indexCount);

    public ReadOnlySpan<Vector3> Vertices => GetArray (_vertices, _vertexCount);

    public ReadOnlySpan<Color> Colors => GetArray (_colors, _colorCount);

    public ReadOnlySpan<Vector4> UVs => GetArray (_uvs, _uvCount);

    public ReadOnlySpan<Vector4> UV1s => GetArray (_uv1s, _uv1Count);

    public ReadOnlySpan<Vector4> UV2s => GetArray (_uv2s, _uv2Count);

    public ReadOnlySpan<Vector4> UV3s => GetArray (_uv3s, _uv3Count);

    public ReadOnlySpan<Vector4> UV4s => GetArray (_uv4s, _uv4Count);

    public ReadOnlySpan<Vector4> UV5s => GetArray (_uv5s, _uv5Count);

    public ReadOnlySpan<Vector4> UV6s => GetArray (_uv6s, _uv6Count);

    public ReadOnlySpan<Vector4> UV7s => GetArray (_uv7s, _uv7Count);

    protected ushort[] _indices = [];

    protected int _indexCount = 0;

    protected Vector3[] _vertices = [];

    protected int _vertexCount = 0;

    protected Color[] _colors = [];

    protected int _colorCount = 0;

    protected Vector4[] _uvs = [];

    protected int _uvCount = 0;

    protected Vector4[] _uv1s = [];

    protected int _uv1Count = 0;

    protected Vector4[] _uv2s = [];

    protected int _uv2Count = 0;

    protected Vector4[] _uv3s = [];

    protected int _uv3Count = 0;

    protected Vector4[] _uv4s = [];

    protected int _uv4Count = 0;

    protected Vector4[] _uv5s = [];

    protected int _uv5Count = 0;

    protected Vector4[] _uv6s = [];

    protected int _uv6Count = 0;

    protected Vector4[] _uv7s = [];

    protected int _uv7Count = 0;

    public void SetIndices (ushort[] indices) => SetArray (indices, ref _indices, ref _indexCount);

    public void SetVertices (Vector3[] vertices) => SetArray (vertices, ref _vertices, ref _vertexCount);

    public void SetColors (Color[] colors) => SetArray (colors, ref _colors, ref _colorCount);

    public void SetUVs (float[] uvs) => SetUVs (uvs, ref _uvs, ref _uvCount);

    public void SetUVs (Vector2[] uvs) => SetUVs (uvs, ref _uvs, ref _uvCount);

    public void SetUVs (Vector3[] uvs) => SetUVs (uvs, ref _uvs, ref _uvCount);

    public void SetUVs (Vector4[] uvs) => SetUVs (uvs, ref _uvs, ref _uvCount);

    public void SetUV1s (float[] uvs) => SetUVs (uvs, ref _uv1s, ref _uv1Count);

    public void SetUV1s (Vector2[] uvs) => SetUVs (uvs, ref _uv1s, ref _uv1Count);

    public void SetUV1s (Vector3[] uvs) => SetUVs (uvs, ref _uv1s, ref _uv1Count);

    public void SetUV1s (Vector4[] uvs) => SetUVs (uvs, ref _uv1s, ref _uv1Count);

    public void SetUV2s (float[] uvs) => SetUVs (uvs, ref _uv2s, ref _uv2Count);

    public void SetUV2s (Vector2[] uvs) => SetUVs (uvs, ref _uv2s, ref _uv2Count);

    public void SetUV2s (Vector3[] uvs) => SetUVs (uvs, ref _uv2s, ref _uv2Count);

    public void SetUV2s (Vector4[] uvs) => SetUVs (uvs, ref _uv2s, ref _uv2Count);

    public void SetUV3s (float[] uvs) => SetUVs (uvs, ref _uv3s, ref _uv3Count);

    public void SetUV3s (Vector2[] uvs) => SetUVs (uvs, ref _uv3s, ref _uv3Count);

    public void SetUV3s (Vector3[] uvs) => SetUVs (uvs, ref _uv3s, ref _uv3Count);

    public void SetUV3s (Vector4[] uvs) => SetUVs (uvs, ref _uv3s, ref _uv3Count);

    public void SetUV4s (float[] uvs) => SetUVs (uvs, ref _uv4s, ref _uv4Count);

    public void SetUV4s (Vector2[] uvs) => SetUVs (uvs, ref _uv4s, ref _uv4Count);

    public void SetUV4s (Vector3[] uvs) => SetUVs (uvs, ref _uv4s, ref _uv4Count);

    public void SetUV4s (Vector4[] uvs) => SetUVs (uvs, ref _uv4s, ref _uv4Count);

    public void SetUV5s (float[] uvs) => SetUVs (uvs, ref _uv5s, ref _uv5Count);

    public void SetUV5s (Vector2[] uvs) => SetUVs (uvs, ref _uv5s, ref _uv5Count);

    public void SetUV5s (Vector3[] uvs) => SetUVs (uvs, ref _uv5s, ref _uv5Count);

    public void SetUV5s (Vector4[] uvs) => SetUVs (uvs, ref _uv5s, ref _uv5Count);

    public void SetUV6s (float[] uvs) => SetUVs (uvs, ref _uv6s, ref _uv6Count);

    public void SetUV6s (Vector2[] uvs) => SetUVs (uvs, ref _uv6s, ref _uv6Count);

    public void SetUV6s (Vector3[] uvs) => SetUVs (uvs, ref _uv6s, ref _uv6Count);

    public void SetUV6s (Vector4[] uvs) => SetUVs (uvs, ref _uv6s, ref _uv6Count);

    public void SetUV7s (float[] uvs) => SetUVs (uvs, ref _uv7s, ref _uv7Count);

    public void SetUV7s (Vector2[] uvs) => SetUVs (uvs, ref _uv7s, ref _uv7Count);

    public void SetUV7s (Vector3[] uvs) => SetUVs (uvs, ref _uv7s, ref _uv7Count);

    public void SetUV7s (Vector4[] uvs) => SetUVs (uvs, ref _uv7s, ref _uv7Count);

    public void Clear ()
    {
        _indexCount = 0;
        _vertexCount = 0;
        _colorCount = 0;
        _uvCount = 0;
        _uv1Count = 0;
        _uv2Count = 0;
        _uv3Count = 0;
        _uv4Count = 0;
        _uv5Count = 0;
        _uv6Count = 0;
        _uv7Count = 0;
    }

    private static ReadOnlySpan<T> GetArray<T> (T[] array, int count) where T : struct
    {
        return array.AsSpan (0, count);
    }

    private static void SetArray<T> (T[] from, ref T[] to, ref int count) where T : struct
    {
        EnsureCapacity (ref to, from.Length);

        from.CopyTo (to, 0);

        count = from.Length;
    }

    private static void EnsureCapacity<T> (ref T[] array, int count)
    {
        if (array.Length < count)
        {
            array = new T[Align (count)];
        }
    }

    private static int Align (int value)
    {
        if (value <= 1)
        {
            return 1;
        }

        value--;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value++;

        return value;
    }

    private static void SetUVs (float[] from, ref Vector4[] to, ref int count)
    {
        EnsureCapacity (ref to, from.Length);

        for (int i = 0; i < from.Length; i++)
        {
            to[i] = new Vector4 (from[i], 0f, 0f, 0f);
        }

        count = from.Length;
    }

    private static void SetUVs (Vector2[] from, ref Vector4[] to, ref int count)
    {
        EnsureCapacity (ref to, from.Length);

        for (int i = 0; i < from.Length; i++)
        {
            to[i] = new Vector4 (from[i], 0f, 0f);
        }

        count = from.Length;
    }

    private static void SetUVs (Vector3[] from, ref Vector4[] to, ref int count)
    {
        EnsureCapacity (ref to, from.Length);

        for (int i = 0; i < from.Length; i++)
        {
            to[i] = new Vector4 (from[i], 0f);
        }

        count = from.Length;
    }

    private static void SetUVs (Vector4[] from, ref Vector4[] to, ref int count)
    {
        EnsureCapacity (ref to, from.Length);

        from.CopyTo (to, 0);

        count = from.Length;
    }
}
