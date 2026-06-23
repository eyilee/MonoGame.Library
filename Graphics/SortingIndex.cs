using System;

namespace MonoGame.Library.Graphics;

public readonly struct SortingIndex (int index, ulong sortKey) : IComparable<SortingIndex>
{
    public int Index { get; } = index;

    public ulong SortKey { get; } = sortKey;

    public readonly int CompareTo (SortingIndex other)
    {
        int r = SortKey.CompareTo (other.SortKey);
        if (r != 0)
        {
            return r;
        }

        return Index.CompareTo (other.Index);
    }
}
