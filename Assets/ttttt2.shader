Shader "Custom/TerrainHeightColorUnlit"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1) // Red
        _Color2 ("Color 2", Color) = (0, 1, 0, 1) // Green
        _Color3 ("Color 3", Color) = (0, 0, 1, 1) // Blue
        _Color4 ("Color 4", Color) = (1, 1, 0, 1) // Yellow
        _Threshold1 ("Threshold 1", Range(0,10)) = 2 // 1단계 높이 임계값
        _Threshold2 ("Threshold 2", Range(0,10)) = 5 // 2단계 높이 임계값
        _Threshold3 ("Threshold 3", Range(0,10)) = 8 // 3단계 높이 임계값
        _HeightScale ("Height Scale", Float) = 100.0 // 높이 스케일
        _HeightOffset ("Height Offset", Float) = 0.0 // 높이 오프셋
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
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

            float _Threshold1;
            float _Threshold2;
            float _Threshold3;
            float _HeightScale;
            float _HeightOffset;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosY = v.vertex.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate normalized height
                float height = (i.worldPosY - _HeightOffset) / _HeightScale;
                float normalizedHeight = saturate(height);

                // Interpolate color based on height
                fixed4 finalColor;
                if (normalizedHeight < _Threshold1 / 10.0)
                {
                    discard;
                    //finalColor = _Color1;
                }
                else if (normalizedHeight < _Threshold2 / 10.0)
                {
                    float t = (normalizedHeight - _Threshold1 / 10.0) / ((_Threshold2 - _Threshold1) / 10.0);
                    finalColor = lerp(_Color1, _Color2, t);
                }
                else if (normalizedHeight < _Threshold3 / 10.0)
                {
                    float t = (normalizedHeight - _Threshold2 / 10.0) / ((_Threshold3 - _Threshold2) / 10.0);
                    finalColor = lerp(_Color2, _Color3, t);
                }
                else
                {
                    float t = (normalizedHeight - _Threshold3 / 10.0) / ((10.0 - _Threshold3) / 10.0);
                    finalColor = lerp(_Color3, _Color4, t);
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
