Shader "Makeway/Tiling"
{
    Properties
    {
        [PerRendererData] _MainTex("BaseMap",2D) = "white"{}
        _UVFO("TilingOfsset", Vector) = (1,1,0,0)
        _TimeSpeedX("TimeSpeedX", Range(0, 100)) = 10
        _TimeSpeedY("TimeSpeedY", Range(0, 100)) = 10
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half _TimeSpeedX;
            half _TimeSpeedY;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _UVFO;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
             	half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv * float2(_UVFO.x, _UVFO.y) + float2(_UVFO.z + (_Time.x * _TimeSpeedX), _UVFO.w + (_Time.x * _TimeSpeedY)));
                return color;
            }
            ENDHLSL
        }
    }
}
