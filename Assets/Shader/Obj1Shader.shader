Shader "OurShaders/Obj1Shader"
{
    Properties
    {
        _DistanceToSee("Limite", float) = 0
        _Color("ColorRadar", Color) = (0,0,0,0)

    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {
            Tags
            {
                "LightMode" = "UseColorTexture"
            }

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float distanceToCam : TEXCOORD1;
            };

            float _DistanceToSee;
            float4 _Color;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float4 positionO = IN.positionOS;
                float4 positionW = mul(UNITY_MATRIX_M, positionO);
                float4 positionC = mul(UNITY_MATRIX_V, positionW);
                float4 positionS = mul(UNITY_MATRIX_P, positionC);

                OUT.distanceToCam = distance(positionC, float4(0, 0, 0, 0));

                OUT.positionHCS = positionS;

                OUT.uv = (positionS.xy / positionS.w) * 0.5 + 0.5;
            #if UNITY_UV_STARTS_AT_TOP
                OUT.uv.y = 1.0 - OUT.uv.y;
            #endif
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                if (IN.distanceToCam < _DistanceToSee)
                {
                    return _Color;
                }
                else
                {
                    return float4(0, 0, 0, 0);
                }
            }    
            ENDHLSL
        }
    }
}