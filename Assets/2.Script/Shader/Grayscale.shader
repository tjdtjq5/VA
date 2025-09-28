Shader "Makeway/GrayscaleReplace"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Target Color", Color) = (1,1,1,1)         // 비교 기준 색상
        _NewColor("Replace Color", Color) = (1,0,0,1)     // 치환할 색상
        _Distance("Threshold", Range(0,1)) = 0.2          // 색상 비교 허용 범위
        _EffectAmount("Grayscale Amount", Range(0,1)) = 1 // 흑백 전환 강도
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

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                fixed4 color    : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;
            };

            sampler2D _MainTex;
            float4 _Color;
            float4 _NewColor;
            float _Distance;
            float _EffectAmount;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;

                // 1) 색상 치환
                if (distance(c.rgb, _Color.rgb) < _Distance)
                {
                    c.rgb = _NewColor.rgb;
                }
                else
                {
                    // 2) Grayscale 변환 (효과 강도 적용)
                    float gray = dot(c.rgb, float3(0.3, 0.59, 0.11));
                    c.rgb = lerp(c.rgb, gray.xxx, _EffectAmount);
                }

                return c;
            }
            ENDCG
        }
    }
}
