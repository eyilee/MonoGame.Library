using Microsoft.Xna.Framework;
using System;

namespace Eycraft.Framework.Graphics;

public class SdfInstanceBatchEncoder : IBatchEncoder<VertexSdfInstance>
{
    public void Encode (VertexSdfInstance[] batchVertices, int index, Mesh mesh)
    {
        ReadOnlySpan<Vector4> uvs = mesh.UVs;
        ReadOnlySpan<Vector4> uv1s = mesh.UV1s;
        ReadOnlySpan<Vector4> uv2s = mesh.UV2s;
        ReadOnlySpan<Color> colors = mesh.Colors;

        ref VertexSdfInstance vertex = ref batchVertices[index];

        vertex.Position = new Vector3 (uvs[0].X, uvs[0].Y, 0f);
        vertex.RotationScaleThickness = uv1s[0];
        vertex.ShapeData0 = uv2s[0];
        vertex.Color = colors[0];
    }
}
