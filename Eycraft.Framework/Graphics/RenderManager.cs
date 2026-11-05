using System;

namespace Eycraft.Framework.Graphics;

public class RenderManager
{
    private readonly struct BatchKey : IComparable<BatchKey>
    {
        public MaterialPropertyBlock? Properties { get; }

        public ushort MaterialId { get; }

        public ulong TextureKey { get; }

        public BatchKey (in RenderCommand command)
        {
            Properties = command.Properties;
            MaterialId = command.Material.Id;
            TextureKey = (ulong)(command.Texture != null ? command.Texture.Id : 0);

            if (command.Textures != null)
            {
                for (int i = 0; i < command.Textures.Length && i < 4; i++)
                {
                    TextureKey |= ((ulong)command.Textures[i].Id << (i * 16));
                }
            }
        }

        public static bool operator == (BatchKey left, BatchKey right) => left.Equals (right);

        public static bool operator != (BatchKey left, BatchKey right) => !left.Equals (right);

        public bool Equals (BatchKey other)
        {
            return ReferenceEquals (Properties, other.Properties)
                && MaterialId.Equals (other.MaterialId)
                && TextureKey.Equals (other.TextureKey);
        }

        public override bool Equals (object? obj)
        {
            return obj is BatchKey other && Equals (other);
        }

        public override int GetHashCode ()
        {
            return HashCode.Combine (Properties, MaterialId, TextureKey);
        }

        public int CompareTo (BatchKey other)
        {
            int value = MaterialId.CompareTo (other.MaterialId);

            if (value != 0)
            {
                return value;
            }

            return TextureKey.CompareTo (other.TextureKey);
        }
    }

    private readonly struct SortKey (int index, ushort depth, BatchKey batchKey) : IComparable<SortKey>
    {
        public int Index { get; } = index;

        public ushort Depth { get; } = depth;

        public BatchKey BatchKey { get; } = batchKey;

        public readonly int CompareTo (SortKey other)
        {
            int value = Depth.CompareTo (other.Depth);

            if (value != 0)
            {
                return value;
            }

            value = BatchKey.CompareTo (other.BatchKey);

            if (value != 0)
            {
                return value;
            }

            return Index.CompareTo (other.Index);
        }
    }

    private SortKey[] _sortKeys = new SortKey[32];

    private RenderCommand[] _commands = new RenderCommand[32];

    private int _count = 0;

    public void Enqueue (in RenderCommand command)
    {
        EnsureArrayCapacity ();

        int index = _count++;
        _sortKeys[index] = new SortKey (index, command.Depth, new BatchKey (command));
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
            ref SortKey firstKey = ref _sortKeys[batchStartIndex];
            ref RenderCommand firstCommand = ref _commands[firstKey.Index];

            int batchEndIndex = batchStartIndex + 1;

            while (batchEndIndex < _count)
            {
                ref SortKey nextKey = ref _sortKeys[batchEndIndex];
                ref RenderCommand nextCommand = ref _commands[nextKey.Index];

                if (!CanBatch (firstKey, nextKey))
                {
                    break;
                }

                batchEndIndex++;
            }

            RenderBatcher batcher = firstCommand.Material.RenderBatcher;

            for (int i = batchStartIndex; i < batchEndIndex; i++)
            {
                int commandIndex = _sortKeys[i].Index;
                ref RenderCommand command = ref _commands[commandIndex];

                if (!batcher.CanBatch (command.Mesh))
                {
                    batcher.DrawBatch (firstCommand.Material, firstCommand.Properties, firstCommand.Texture, firstCommand.Textures);
                }

                batcher.Batch (command.Mesh);
            }

            batcher.DrawBatch (firstCommand.Material, firstCommand.Properties, firstCommand.Texture, firstCommand.Textures);

            batchStartIndex = batchEndIndex;
        }

        _count = 0;
    }

    private static bool CanBatch (in SortKey firstKey, in SortKey nextKey)
    {
        return firstKey.BatchKey.Equals (nextKey.BatchKey);
    }
}
