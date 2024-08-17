Shader "Custom/Red Alpha Gradient FX" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _AlphaMap ("Gradient Transparency Map (R)", 2D) = "white" {}
        _GlowColor ("Glow Color", Color ) = ( 1.0, 1.0, 1.0, 1.0 )
        _ScrollXSpeed("X Scroll Speed", Float) = 2
        _ScrollYSpeed("Y Scroll Speed", Float) = 2
    }
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     
        ZWrite Off
        Blend SrcAlpha One
    
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
    
            sampler2D _MainTex;
            sampler2D _AlphaMap;
            float4 _MainTex_ST;
            float4 _AlphaMap_ST;
    
            fixed4 _Color;
            fixed4 _GlowColor;
            float _ScrollXSpeed;
            float _ScrollYSpeed;
    
            struct v2f {
                float4 pos : SV_POSITION;
                float4 texcoord : TEXCOORD0;
            };
    
            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.texcoord.zw = TRANSFORM_TEX(v.texcoord, _AlphaMap) + frac(float2(_ScrollXSpeed, _ScrollYSpeed) * _Time.xx);
                return o;
            }
    
            fixed4 frag (v2f i) : SV_Target
            {
                fixed main_mask = tex2D(_MainTex, i.texcoord.xy).a * _Color.a;
                fixed alpha_mask = tex2D(_AlphaMap, i.texcoord.zw).r;
    
                return fixed4(_GlowColor.rgb, main_mask * alpha_mask);
            }
            ENDCG
        }
    }
    }