using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

internal class StandardBatcher<T> : RenderBatcher where T : struct, IVertexType
{
    public static VertexDeclaration VertexDeclaration => VertexDeclarationCache<T>.VertexDeclaration;

    private const int IndexCount = 3;

    private const int VertexCount = 3;

    private const int InitialCapacity = 32;

    private readonly IBatchEncoder<T> _batchEncoder;

    private readonly int _batchSize;

    private int _batchCount;

    private ushort[] _batchIndices;

    private T[] _batchVertices;

    private readonly DynamicIndexBuffer _indexBuffer;

    private readonly DynamicVertexBuffer _vertexBuffer;

    public StandardBatcher (GraphicsDevice graphicsDevice, string name, IBatchEncoder<T> batchEncoder, int batchSize = ushort.MaxValue / IndexCount)
        : base (graphicsDevice, name)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan (batchSize, ushort.MaxValue / IndexCount);

        _batchEncoder = batchEncoder;
        _batchSize = batchSize;

        _batchCount = 0;
        _batchIndices = new ushort[InitialCapacity * IndexCount];
        _batchVertices = new T[InitialCapacity * VertexCount];

        _indexBuffer = new DynamicIndexBuffer (graphicsDevice, IndexElementSize.SixteenBits, _batchSize * IndexCount, BufferUsage.WriteOnly);
        _vertexBuffer = new DynamicVertexBuffer (graphicsDevice, VertexDeclaration, _batchSize * VertexCount, BufferUsage.WriteOnly);
    }

    public override void Batch (Mesh mesh)
    {
        EnsureIndexArrayCapacity (mesh.Indices.Length);
        EnsureVertexArrayCapacity (mesh.Vertices.Length);

        int batchCount = mesh.Indices.Length / IndexCount;

        mesh.Indices.CopyTo (_batchIndices, _batchCount * IndexCount);
        _batchEncoder.Encode (_batchVertices, _batchCount * VertexCount, mesh);

        _batchCount += batchCount;
    }

    private void EnsureIndexArrayCapacity (int count)
    {
        int size = _batchCount * IndexCount + count;

        if (size >= _batchIndices.Length)
        {
            int newSize = int.Max (_batchIndices.Length, InitialCapacity * IndexCount);

            while (newSize < size)
            {
                newSize *= 2;
            }

            Array.Resize (ref _batchIndices, newSize);
        }
    }

    private void EnsureVertexArrayCapacity (int count)
    {
        int size = _batchCount * VertexCount + count;

        if (size >= _batchVertices.Length)
        {
            int newSize = int.Max (_batchVertices.Length, InitialCapacity * VertexCount);

            while (newSize < size)
            {
                newSize *= 2;
            }

            Array.Resize (ref _batchVertices, newSize);
        }
    }

    public override void DrawBatch (Material material, MaterialPropertyBlock? properties, Texture? texture)
    {
        if (_batchCount == 0)
        {
            return;
        }

        material.ApplyStates (_graphicsDevice);
        material.ApplyProperties (properties);

        int batchIndex = 0;
        int batchCount = _batchCount;

        while (batchCount > 0)
        {
            int batchCountToProcess = batchCount;

            if (batchCountToProcess > _batchSize)
            {
                batchCountToProcess = _batchSize;
            }

            FlushArray (material, texture, batchIndex, batchCountToProcess);

            batchIndex += batchCountToProcess;
            batchCount -= batchCountToProcess;
        }

        _batchCount = 0;
    }

    private void FlushArray (Material material, Texture? texture, int batchIndex, int batchCount)
    {
        if (batchCount <= 0)
        {
            return;
        }

        _indexBuffer.SetData (_batchIndices, batchIndex * IndexCount, batchCount * IndexCount, SetDataOptions.Discard);
        _vertexBuffer.SetData (_batchVertices, batchIndex * VertexCount, batchCount * VertexCount, SetDataOptions.Discard);

        _graphicsDevice.Indices = _indexBuffer;
        _graphicsDevice.SetVertexBuffer (_vertexBuffer);

        foreach (EffectPass pass in material.Effect.CurrentTechnique.Passes)
        {
            pass.Apply ();

            _graphicsDevice.Textures[0] = texture;
            _graphicsDevice.DrawIndexedPrimitives (PrimitiveType.TriangleList, 0, 0, batchCount);
        }
    }
}
