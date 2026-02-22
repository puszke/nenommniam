Shader "Unlit/Noen"
{
    Properties
    {
        _Scale ("Pattern Scale", Float) = 2
        _Speed ("Animation Speed", Float) = 1
        _Intensity ("Glow Intensity", Float) = 5
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline"
               "RenderType"="Transparent"
               "Queue"="Transparent" }

        Pass
        {
            Blend One One
            ZWrite Off
            Cull Off

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
                float3 objectPos : TEXCOORD0;
            };

            float _Scale;
            float _Speed;
            float _Intensity;

            float3 HSVtoRGB(float3 c)
            {
                float4 K = float4(1., 2./3., 1./3., 3.);
                float3 p = abs(frac(c.xxx + K.xyz) * 6. - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.objectPos = IN.positionOS.xyz;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float time = _Time.y * _Speed;

                // OBJECT SPACE pattern (stały względem obiektu)
                float pattern = sin(IN.objectPos.x * _Scale + time) *
                                cos(IN.objectPos.z * _Scale + time);

                pattern = pattern * 0.5 + 0.5;

                float hue = frac(pattern + time * 0.2);

                float3 rainbow = HSVtoRGB(float3(hue, 1, 1));

                return float4(rainbow * _Intensity, 1);
            }

            ENDHLSL
        }
    }
}
