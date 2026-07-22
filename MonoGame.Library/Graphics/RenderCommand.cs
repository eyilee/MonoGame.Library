namespace MonoGame.Library.Graphics;

public readonly struct RenderCommand
{
    public MaterialInstance Material { get; }

    public readonly ushort BatcherId => Material.BatcherId;

    public MaterialPropertyBlock? Properties { get; }

    public Mesh Mesh { get; }

    public TextureResource? Texture { get; }

    public float Depth { get; }

    public ulong SortKey { get; }

    public RenderCommand (MaterialInstance material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource? texture, float depth = 0f)
    {
        Material = material;
        Properties = properties;
        Mesh = mesh;
        Texture = texture;
        Depth = depth;
        SortKey = GetSortKey ();
    }

    public RenderCommand (MaterialInstance material, Mesh mesh, TextureResource? texture, float depth = 0f)
        : this (material, null, mesh, texture, depth)
    {
    }

    public RenderCommand (MaterialInstance material, Mesh mesh, float depth = 0f)
        : this (material, null, mesh, null, depth)
    {
    }

    private ulong GetSortKey ()
    {
        ushort depthBits = (ushort)(float.Clamp (Depth, 0f, 1f) * ushort.MaxValue);

        return (ulong)Material.Id << 32
            | (ulong)(Texture?.Id ?? 0) << 16
            | depthBits;
    }
}
