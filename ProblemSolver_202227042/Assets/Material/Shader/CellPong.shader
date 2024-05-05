Shader "BaseMaterial/CellShader.Shader"
{
    Properties
    {
        _DiffuseColor("Diffuse Color", Color) = (1,1,0,1) // 물체의 기본 색상 설정
        _LightDirection("Light Direction", Vector) = (1,1,1,0) // 빛이 오는 방향 설정
        _SpecularColor("Specular Color", Color) = (1,1,1,1) // 반사광의 색상 설정
        _Shininess("Shininess", Range(0.1, 100)) = 10 // 반사광의 세기 조절 (높을수록 반짝임이 강해짐)
        _AmbientColor("Ambient Color", Color)=(1,1,1,1) // 주변광의 색상 설정
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // 렌더링 타입을 불투명하게 설정

        Pass
        {
            CGPROGRAM
            #pragma vertex vert // 버텍스 쉐이더 함수 지정
            #pragma fragment frag // 프래그먼트 쉐이더 함수 지정
           
            #include "UnityCG.cginc" // Unity의 기본 쉐이더 함수와 변수를 포함

            struct appdata
            {
                float4 vertex : POSITION; // 메쉬의 정점 위치
                float3 normal : NORMAL; // 메쉬의 법선 벡터
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // 스크린 좌표계로 변환된 정점 위치
                float3 normal : NORMAL; // 법선 벡터
                float3 viewDir : TEXCOORD0; // 카메라에서 정점까지의 방향 벡터
            };

            // 쉐이더의 각 프로퍼티를 변수로 선언
            float4 _DiffuseColor;
            float4 _LightDirection;
            float4 _SpecularColor;
            float _Shininess;
            float4 _AmbientColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 객체의 위치를 카메라 뷰에 맞춰 조정
                o.normal = v.normal; // 정점의 법선 벡터를 그대로 사용
                o.viewDir = normalize(_WorldSpaceCameraPos - v.vertex.xyz); // 카메라에서 정점까지의 방향 벡터 계산
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 ambient = _AmbientColor * 0.4f; // 주변광 색상에 강도를 곱하여 적용

                float4 lightDir = normalize(_LightDirection); // 빛의 방향 벡터를 정규화
                float lightIntensity = max(dot(i.normal, lightDir), 0); // 빛의 방향과 법선 벡터의 내적으로 빛의 강도 계산
                float4 diffuse = _DiffuseColor * lightIntensity; // 확산광 계산 (물체 색상에 빛의 강도를 곱함)

                float3 viewDir = normalize(i.viewDir); // 카메라 방향 벡터 정규화
                float3 halfwayDir = normalize(lightDir + viewDir); // 하프웨이 벡터 계산

                float specularIntensity = pow(max(dot(i.normal, halfwayDir), 0.0), _Shininess); // 반사광 강도 계산
                float4 specular = _SpecularColor.rgba * specularIntensity; // 반사광 색상 계산

                float4 color = diffuse + specular + ambient; // 최종 색상은 확산광, 반사광, 주변광을 더해서 계산

                float threshold = 0.1; // 색상의 밴딩 임계값 설정
                float4 banding = floor(color / threshold); // 색상을 임계값으로 나누어 밴딩 효과 적용
                float4 col = banding * threshold; // 밴딩 적용된 색상 계산

                return col; // 최종 색상 반환
            }
            ENDCG
        }
    }
}
