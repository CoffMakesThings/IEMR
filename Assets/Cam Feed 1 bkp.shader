Shader "Unlit/StructuredBufferColor32"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #pragma target 5.0
 
            #include "UnityCG.cginc"
 
            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }
 
            // Color32 values packed into a uint
            StructuredBuffer<uint> _Colors;
 
            // decode the 8 bit color values from the uint back to a half4
            half4 DecodeColor(uint colorData)
            {
                half4 col = half4(colorData & 255, (colorData >> 8) & 255, (colorData >> 16) & 255, (colorData >> 24) & 255) / 255.0;
 
                // correct for gamma conversion when using linear space rendering
            #ifndef UNITY_COLORSPACE_GAMMA
                col.rgb = GammaToLinearSpace(col.rgb);
            #endif
 
                return col;
            }
 
            half4 frag () : SV_Target
            {
                uint num, stride;
                _Colors.GetDimensions(num, stride);
 
                uint indexA = uint(_Time.y) % num;
                half4 colorA = DecodeColor(_Colors[indexA]);
 
                uint indexB = (indexA + 1) % num;
                half4 colorB = DecodeColor(_Colors[indexB]);
 
                half t = frac(_Time.y);
 
                return lerp(colorA, colorB, t);
            }
            ENDCG
        }
    }
}