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
 
            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 position : SV_POSITION;
                float4 vertexPosition : TEXCOORD0;
                //float4 col : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                //o.position = normalize(v.vertex);
                //o.position = UnityObjectToViewPos(v.vertex);
                o.position = UnityObjectToClipPos(v.vertex);
                o.vertexPosition = v.vertex;
                //o.position = v.vertex;
                //o.position = float4(1, 1, 1, 1);
                //o.position = UnityObjectToClipPos (v.vertex);
                //o.col = float4(1, 0, 0, 0);
                return o;
            }
 
            // Color32 values packed into a uint
            StructuredBuffer<int> _Colors;
 
            // decode the 8 bit color values from the uint back to a half4
            half4 DecodeColor(int indexX, int indexY)
            {
            //    half4 col = half4(colorData & 255, (colorData >> 8) & 255, (colorData >> 16) & 255, (colorData >> 24) & 255) / 255.0;
 
            //    // correct for gamma conversion when using linear space rendering
            //#ifndef UNITY_COLORSPACE_GAMMA
            //    col.rgb = GammaToLinearSpace(col.rgb);
            //#endif
 
            //    return col;


                // Working code
                //int index = indexY * 640 + indexX;

                //float r = ((float)((int) _Colors[index * 3])) / 255;
                //float g = ((float)((int) _Colors[index * 3 + 1])) / 255;
                //float b = ((float)((int) _Colors[index * 3 + 2])) / 255;
                
                //half4 col = half4(r, g, b, 1);

                // Performance test
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
                //half4 col = half4(indexX < 630 ? 1 : 0, indexY < 470 ? 1 : 0, 0, 1); 
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

                int panelWidth = 640;
                int padding = 10;

                int indexX = i.position.x - _ScreenParams.x + panelWidth + padding;
                int indexY = i.position.y - padding;
                //int indexX = i.position[0];
                //int indexY = i.position[1];
                //uint index = ((640 * 480) - ((indexY + 1) * 640)) + indexX;
                return DecodeColor(indexX, indexY);
            }
            ENDCG
        }
    }
}