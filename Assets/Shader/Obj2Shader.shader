Shader "OurShaders/Obj2Shader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Mask("Mask", 2D) = "white" {}
        _Cantidad("Cantidad", float) = 0
        _Intensidad("Intensidad", float) = 0

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
            };

            sampler2D _MainTex;
            sampler2D _Mask;
            float _Cantidad;
            float _Intensidad;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float4 positionO = IN.positionOS;
                float4 positionW = mul(UNITY_MATRIX_M, positionO);
                
                positionW.x = lerp(positionW.x, positionW.x + (sin(positionW.y * _Cantidad * 2 * 3.1415)), _Intensidad);
                
                float4 positionC = mul(UNITY_MATRIX_V, positionW);
                float4 positionS = mul(UNITY_MATRIX_P, positionC);


                OUT.positionHCS = positionS;

                OUT.uv = (positionS.xy / positionS.w) * 0.5 + 0.5;
            #if UNITY_UV_STARTS_AT_TOP
                OUT.uv.y = 1.0 - OUT.uv.y;
            #endif
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {

                float4 texColor = tex2D(_MainTex, IN.uv);

                return texColor;
            }    
            ENDHLSL
        }
    }
}