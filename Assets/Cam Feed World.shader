// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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

            struct v2f {
                float4 position : SV_POSITION;
                float4 vertexPosition : TEXCOORD0;
            };

            v2f vert (float4 vertex : POSITION)
            {
                v2f o;
                o.position = UnityObjectToClipPos(vertex);
                o.vertexPosition = mul(unity_ObjectToWorld, vertex);
                return o;
            }
 
            StructuredBuffer<int> _Colors;
 
            half4 DecodeColor(int indexX, int indexY)
            {
                // Working code
                //int index = indexY * 640 + indexX;

                //float r = ((float)((int) _Colors[index * 3])) / 255;
                //float g = ((float)((int) _Colors[index * 3 + 1])) / 255;
                //float b = ((float)((int) _Colors[index * 3 + 2])) / 255;
                
                //half4 col = half4(r, g, b, 1);

                // Potentially performance enhanced working code
                int index = indexY * 640 + indexX;
                float oneOver255 = 0.003921568627451;

                float r = _Colors[index * 3] * oneOver255;
                float g = _Colors[index * 3 + 1] * oneOver255;
                float b = _Colors[index * 3 + 2] * oneOver255;
                
                half4 col = half4(r, g, b, 1);

                // Test

                //float ting = (float)_Colors[0] / 255;
                //half4 col = half4(ting, ting, ting, 1);

                // WTF, TEXTURE IS 200 HEIGHT
                //half4 col = half4(indexX < 0 ? 1 : 0, indexY < 240 ? 1 : 0, 0, 1); 
                //half4 col = half4(1, 0, 0, 1);

                // correct for gamma conversion when using linear space rendering
            #ifndef UNITY_COLORSPACE_GAMMA
                col.rgb = GammaToLinearSpace(col.rgb);
            #endif
 
                return col;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                //uint num, stride;
                //_Colors.GetDimensions(num, stride);
 
                //uint indexA = uint(_Time.y) % num;
                //half4 colorA = DecodeColor(_Colors[indexA]);
 
                //uint indexB = (indexA + 1) % num;
                //half4 colorB = DecodeColor(_Colors[indexB]);
 
                //half t = frac(_Time.y);
 
                //return lerp(colorA, colorB, t);
                //return fixed4(i.vertexPosition[0] / 640, 0, 0, 1);
                //return fixed4(0, i.vertexPosition[1] / 480, 0, 1);
                //return fixed4(i.vertexPosition[0] / 640, i.vertexPosition[1] / 480, 0, 1);
                //return float4(0.5, 0, 0, 0);
                //return i.col;

                //float2 normalizedUV = i.vertexPosition.xy / _ScreenParams.xy;

                // _ScreenParams.x is the size of x dimension of viewport

                //int panelWidth = 640;
                //int padding = 10;

                int indexX = i.vertexPosition.x + 320;
                int indexY = i.vertexPosition.y;
                //int indexX = i.position.x - _ScreenParams.x + panelWidth + padding;
                //int indexY = i.position.y - padding;
                //int indexX = i.position[0];
                //int indexY = i.position[1];
                //uint index = ((640 * 480) - ((indexY + 1) * 640)) + indexX;
                return DecodeColor(indexX, indexY);
            }
            ENDCG
        }
    }
}