using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library.Graphics;

public class RenderManager
{
    private readonly struct SortKey (int index, ulong value) : IComparable<SortKey>
    {
        public int Index { get; } = index;

        public ulong Value { get; } = value;

        public readonly int CompareTo (SortKey other)
        {
            int r = Value.CompareTo (other.Value);
            if (r != 0)
            {
                return r;
            }

            return Index.CompareTo (other.Index);
        }
    }

    private SortKey[] _sortKeys = new SortKey[32];

    private RenderCommand[] _commands = new RenderCommand[32];

    private int _count = 0;

    public RenderManager (GraphicsDevice graphicsDevice)
    {
        _ = new QuadBatcher<VertexPositionColorTexture> (graphicsDevice, "Sprite", new SpriteBatchEncoder ());
        _ = new QuadInstanceBatcher<VertexSdfInstance> (graphicsDevice, "SdfInstance", new SdfInstanceBatchEncoder ());
    }

    public void Enqueue (in RenderCommand command)
    {
        EnsureArrayCapacity ();

        int index = _count++;
        _sortKeys[index] = new SortKey (index, command.SortKey);
        _commands[index] = command;
    }

    private void EnsureArrayCapacity ()
    {
        if (_count < _sortKeys.Length)
        {
            return;
        }

        Array.Resize (ref _sortKeys, _sortKeys.Length * 2);
        Array.Resize (ref _commands, _commands.Length * 2);
    }

    public void Draw ()
    {
        if (_count == 0)
        {
            return;
        }

        Array.Sort (_sortKeys, 0, _count);

        int batchStartIndex = 0;
        while (batchStartIndex < _count)
        {
            int firstCommandIndex = _sortKeys[batchStartIndex].Index;
            ref RenderCommand firstCommand = ref _commands[firstCommandIndex];

            int batchEndIndex = batchStartIndex + 1;
            while (batchEndIndex < _count)
            {
                int nextCommandIndex = _sortKeys[batchEndIndex].Index;
                ref RenderCommand nextCommand = ref _commands[nextCommandIndex];

                if (!CanBatch (firstCommand, nextCommand))
                {
                    break;
                }

                batchEndIndex++;
            }

            if (RenderBatcherRegistry.TryGetValue (firstCommand.BatcherId, out RenderBatcher? batcher) && batcher != null)
            {
                for (int i = batchStartIndex; i < batchEndIndex; i++)
                {
                    int commandIndex = _sortKeys[i].Index;
                    ref RenderCommand command = ref _commands[commandIndex];
                    batcher.Batch (command.Mesh);
                }

                batcher.DrawBatch (firstCommand.Material, firstCommand.Properties, firstCommand.Texture?.Texture);
            }

            batchStartIndex = batchEndIndex;
        }

        _count = 0;
    }

    private static bool CanBatch (in RenderCommand firstCommand, in RenderCommand nextCommand)
    {
        return ReferenceEquals (nextCommand.Material, firstCommand.Material)
            && ReferenceEquals (nextCommand.Properties, firstCommand.Properties)
            && ReferenceEquals (nextCommand.Texture, firstCommand.Texture);
    }
}
