Shader "Makeway/UnlitAndTilingAndMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskMap("Mask", 2D) = "white"{}
        _Color ("Tint", Color) = (1,1,1,1)
        _Value("Value", Range(0, 6)) = 1

        _UVFO ("TilingOffset", Vector) = (1,1,0,0)
        _TimeSpeedX("TimeSpeedX", Range(0, 100)) = 10
        _TimeSpeedY("TimeSpeedY", Range(0, 100)) = 10

        [PerRendererData] _StencilComp ("Stencil Comparison", Float) = 8
        [PerRendererData] _Stencil ("Stencil ID", Float) = 0
        [PerRendererData] _StencilOp ("Stencil Operation", Float) = 0
        [PerRendererData] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [PerRendererData] _StencilReadMask ("Stencil Read Mask", Float) = 255

        [PerRendererData] _ColorMask ("Color Mask", Float) = 15
        [PerRendererData] _ClipRect ("Clip Rect", Vector) = (-32767, -32767, 32767, 32767)
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
            sampler2D _MaskMap;
			float4 _MaskMap_ST;
            float4 _Color;
            float4 _ClipRect;
            float4 _UVFO;
            half _TimeSpeedX;
            half _TimeSpeedY;
            float _Value;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD1;
                fixed4 color : COLOR;
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
                float2 uv = i.uv * _UVFO.xy + _UVFO.zw + float2(_Time.y * _TimeSpeedX, _Time.y * _TimeSpeedY);
            
                half4 col = tex2D(_MainTex, uv) * i.color * _Value;
                half4 mask = tex2D(_MaskMap, i.uv); // 마스크는 기본 UV 기준
            
                float clipVal = UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                
                // ✅ 알파와 RGB 모두 마스크 알파를 곱함
                col.a *= mask.a * clipVal;
                col.rgb *= col.a;
            
            #ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.01);
            #endif
            
                return col;
            }
            ENDCG
        }
    }
}
