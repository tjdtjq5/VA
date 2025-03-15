// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Makeway/Test"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ElectricColor ("Electric Color", Color) = (0.5, 0.8, 1, 1)
        _ElectricIntensity ("Electric Intensity", Range(0, 2)) = 1
        _Speed ("Speed", Range(0, 10)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
            fixed4 _ElectricColor;
            float _ElectricIntensity;
            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                float alpha = texColor.a; // 원래 투명도 유지

                // 전기 애니메이션 효과
                float electricWave = sin(i.uv.x * 50 + _Time.y * _Speed) * 0.5 + 0.5;
                float glow = smoothstep(0.4, 1.0, electricWave) * _ElectricIntensity;

                // 최종 색상 적용
                fixed4 finalColor = texColor + glow * _ElectricColor;
                finalColor.a = alpha; // 투명도 유지

                return finalColor;
            }
            ENDCG
        }
    }
}
