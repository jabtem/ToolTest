Shader "Custom/TerrainHeightColorLit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _HeightMap ("Height Map", 2D) = "white" {}
        _HeightScale ("Height Scale", Float) = 1.0
        _HeightOffset ("Height Offset", Float) = 0.0
        _HeightThreshold ("Height Threshold", Range(0,1)) = 0.5
        _AboveColor ("Above Color", Color) = (1,0,0,1)
        _BelowColor ("Below Color", Color) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        sampler2D _MainTex;
        sampler2D _HeightMap;
        float4 _Color;
        float _HeightScale;
        float _HeightOffset;
        float _HeightThreshold;
        float4 _AboveColor;
        float4 _BelowColor;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;

            // Sample height map
            float height = tex2D(_HeightMap, IN.uv_MainTex).r;
            height = height * _HeightScale + _HeightOffset;

            // Normalize height
            float normalizedHeight = (IN.worldPos.y - height) / (_HeightScale * 2);

            // Interpolate color based on height
            o.Emission = lerp(_BelowColor.rgb, _AboveColor.rgb, saturate((normalizedHeight - _HeightThreshold) / (1 - _HeightThreshold)));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
