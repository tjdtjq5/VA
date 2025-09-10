// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Makeway/Rainbow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RainbowSpeed ("Rainbow Speed", Range(0, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _RainbowSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float rainbow = sin(_Time.y * _RainbowSpeed + i.uv.x * 10);
                float3 color = float3(0.5 + 0.5 * sin(rainbow), 0.5 + 0.5 * sin(rainbow + 2), 0.5 + 0.5 * sin(rainbow + 4));
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return fixed4(texColor.rgb * color, texColor.a); // ✅ 투명도 유지
            }
            ENDCG
        }
    }
}
