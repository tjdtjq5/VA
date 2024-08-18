Shader "Slitz/TestShader"
{
    Properties
    {
        _BaseMap("BaseMap",2D) = "white"{}
        _UVFO("TilingOfsset", Vector) = (1,1,0,0)
        _Brightness("Brightness", Range(-1, 1)) = 0 // 프로퍼티 밝기 변수
        [HDR] _BaseColor("Base Color", Color) = (1, 1, 1, 1) 
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _UVFO;
                half4 _BaseColor;
                half _Brightness; 
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                
                half4 result = color * _BaseColor;
                result.rgb = result.rgb * _Brightness;
                
                return result;
            }
            ENDHLSL
        }
    }
}