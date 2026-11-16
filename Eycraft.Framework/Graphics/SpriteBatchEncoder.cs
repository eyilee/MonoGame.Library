using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Eycraft.Framework.Graphics;

public class SpriteBatchEncoder : IBatchEncoder<VertexPositionColorTexture>
{
    private const int VertexCount = 4;

    public void Encode (VertexPositionColorTexture[] batchVertices, int index, Mesh mesh)
    {
        ReadOnlySpan<Vector3> vertices = mesh.Vertices;
        ReadOnlySpan<Color> colors = mesh.Colors;
        ReadOnlySpan<Vector4> uvs = mesh.UVs;

        for (int i = 0; i < VertexCount; i++)
        {
            ref VertexPositionColorTexture vertex = ref batchVertices[index + i];

            vertex.Position = vertices[i];
            vertex.Color = colors[i];
            vertex.TextureCoordinate = new Vector2 (uvs[i].X, uvs[i].Y);
        }
    }
}
