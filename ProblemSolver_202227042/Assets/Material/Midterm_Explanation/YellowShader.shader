Shader "Unlit/YellowShader"
{
    Properties
    {
        _DiffuseColor("DiffuseColor", Color) = (1,1,1,1) // 기본 색상
        _LightDirection("LightDirection", Vector) = (1,-1,-1,0) // 광원 방향
        _Brightness("Brightness", Float) = 1.0 // 밝기 조절 변수
        _AmbientColor("Ambient Color", Color) = (1,1,1,1) // 주변광 색상
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION; // 정점 위치
                float3 normal : NORMAL; // 정점의 법선 벡터
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // 스크린 좌표 위치
                float3 normal : NORMAL; // 변환된 법선 벡터
            };

            float4 _DiffuseColor; // 표면의 색상
            float4 _LightDirection; // 광원의 방향
            float _Brightness; // 밝기 조절
            float4 _AmbientColor; // 주변광 색상

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 오브젝트 좌표를 클립 좌표로 변환
                o.normal = v.normal; // 법선 데이터 전달
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_LightDirection.xyz); // 광원 방향 정규화
                float lightIntensity = max(dot(i.normal, lightDir), 0); // 광원과 법선 벡터의 각도 계산
                float3 ambientLight = _AmbientColor.rgb * _AmbientColor.a; // 주변광 색상 사용, 알파는 강도 조절

                float4 col = _DiffuseColor * (_Brightness * lightIntensity + ambientLight); // 최종 색상 계산

                return col; // 색상 반환
            }
            ENDCG
        }
    }
}
