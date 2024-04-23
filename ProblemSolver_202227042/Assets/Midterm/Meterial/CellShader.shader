Shader "Unlit/YellowShader"
{
    Properties
    {
        // 사용자가 조절 가능한 쉐이더 속성들
        _DiffuseColor("DiffuseColor", Color) = (1,1,0,1) // 분산광 색상
        _LightDirection("LightDirection", Vector) = (1,1,1,0) // 빛의 방향
        _SpecularColor("SpecularColor", Color) = (1,1,1,1) // 반사광 색상
        _Shininess("Shininess", Range(0.1, 100)) = 10    // 반사광의 광택 강도
        _AmbientColor("AmbientColor", Color) = (1,1,1,1) // 주변광 색상
        _Brightness("Brightness", Float) = 1.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // 렌더링 유형 설정

        Pass
        {
            CGPROGRAM
            // 선언: 정점 및 프래그먼트 쉐이더 함수
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // Unity의 공통 그래픽 인클루드 파일

            // 입력 구조체: 메시의 정점 데이터
            struct appdata
            {
                float4 vertex : POSITION; // 정점 위치
                float3 normal : NORMAL; // 정점 법선
            };

            // 출력 구조체: 정점 쉐이더에서 프래그먼트 쉐이더로 데이터 전달
            struct v2f
            {
                float4 vertex : SV_POSITION; // 스크린 공간에서의 위치
                float3 normal : NORMAL; // 변환된 정점 법선
                float3 viewDir : TEXCOORD0; // 카메라에서 정점까지의 방향
            };

            // 쉐이더 속성
            float4 _DiffuseColor;
            float4 _LightDirection;
            float4 _SpecularColor;
            float _Shininess;
            float4 _AmbientColor;
            float _Brightness;

            // 정점 쉐이더: 메시의 각 정점 처리
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 오브젝트 공간에서 클립 공간으로 변환
                o.normal = v.normal; // 정점 법선
                o.viewDir = normalize(_WorldSpaceCameraPos - v.vertex.xyz); // 카메라 방향 계산
                return o;
            }

            // 프래그먼트 쉐이더: 픽셀별 색상 계산
            fixed4 frag (v2f i) : SV_Target
            {
                // 주변광 계산
                float4 ambient = _AmbientColor * 0.4f; // 주변광 강도 적용

                // 빛의 방향과 정규화
                float4 lightDir = normalize(_LightDirection);
                // 법선과 빛의 방향으로부터 분산광 계산
                float lightIntensity = max(dot(i.normal, lightDir), 0);
                float4 diffuse = _DiffuseColor * lightIntensity;

                // 뷰 방향과 반사광 계산
                float3 viewDir = normalize(i.viewDir);
                float3 halfwayDir = normalize(lightDir + viewDir);
                float specularIntensity = pow(max(dot(i.normal, halfwayDir), 0.0), _Shininess);
                float4 specular = _SpecularColor.rgba * specularIntensity;

                // 최종 색상 계산
                float4 color = diffuse + specular + ambient;

                // 색상 밴딩 효과
                float threshold = 0.1; // 임계값 설정
                float4 banding = floor(color / threshold); // 색상을 임계값으로 나누어 단계화
                float4 col = banding * threshold; // 밴딩 적용
                col *= _Brightness;
                return col; // 최종 색상 반환
            }
            ENDCG
        }
    }
}
