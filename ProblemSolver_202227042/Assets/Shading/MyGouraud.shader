Shader "My/gouraudShader"
{
    Properties
    {
        // 주 색상
        _Color("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Pass
        {
            Tags{ "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert  // vert 함수를 정점 셰이더로 사용
            #pragma fragment frag  // frag 함수를 프래그먼트 셰이더로 사용

            #include "UnityCG.cginc"  // Unity의 공통 셰이더 함수 및 매크로 포함
            #include "Lighting.cginc"  // Unity의 조명 계산 함수 포함

            // 정점 입력 구조체
            struct appdata
            {
                float4 vertex : POSITION;  // 정점의 위치
                float3 normal : NORMAL;    // 정점의 법선 벡터
            };

            // 정점 출력 및 프래그먼트 입력 구조체
            struct v2f
            {
                float4 vertex : SV_POSITION;  // 클립 공간에서의 정점 위치
                float4 illumination : COLOR0;  // 조명 결과 (주변광 + 확산광 + 반사광)
            };

            // 정점 셰이더 함수
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);  // 정점의 위치를 클립 공간으로 변환

                // 주변광 계산
                float4 ambientReflection = 2.0 * UNITY_LIGHTMODEL_AMBIENT;

                // 확산광 계산
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);  // 정점의 법선 벡터를 월드 공간으로 변환
                float3 lightDir = normalize(_WorldSpaceLightPos0);  // 월드 공간에서의 빛 방향
                float3 diffuseReflection = 1.0 * _LightColor0 * saturate(dot(worldNormal, lightDir));  // 람베르트 법칙에 따른 확산광 계산

                // 반사광 계산
                float3 reflectedDir = reflect(-lightDir, worldNormal);  // 반사 벡터 계산
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldNormal);  // 뷰 벡터 계산
                float reflectIntensity = saturate(dot(reflectedDir, viewDir));  // 반사광 강도 계산

                float n = 4.0;
                reflectIntensity = pow(reflectIntensity, n);  // 반사광 지수 적용
                float3 specularReflection = 1.0 * _LightColor0 * reflectIntensity;  // 반사광 계산

                // 조명 결과를 하나로 합산하여 프래그먼트 셰이더로 전달
                o.illumination = float4(ambientReflection + diffuseReflection + specularReflection, 1.0);

                return o;
            }

            float4 _Color;  // 주 색상 프로퍼티

            // 프래그먼트 셰이더 함수
            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = _Color * i.illumination;  // 조명 결과와 주 색상을 곱하여 최종 색상 계산
                return color;
            }
            ENDCG
        }
    }
}
