using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Graphics;

public struct VertexSdfInstance : IVertexType
{
    public Vector3 Position;

    public Vector4 RotationScaleThickness;

    public Vector4 ShapeData0;

    public Vector4 ShapeData1;

    public Color Color;

    public static readonly VertexDeclaration VertexDeclaration = new (
        new VertexElement (0, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0),
        new VertexElement (12, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
        new VertexElement (28, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2),
        new VertexElement (44, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 3),
        new VertexElement (60, VertexElementFormat.Color, VertexElementUsage.Color, 0)
        );

    readonly VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
}
