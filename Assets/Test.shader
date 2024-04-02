Shader "Custom/TerrainHeightColorUnlit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _HeightThreshold ("Height Threshold", Range(0,1)) = 0.5
        _AboveColor ("Above Color", Color) = (1,0,0,1)
        _BelowColor ("Below Color", Color) = (0,0,1,1)
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
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float worldPosY : TEXCOORD0;
            };

            float4 _Color;
            float _HeightThreshold;
            float4 _AboveColor;
            float4 _BelowColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosY = v.vertex.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Interpolate color based on height
                fixed4 color = lerp(_BelowColor, _AboveColor, saturate((i.worldPosY - _HeightThreshold) / (1 - _HeightThreshold))) * _Color;
                return color;
            }
            ENDCG
        }
    }
}
