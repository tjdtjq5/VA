Shader "Makeway/Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    	
        [PerRendererData]	_StencilComp ("Stencil Comparison", Float) = 8
	    [PerRendererData]	_Stencil ("Stencil ID", Float) = 0
	    [PerRendererData]	_StencilOp ("Stencil Operation", Float) = 0
	    [PerRendererData]	_StencilWriteMask ("Stencil Write Mask", Float) = 255
	    [PerRendererData]	_StencilReadMask ("Stencil Read Mask", Float) = 255
        
        [PerRendererData]     _ColorMask ("Color Mask", Float) = 15
        [PerRendererData]     _ClipRect ("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)
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
      	
        LOD 100
        Color [_Color]
        Lighting Off
		ZWrite Off
        Blend DstColor OneMinusSrcAlpha
        ColorMask [_ColorMask]
       
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _ClipRect;
          
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color    : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD1;
                fixed4 color    : COLOR;
            };



            v2f vert (appdata v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
