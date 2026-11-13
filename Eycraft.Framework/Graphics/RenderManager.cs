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
                    TextureKey |= (ulong)command.Textures[i].Id << (i * 16);
                }
            }
        }

        public static bool operator == (BatchKey lhs, BatchKey rhs) => lhs.Equals (rhs);

        public static bool operator != (BatchKey lhs, BatchKey rhs) => !lhs.Equals (rhs);

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

    private readonly struct RenderKey (int index, in RenderCommand command) : IComparable<RenderKey>
    {
        public int Index { get; } = index;

        public ushort RenderQueue { get; } = command.Material.RenderQueue;

        public RenderQueueType RenderQueueType { get; } = command.Material.RenderQueueType;

        public uint SortingKey { get; } = command.SortingKey;

        public BatchKey BatchKey { get; } = new BatchKey (command);

        public readonly int CompareTo (RenderKey other)
        {
            int value = RenderQueue.CompareTo (other.RenderQueue);

            if (value != 0)
            {
                return value;
            }

            value = SortingKey.CompareTo (other.SortingKey);

            if (value != 0)
            {
                return value;
            }

            if (RenderQueueType == RenderQueueType.Opaque)
            {
                value = BatchKey.CompareTo (other.BatchKey);

                if (value != 0)
                {
                    return value;
                }
            }

            return Index.CompareTo (other.Index);
        }
    }

    private RenderKey[] _keys = new RenderKey[32];

    private RenderCommand[] _commands = new RenderCommand[32];

    private int _count = 0;

    public void Enqueue (in RenderCommand command)
    {
        EnsureArrayCapacity ();

        int index = _count++;
        _keys[index] = new RenderKey (index, command);
        _commands[index] = command;
    }

    private void EnsureArrayCapacity ()
    {
        if (_count < _keys.Length)
        {
            return;
        }

        Array.Resize (ref _keys, _keys.Length * 2);
        Array.Resize (ref _commands, _commands.Length * 2);
    }

    public void Draw ()
    {
        if (_count == 0)
        {
            return;
        }

        Array.Sort (_keys, 0, _count);

        int batchStartIndex = 0;

        while (batchStartIndex < _count)
        {
            ref RenderKey firstKey = ref _keys[batchStartIndex];
            ref RenderCommand firstCommand = ref _commands[firstKey.Index];

            int batchEndIndex = batchStartIndex + 1;

            while (batchEndIndex < _count)
            {
                ref RenderKey nextKey = ref _keys[batchEndIndex];
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
                int commandIndex = _keys[i].Index;
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

    private static bool CanBatch (in RenderKey firstKey, in RenderKey nextKey)
    {
        return firstKey.BatchKey.Equals (nextKey.BatchKey);
    }
}
