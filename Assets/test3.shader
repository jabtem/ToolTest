Shader "Custom/TerrainHeightColorUnlit"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1) // Red
        _Color2 ("Color 2", Color) = (0, 1, 0, 1) // Green
        _Color3 ("Color 3", Color) = (0, 0, 1, 1) // Blue
        _Color4 ("Color 4", Color) = (1, 1, 0, 1) // Yellow
        _HeightMap ("Height Map", 2D) = "white" {} // 지형 높이 맵
        _HeightScale ("Height Scale", Float) = 100.0 // 높이 스케일
        _HeightOffset ("Height Offset", Float) = 0.0 // 높이 오프셋
        _Threshold1 ("Threshold 1", Range(0,1)) = 0.25 // 1단계 높이 임계값
        _Threshold2 ("Threshold 2", Range(0,1)) = 0.5 // 2단계 높이 임계값
        _Threshold3 ("Threshold 3", Range(0,1)) = 0.75 // 3단계 높이 임계값
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
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

            sampler2D _HeightMap;
            float _HeightScale;
            float _HeightOffset;
            float _Threshold1;
            float _Threshold2;
            float _Threshold3;
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
                // Sample height map
                float height = tex2D(_HeightMap, i.worldPosY).r;
                height = height * _HeightScale + _HeightOffset;

                // Normalize height
                float normalizedHeight = (i.worldPosY - height) / (_HeightScale * 2);

                // Interpolate color based on height
                fixed4 finalColor;
                if (normalizedHeight < _Threshold1)
                    finalColor = _Color1;
                else if (normalizedHeight < _Threshold2)
                    finalColor = _Color2;
                else if (normalizedHeight < _Threshold3)
                    finalColor = _Color3;
                else
                    finalColor = _Color4;

                return finalColor;
            }
            ENDCG
        }
    }
}
