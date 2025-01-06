// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Makeway/Tiling"
{
    Properties
    {
        [PerRendererData] _MainTex("BaseMap",2D) = "white"{}
        _Color ("Tint", Color) = (1,1,1,1)
        _UVFO("TilingOfsset", Vector) = (1,1,0,0)
        _TimeSpeedX("TimeSpeedX", Range(0, 100)) = 10
        _TimeSpeedY("TimeSpeedY", Range(0, 100)) = 10
        
         _StencilComp ("Stencil Comparison", Float) = 8
         _Stencil ("Stencil ID", Float) = 0
         _StencilOp ("Stencil Operation", Float) = 0
         _StencilWriteMask ("Stencil Write Mask", Float) = 255
         _StencilReadMask ("Stencil Read Mask", Float) = 255
         _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
    	Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
    	
    	Stencil
         {
             Ref [_Stencil]
             Comp [_StencilComp]
             Pass [_StencilOp]
             ReadMask [_StencilReadMask]
             WriteMask [_StencilWriteMask]
         }
         ColorMask [_ColorMask]

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			#include "UnityUI.cginc"

            struct Attributes
            {
                float4 vertex       : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 vertex       : SV_POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

        	sampler2D _MainTex;
            half _TimeSpeedX;
            half _TimeSpeedY;
            fixed4 _Color;

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _UVFO;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;
                // OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
             	half4 color = tex2D(_MainTex, IN.uv * float2(_UVFO.x, _UVFO.y) + float2(_UVFO.z + (_Time.x * _TimeSpeedX), _UVFO.w + (_Time.x * _TimeSpeedY))) * IN.color;
                return color;
            }
            ENDCG
        }
    }
}
