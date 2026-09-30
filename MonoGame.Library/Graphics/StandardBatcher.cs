using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

internal class StandardBatcher<T> : RenderBatcher where T : struct, IVertexType
{
    public static VertexDeclaration VertexDeclaration => VertexDeclarationCache<T>.VertexDeclaration;

    private const int InitialCapacity = 32;

    private readonly IBatchEncoder<T> _batchEncoder;

    private readonly int _batchSize;

    private int _indexCount;

    private int _vertexCount;

    private ushort[] _batchIndices;

    private T[] _batchVertices;

    private readonly DynamicIndexBuffer _indexBuffer;

    private readonly DynamicVertexBuffer _vertexBuffer;

    public StandardBatcher (GraphicsDevice graphicsDevice, string name, IBatchEncoder<T> batchEncoder, int batchSize = ushort.MaxValue)
        : base (graphicsDevice, name)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan (batchSize, ushort.MaxValue);

        _batchEncoder = batchEncoder;
        _batchSize = batchSize;

        _indexCount = 0;
        _vertexCount = 0;
        _batchIndices = new ushort[InitialCapacity];
        _batchVertices = new T[InitialCapacity];

        _indexBuffer = new DynamicIndexBuffer (graphicsDevice, IndexElementSize.SixteenBits, _batchSize, BufferUsage.WriteOnly);
        _vertexBuffer = new DynamicVertexBuffer (graphicsDevice, VertexDeclaration, _batchSize, BufferUsage.WriteOnly);
    }

    public override bool CanBatch (Mesh mesh)
    {
        if (_indexCount + mesh.Indices.Length > _batchSize)
        {
            return false;
        }

        if (_vertexCount + mesh.Vertices.Length > _batchSize)
        {
            return false;
        }

        return true;
    }

    public override void Batch (Mesh mesh)
    {
        EnsureIndexArrayCapacity (mesh.Indices.Length);
        EnsureVertexArrayCapacity (mesh.Vertices.Length);

        for (int i = 0; i < mesh.Indices.Length; i++)
        {
            _batchIndices[i + _indexCount] = (ushort)(mesh.Indices[i] + _vertexCount);
        }

        _batchEncoder.Encode (_batchVertices, _vertexCount, mesh);

        _indexCount += mesh.Indices.Length;
        _vertexCount += mesh.Vertices.Length;
    }

    private void EnsureIndexArrayCapacity (int count)
    {
        int size = _indexCount + count;

        if (size >= _batchIndices.Length)
        {
            int newSize = int.Max (_batchIndices.Length, InitialCapacity);

            while (newSize < size)
            {
                newSize *= 2;
            }

            Array.Resize (ref _batchIndices, newSize);
        }
    }

    private void EnsureVertexArrayCapacity (int count)
    {
        int size = _vertexCount + count;

        if (size >= _batchVertices.Length)
        {
            int newSize = int.Max (_batchVertices.Length, InitialCapacity);

            while (newSize < size)
            {
                newSize *= 2;
            }

            Array.Resize (ref _batchVertices, newSize);
        }
    }

    public override void DrawBatch (Material material, MaterialPropertyBlock? properties, Texture? texture)
    {
        if (_indexCount == 0 && _vertexCount == 0)
        {
            return;
        }

        material.ApplyStates (_graphicsDevice);
        material.ApplyProperties (properties);

        _indexBuffer.SetData (_batchIndices, 0, _indexCount, SetDataOptions.Discard);
        _vertexBuffer.SetData (_batchVertices, 0, _vertexCount, SetDataOptions.Discard);

        _graphicsDevice.Indices = _indexBuffer;
        _graphicsDevice.SetVertexBuffer (_vertexBuffer);

        foreach (EffectPass pass in material.Effect.CurrentTechnique.Passes)
        {
            pass.Apply ();

            _graphicsDevice.Textures[0] = texture;
            _graphicsDevice.DrawIndexedPrimitives (PrimitiveType.TriangleList, 0, 0, _indexCount / 3);
        }

        _indexCount = 0;
        _vertexCount = 0;
    }
}
