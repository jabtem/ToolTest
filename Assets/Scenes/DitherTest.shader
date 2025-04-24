Shader "Universal Render Pipeline/Test/DitheringTranparent"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DitherLevel("DitherLevel", Range(0, 10)) = 5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            Pass
            {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float4 screenPos : TEXCOORD1;
                    float3 worldPos : TEXCOORD3;
                };

                sampler2D _MainTex;

                CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _DitherLevel;
                CBUFFER_END

                    // 디더링 패턴 행렬
                    static const float pattern[16] = {
                         0 / 16.0,  8 / 16.0,  2 / 16.0, 10 / 16.0,
                        12 / 16.0,  4 / 16.0, 14 / 16.0,  6 / 16.0,
                         3 / 16.0, 11 / 16.0,  1 / 16.0,  9 / 16.0,
                        15 / 16.0,  7 / 16.0, 13 / 16.0,  5 / 16.0
                    };

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.vertex = TransformObjectToHClip(v.vertex);
                        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                        // 클립 공간의 좌표로 스크린 좌표를 계산
                        o.screenPos = ComputeScreenPos(o.vertex);
                        return o;
                    }

                    half4 frag(v2f i) : SV_Target
                    {
                        // w로 나눠서 스크린 스페이스에서의 위치를 구함
                        float2 viewPortPos = i.screenPos.xy / i.screenPos.w;
                        // 0~1의 위치 값에서 실제 픽셀의 위치로 변환
                        float2 screenPosInPixel = viewPortPos.xy * _ScreenParams.xy;

                        // 디더링 패턴은 4x4행렬이므로, 스크린의 픽셀 위치를 4로 나눈 나머지로 대응시킨다
                        // (단 실제 디더링 패턴 데이터는 1차원 행렬이므로 X좌표의 나머지에는 4를 곱해서 2차원 배열처럼 값을 취득한다)
                        uint index = (uint(screenPosInPixel.x) % 4) * 4 + uint(screenPosInPixel.y) % 4;
                        float ditherOut = 1.0f - pattern[index];
                        clip(_DitherLevel - ditherOut);

                        float4 color = tex2D(_MainTex, i.uv);
                        return color;
                    }
                    ENDHLSL
                }
        }
}