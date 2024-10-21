Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color("Outline Color", Color) = (1,0,0,1) // 아웃라인 색상 (빨간색)
        _OutlineWidth("Outline Width", Float) = 0.03 // 아웃라인 두께
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "Outline"

            // 아웃라인 렌더링을 위해 Cull Front 사용
            Cull Front
            ZWrite On
            ColorMask RGB

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float _OutlineWidth;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                // 노멀 방향으로 정점 확대
                float3 norm = mul((float3x3)unity_ObjectToWorld, v.normal);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.pos.xy += norm.xy * _OutlineWidth; // 아웃라인 두께만큼 정점을 이동
                o.color = _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color; // 아웃라인 색상 출력
            }
            ENDCG
        }
    }
}
