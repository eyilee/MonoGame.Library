namespace Eycraft.Framework.Graphics;

public readonly struct RenderCommand
{
    public Material Material { get; }

    public MaterialPropertyBlock? Properties { get; }

    public Mesh Mesh { get; }

    public TextureResource? Texture { get; }

    public TextureResource[]? Textures { get; }

    public ushort Depth { get; }

    public RenderCommand (Material material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource? texture, TextureResource[]? textures, float depth = 0f)
    {
        Material = material;
        Properties = properties;
        Mesh = mesh;
        Texture = texture;
        Textures = textures;
        Depth = (ushort)(float.Clamp (depth, 0f, 1f) * ushort.MaxValue);
    }

    public static RenderCommand CreateSingleTexture (Material material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource? texture, float depth = 0f)
    {
        return new RenderCommand (material, properties, mesh, texture, null, depth);
    }

    public static RenderCommand CreateSingleTexture (Material material, Mesh mesh, TextureResource? texture, float depth = 0f)
    {
        return new RenderCommand (material, null, mesh, texture, null, depth);
    }

    public static RenderCommand CreateSingleTexture (Material material, Mesh mesh, float depth = 0f)
    {
        return new RenderCommand (material, null, mesh, null, null, depth);
    }

    public static RenderCommand CreateMultipleTexture (Material material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource[]? textures, float depth = 0f)
    {
        return new RenderCommand (material, properties, mesh, null, textures, depth);
    }

    public static RenderCommand CreateMultipleTexture (Material material, Mesh mesh, TextureResource[]? textures, float depth = 0f)
    {
        return new RenderCommand (material, null, mesh, null, textures, depth);
    }

    public static RenderCommand CreateMultipleTexture (Material material, Mesh mesh, float depth = 0f)
    {
        return new RenderCommand (material, null, mesh, null, null, depth);
    }
}
