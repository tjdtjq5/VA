// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Makeway/Wave"
{
    Properties
    {
        _MainTex ("Water Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1
        _WaveStrength ("Wave Strength", Range(0, 0.1)) = 0.02
        _Distortion ("Distortion", Range(0, 1)) = 0.1
        _ReflectionColor ("Reflection Color", Color) = (0.5, 0.5, 1, 1)
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
            float _WaveSpeed;
            float _WaveStrength;
            float _Distortion;
            fixed4 _ReflectionColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // 파도 애니메이션 (UV 왜곡)
                float wave = sin(_Time.y * _WaveSpeed + v.uv.y * 10.0) * _WaveStrength;
                o.uv = v.uv + wave;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // ✅ 투명한 영역(Alpha 값이 낮은 부분)에는 반사 효과를 추가하지 않음
                float alpha = texColor.a;
                float reflection = sin(i.uv.y * 50.0 + _Time.y * 3.0) * _Distortion;

                // ✅ Alpha 값을 유지하면서 반사 효과 적용
                fixed4 finalColor = texColor + reflection * _ReflectionColor * alpha;
                finalColor.a = alpha; // Alpha 값 유지

                return finalColor;
            }
            ENDCG
        }
    }
}
