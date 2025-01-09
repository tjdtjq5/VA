Shader "Makeway/AdditiveAndTiling"
{
	Properties
	{
		[PerRendererData] _MainTex("BaseMap",2D) = "white"{}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_UVFO("TilingOfsset", Vector) = (1,1,0,0)
        _TimeSpeedX("TimeSpeedX", Range(0, 100)) = 10
        _TimeSpeedY("TimeSpeedY", Range(0, 100)) = 10
		
	    [PerRendererData]	_StencilComp ("Stencil Comparison", Float) = 8
	    [PerRendererData]	_Stencil ("Stencil ID", Float) = 0
	    [PerRendererData]	_StencilOp ("Stencil Operation", Float) = 0
	    [PerRendererData]	_StencilWriteMask ("Stencil Write Mask", Float) = 255
	    [PerRendererData]	_StencilReadMask ("Stencil Read Mask", Float) = 255

	    [PerRendererData]	_ColorMask ("Color Mask", Float) = 15
	    [PerRendererData]	_ClipRect ("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)
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

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Fog { Mode Off }
		Blend One One

		ColorMask [_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color	: COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color	: COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _ClipRect;
			half _TimeSpeedX;
            half _TimeSpeedY;
			float4 _UVFO;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
#endif
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = tex2D(_MainTex, IN.texcoord * float2(_UVFO.x, _UVFO.y) + float2(_UVFO.z + (_Time.x * _TimeSpeedX), _UVFO.w + (_Time.x * _TimeSpeedY))) * IN.color;
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.01);
#endif
				color.rgb *= color.a;
				return color;
			}
		ENDCG
		}
	}
}
