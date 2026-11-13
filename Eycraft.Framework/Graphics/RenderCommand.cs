namespace Eycraft.Framework.Graphics;

public readonly struct RenderCommand (Material material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource? texture, TextureResource[]? textures, short sortingLayer = 0, short sortingOrder = 0)
{
    public Material Material { get; } = material;

    public MaterialPropertyBlock? Properties { get; } = properties;

    public Mesh Mesh { get; } = mesh;

    public TextureResource? Texture { get; } = texture;

    public TextureResource[]? Textures { get; } = textures;

    public uint SortingKey { get; } = ((uint)(sortingLayer - short.MinValue) << 16) | (uint)(sortingOrder - short.MinValue);

    public static RenderCommand CreateSingleTexture (Material material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource? texture, short sortingLayer = 0, short sortingOrder = 0)
    {
        return new RenderCommand (material, properties, mesh, texture, null, sortingLayer, sortingOrder);
    }

    public static RenderCommand CreateSingleTexture (Material material, Mesh mesh, TextureResource? texture, short sortingLayer = 0, short sortingOrder = 0)
    {
        return new RenderCommand (material, null, mesh, texture, null, sortingLayer, sortingOrder);
    }

    public static RenderCommand CreateSingleTexture (Material material, Mesh mesh, short sortingLayer = 0, short sortingOrder = 0)
    {
        return new RenderCommand (material, null, mesh, null, null, sortingLayer, sortingOrder);
    }

    public static RenderCommand CreateMultipleTexture (Material material, MaterialPropertyBlock? properties, Mesh mesh, TextureResource[]? textures, short sortingLayer = 0, short sortingOrder = 0)
    {
        return new RenderCommand (material, properties, mesh, null, textures, sortingLayer, sortingOrder);
    }

    public static RenderCommand CreateMultipleTexture (Material material, Mesh mesh, TextureResource[]? textures, short sortingLayer = 0, short sortingOrder = 0)
    {
        return new RenderCommand (material, null, mesh, null, textures, sortingLayer, sortingOrder);
    }

    public static RenderCommand CreateMultipleTexture (Material material, Mesh mesh, short sortingLayer = 0, short sortingOrder = 0)
    {
        return new RenderCommand (material, null, mesh, null, null, sortingLayer, sortingOrder);
    }
}
