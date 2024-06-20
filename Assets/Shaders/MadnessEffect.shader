Shader "Unlit/GrabPass"
{
    Properties
    {
        _IntensidadBlur("Intensidad Blur", Range(0,1)) = 0
        _MaximaSeparacionBlur("Separacion Blur", Float) = 0
        _GridSizeBlur("GrizSize Blur", Range(1,5)) = 0
        
        _TexturaVinyeta("Textura Vinyeta", 2D) = "white"{}
        _IntensidadVinyeta("Intensidad Vinyeta", Range(0,1)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "LightMode" = "UseColorTexture"
        }

                
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VShader
            #pragma fragment FShader
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct VSInput
            {
                float4 position : POSITION;
            };

            struct VSOutput
            {
                float4 position : SV_POSITION;
                float2 grabUV : TEXCOORD0;
            };

            sampler2D _GrabbedTexture;
            sampler2D _TexturaVinyeta;
            float _IntensidadVinyeta;

            VSOutput VShader(VSInput i)
            {
                VSOutput o = {float4 (0,0,0,0), float2(0,0)};

                float4 positionO = i.position;
                float4 positionW = mul(UNITY_MATRIX_M, positionO);
                float4 positionC = mul(UNITY_MATRIX_V, positionW);
                float4 positionS = mul(UNITY_MATRIX_P, positionC);

                o.position = positionS;

                o.grabUV = (positionS.xy / positionS.w) * 0.5 + 0.5;
            #if UNITY_UV_STARTS_AT_TOP
                o.grabUV.y = 1.0 - o.grabUV.y;
            #endif

                return o;
            }

            float4 FShader(VSOutput i) : SV_Target
            {
                float4 backgroundColor = tex2D(_GrabbedTexture, i.grabUV);
                float4 vinyetaColor = tex2D(_TexturaVinyeta, i.grabUV);
                float4 mixedTexture = backgroundColor * vinyetaColor;

                float4 processedColor = lerp(backgroundColor, mixedTexture, _IntensidadVinyeta);

                return processedColor;
            }

            ENDHLSL
        }

        /*Pass
        {
            HLSLPROGRAM
            #pragma vertex VShader
            #pragma fragment FShader
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct VSInput
            {
                float4 position : POSITION;
            };

            struct VSOutput
            {
                float4 position : SV_POSITION;
                float2 grabUV : TEXCOORD0;
            };

            sampler2D _GrabbedTexture;
            float _IntensidadBlur;
            float _MaximaSeparacionBlur;
            uint _GridSizeBlur;

            VSOutput VShader(VSInput i)
            {
                VSOutput o = {float4 (0,0,0,0), float2(0,0)};

                float4 positionO = i.position;
                float4 positionW = mul(UNITY_MATRIX_M, positionO);
                float4 positionC = mul(UNITY_MATRIX_V, positionW);
                float4 positionS = mul(UNITY_MATRIX_P, positionC);

                o.position = positionS;

                o.grabUV = (positionS.xy / positionS.w) * 0.5 + 0.5;
            #if UNITY_UV_STARTS_AT_TOP
                o.grabUV.y = 1.0 - o.grabUV.y;
            #endif

                return o;
            }

            float4 FShader(VSOutput i) : SV_Target
            {
                float4 backgroundColor = tex2D(_GrabbedTexture, i.grabUV);
                float4 processedColor;

                _IntensidadBlur = clamp(_IntensidadBlur, 0, 1);
                _GridSizeBlur = clamp(_GridSizeBlur, 1, 5);
                
                if (_IntensidadBlur != 0 && _GridSizeBlur != 1)
                {
                    float startPosX = 0;
                    float startPosY = 0;

                    if (5 % 2 == 0)
                    {
                        startPosX = (_MaximaSeparacionBlur * (_GridSizeBlur / 2)) + (_MaximaSeparacionBlur / 2) * _IntensidadBlur / 1000;
                    }
                    else
                    {
                        startPosX = (_MaximaSeparacionBlur * (_GridSizeBlur / 2)) * _IntensidadBlur / 1000;
                    }


                    if (5 % 2 == 0)
                    {
                        startPosY = (_MaximaSeparacionBlur * (_GridSizeBlur / 2)) + (_MaximaSeparacionBlur / 2) * _IntensidadBlur / 1000;
                    }
                    else
                    {
                        startPosY = (_MaximaSeparacionBlur * (_GridSizeBlur / 2)) * _IntensidadBlur / 1000;
                    }

                    float posX = startPosX;
                    float posY = startPosY;
                    bool firstTime = true;

                    for (uint k = 0; k < _GridSizeBlur; k++)
                    {
                        for (uint j = 0; j < _GridSizeBlur; j++)
                        {
                            float2 pos = float2(posX, posY);

                            if (firstTime)
                            {
                                processedColor = tex2D(_GrabbedTexture, i.grabUV + pos);
                                firstTime = false;
                            }
                            else
                                processedColor += tex2D(_GrabbedTexture, i.grabUV + pos);

                            posX = startPosX - (_MaximaSeparacionBlur * _IntensidadBlur / 1000 * (j + 1));
                        }
                        posY -= startPosY - (_MaximaSeparacionBlur * _IntensidadBlur / 1000 * (k + 1));
                    }
                    processedColor = processedColor / (_GridSizeBlur * _GridSizeBlur);

                    
                }
                else
                {
                    processedColor = backgroundColor;
                }

                return processedColor;
            }


                
            ENDHLSL
        }*/
        
        
    }
}
