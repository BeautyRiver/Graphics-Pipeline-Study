Shader "BaseMaterial/CellShader.Shader"
{
   Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // 기본 색상
        _MainTex ("Texture", 2D) = "white" {} // 사용할 텍스처 지정
        _DivideLevel("Divide Level", Int) = 5 // 계단식 조명 효과를 위한 분할 레벨
        _Brightness("Brightness", Float) = 1.0 // 최종 색상의 밝기 조절
        _AmbientColor("AmbientColor", Color) = (1,1,1,1) // 환경 광 색상
        _SpecularColor("Specular Color", Color) = (1,1,1,1) // 스펙큘러 색상
        _Shininess("Shininess", Float) = 20.0 // 반사 하이라이트의 선명도
    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"} // 렌더링 유형 및 광원 처리 모드 지정
            LOD 200 // 상세 수준 설정

            CGPROGRAM
            #pragma vertex vert // 정점 셰이더 함수 지정
            #pragma fragment frag // 프래그먼트 셰이더 함수 지정
            #pragma target 3.0 // 셰이더 모델 목표 지정
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight // 라이트맵 옵션 설정
            #include "UnityCG.cginc" // Unity 셰이더 공통 헤더 포함
            #include "Lighting.cginc" // Unity 조명 관련 헤더 포함
            #include "AutoLight.cginc" // 자동 광원 처리 관련 헤더 포함

            sampler2D _MainTex; // 메인 텍스처
            int _DivideLevel; // 계단식 조명 효과 분할 레벨
            fixed4 _Color; // 기본 색상
            float _Brightness; // 밝기 조절
            float4 _AmbientColor; // 환경 광 색상
            float4 _SpecularColor; // 스펙큘러 색상
            float _Shininess; // 반사 광의 선명도

            struct v2f
            {
                float4 pos : SV_POSITION; // 클립 좌표 위치
                float2 uv : TEXCOORD0; // 텍스처 좌표
                float3 worldNormal : NORMAL; // 월드 좌표계의 법선
                float3 worldPos : TEXCOORD1; // 월드 좌표계의 위치
                SHADOW_COORDS(2) // 그림자 처리를 위한 좌표
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // 오브젝트에서 클립 좌표로 변환
                o.uv = v.texcoord; // 텍스처 좌표 전달
                o.worldNormal = UnityObjectToWorldNormal(v.normal); // 법선을 월드 좌표계로 변환
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // 오브젝트 좌표를 월드 좌표로 변환
                TRANSFER_SHADOW(o); // 그림자 좌표 전달
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // 텍스처에서 색상 샘플링
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos); // 시야 방향 계산

                // Ambient
                half3 ambient = ShadeSH9(half4(i.worldNormal, 1)); // 환경 광 적용

                // Diffuse
                half diffuse = max(0, dot(i.worldNormal, _WorldSpaceLightPos0.xyz)); // 램버트 조명 계산

                // Specular
                half3 halfwayDir = normalize(_WorldSpaceLightPos0.xyz + viewDir); // 하프웨이 벡터 계산
                half specAngle = max(0, dot(i.worldNormal, halfwayDir)); // 스펙큘러 각도 계산
                half specular = pow(specAngle, _Shininess); // Phong 스펙큘러 계산

                // Shadow
                fixed shadow = SHADOW_ATTENUATION(i); // 그림자 감쇄 계산

                // Lighting and color calculation
                float3 lighting = diffuse * shadow * _LightColor0.rgb; // 디퓨즈 조명 계산
                lighting += ambient; // 환경 광 추가
                lighting += specular * _SpecularColor.rgb; // 스펙큘러 조명 추가

                col *= _Brightness; // 밝기 조절
                col.rgb *= _Color.rgb * lighting; // 최종 색상 계산
                col.rgb += _AmbientColor.rgb; // 환경 광 색상 추가

                return col; // 최종 색상 반환
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // 폴백 옵션
}
