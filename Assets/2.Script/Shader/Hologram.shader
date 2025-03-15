// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Makeway/Hologram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveStrength ("Wave Strength", Range(0, 1)) = 0.1
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
            float _WaveStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float wave = sin(i.uv.y * 50 + _Time.y * 5) * _WaveStrength;
                float3 colorShift = float3(0.5 + 0.5 * sin(_Time.y + wave), 0.5 + 0.5 * sin(_Time.y * 1.5 + wave), 1);
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return fixed4(texColor.rgb * colorShift, texColor.a); // ✅ Alpha 유지
            }
            ENDCG
        }
    }
}
