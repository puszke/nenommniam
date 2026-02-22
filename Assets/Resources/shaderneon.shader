Shader "Unlit/shaderneon"
{
    Properties
    {
        _ColorA ("Color A", Color) = (0,1,1,1)
        _ColorB ("Color B", Color) = (1,0,1,1)
        _ColorC ("Color C", Color) = (0,0.5,1,1)

        _Intensity ("Neon Intensity", Range(0,10)) = 3
        _Speed ("Animation Speed", Float) = 1
        _Scale ("Gradient Scale", Float) = 3
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalRenderPipeline"
            "LightMode"="UniversalForward"
        }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 screenUV : TEXCOORD0;
            };

            float4 _ColorA;
            float4 _ColorB;
            float4 _ColorC;

            float _Intensity;
            float _Speed;
            float _Scale;

            Varyings vert (Attributes v)
            {
                Varyings o;

                o.positionHCS = TransformObjectToHClip(v.positionOS);

                // SCREEN SPACE UV
                float4 clipPos = o.positionHCS;
                o.screenUV = clipPos.xy / clipPos.w;
                o.screenUV = o.screenUV * 0.5 + 0.5;

                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float2 uv = i.screenUV;

                // Centered coordinates
                float2 p = (uv - 0.5) * _Scale;

                float time = _Time.y * _Speed;

                // Neon waves
                float wave =
                    sin(p.x * 3 + time) +
                    cos(p.y * 4 - time) +
                    sin(length(p) * 6 - time * 2);

                wave *= 0.33;

                float3 col =
                    lerp(_ColorA.rgb, _ColorB.rgb, sin(wave) * 0.5 + 0.5);

                col = lerp(col, _ColorC.rgb,
                    smoothstep(-0.2, 0.8, wave));

                // Neon glow boost
                col *= _Intensity;

                return float4(col,1);
            }

            ENDHLSL
        }
    }
}
