// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Makeway/PureReflective"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // 기본 텍스처
        _ReflectionColor ("Reflection Color", Color) = (1, 1, 1, 1)  // 반사 색상
        _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5  // 반사 강도
        _ShineSpeed ("Shine Speed", Range(0, 5)) = 1.5  // 빛 반짝이는 속도
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
            fixed4 _ReflectionColor;
            float _ReflectionStrength;
            float _ShineSpeed;

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
                float alpha = texColor.a; // 원래 Alpha 값 저장

                // 빛 반사 효과: X축 기준으로 반짝이는 패턴
                float reflection = sin(i.uv.x * 40 + _Time.y * _ShineSpeed) * _ReflectionStrength;
                reflection = max(reflection, 0);  // 음수 값 제거

                // 빛 반사가 텍스처의 밝은 부분에서만 적용되도록 조정
                float luminance = dot(texColor.rgb, float3(0.3, 0.59, 0.11)); // 밝기 계산
                reflection *= luminance; // 밝은 부분에만 반사 적용

                fixed4 finalColor = texColor + reflection * _ReflectionColor;
                finalColor.a = alpha;  // 원래 Alpha 값 유지

                return finalColor;
            }
            ENDCG
        }
    }
}
