#include "Sdf.fxh"

// p: local position
// s: half size
float sdfRectangle (float2 p, float2 s)
{
    float2 d = abs (p) - s;
    return abs (length (max (d, 0.0)) + min (max (d.x, d.y), 0.0));
}

float4 MainPS (PSInput i) : SV_Target
{
    float distance = sdfRectangle (i.LocalPos, i.ShapeData0.xy);
    float thickness = i.Rotation_Scale_Thickness.w / 2.0;
    float w = fwidth (distance);
    float alpha = 1.0 - smoothstep (thickness - w, thickness + w, distance);
    return float4 (i.Color.rgb * alpha, i.Color.a * alpha);
}

technique SdfLine
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS ();
        PixelShader = compile PS_SHADERMODEL MainPS ();
    }
}