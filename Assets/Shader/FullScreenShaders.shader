Shader "OurShaders/FullScreenShaders"
{
    Properties
    {
        _BlurSize ("Blur Size", Range(1,10)) = 10
        _MinSeparation("MinSeparation", float) = 0
        _MaxSeparation("MaxSeparation", float) = 1
        _LifePercent("LifePercent", Range(0,1)) = 1
        _LifePercentToStart("LifePercentToStart", Range(0,1)) = 0.5
        _Vinyeta("Vinyeta", 2D) = "white"{}
        _ColorVinyeta("VinyetaColor",Color) = (0,0,0,1)
	    _MagnitudVinyeta("MagnitudVinyeta", Range(0,1)) = 0.5

	    _TipoDaltonismo("TipoDaltonismo", int) = 0

        _FadeColor("FadeColor",Color) = (0,0,0,1)
        _FadeValue("FadeValue", Range(0,1)) = 0 

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

            sampler2D _GrabbedTexture;
            float _BlurSize;
            float _MinSeparation;
            float _MaxSeparation;

            sampler2D _Vinyeta;
            float4 _ColorVinyeta;
	        float _MagnitudVinyeta;

            float _LifePercent;
            float _LifePercentToStart;

            int _TipoDaltonismo;

            float4 _FadeColor;
            float _FadeValue;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float4 positionO = IN.positionOS;
                float4 positionW = mul(UNITY_MATRIX_M, positionO);
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
                float4 texColor = tex2D(_GrabbedTexture, IN.uv);
                #include "Assets/Shader/PocaVida.hlsl"
                #include "Assets/Shader/Daltonismo.hlsl"
                #include "Assets/Shader/Fade.hlsl"

                return texColor;
            }    
            ENDHLSL
        }
    }
}