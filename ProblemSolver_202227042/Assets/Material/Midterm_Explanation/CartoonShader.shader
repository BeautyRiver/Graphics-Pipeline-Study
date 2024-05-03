Shader "BaseMaterial/CellShader.Shader"
{
   Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // �⺻ ����
        _MainTex ("Texture", 2D) = "white" {} // ����� �ؽ�ó ����
        _DivideLevel("Divide Level", Int) = 5 // ��ܽ� ���� ȿ���� ���� ���� ����
        _Brightness("Brightness", Float) = 1.0 // ���� ������ ��� ����
        _AmbientColor("AmbientColor", Color) = (1,1,1,1) // ȯ�� �� ����
        _SpecularColor("Specular Color", Color) = (1,1,1,1) // ����ŧ�� ����
        _Shininess("Shininess", Float) = 20.0 // �ݻ� ���̶���Ʈ�� ����
    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"} // ������ ���� �� ���� ó�� ��� ����
            LOD 200 // �� ���� ����

            CGPROGRAM
            #pragma vertex vert // ���� ���̴� �Լ� ����
            #pragma fragment frag // �����׸�Ʈ ���̴� �Լ� ����
            #pragma target 3.0 // ���̴� �� ��ǥ ����
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight // ����Ʈ�� �ɼ� ����
            #include "UnityCG.cginc" // Unity ���̴� ���� ��� ����
            #include "Lighting.cginc" // Unity ���� ���� ��� ����
            #include "AutoLight.cginc" // �ڵ� ���� ó�� ���� ��� ����

            sampler2D _MainTex; // ���� �ؽ�ó
            int _DivideLevel; // ��ܽ� ���� ȿ�� ���� ����
            fixed4 _Color; // �⺻ ����
            float _Brightness; // ��� ����
            float4 _AmbientColor; // ȯ�� �� ����
            float4 _SpecularColor; // ����ŧ�� ����
            float _Shininess; // �ݻ� ���� ����

            struct v2f
            {
                float4 pos : SV_POSITION; // Ŭ�� ��ǥ ��ġ
                float2 uv : TEXCOORD0; // �ؽ�ó ��ǥ
                float3 worldNormal : NORMAL; // ���� ��ǥ���� ����
                float3 worldPos : TEXCOORD1; // ���� ��ǥ���� ��ġ
                SHADOW_COORDS(2) // �׸��� ó���� ���� ��ǥ
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // ������Ʈ���� Ŭ�� ��ǥ�� ��ȯ
                o.uv = v.texcoord; // �ؽ�ó ��ǥ ����
                o.worldNormal = UnityObjectToWorldNormal(v.normal); // ������ ���� ��ǥ��� ��ȯ
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // ������Ʈ ��ǥ�� ���� ��ǥ�� ��ȯ
                TRANSFER_SHADOW(o); // �׸��� ��ǥ ����
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // �ؽ�ó���� ���� ���ø�
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos); // �þ� ���� ���

                // Ambient
                half3 ambient = ShadeSH9(half4(i.worldNormal, 1)); // ȯ�� �� ����

                // Diffuse
                half diffuse = max(0, dot(i.worldNormal, _WorldSpaceLightPos0.xyz)); // ����Ʈ ���� ���

                // Specular
                half3 halfwayDir = normalize(_WorldSpaceLightPos0.xyz + viewDir); // �������� ���� ���
                half specAngle = max(0, dot(i.worldNormal, halfwayDir)); // ����ŧ�� ���� ���
                half specular = pow(specAngle, _Shininess); // Phong ����ŧ�� ���

                // Shadow
                fixed shadow = SHADOW_ATTENUATION(i); // �׸��� ���� ���

                // Lighting and color calculation
                float3 lighting = diffuse * shadow * _LightColor0.rgb; // ��ǻ�� ���� ���
                lighting += ambient; // ȯ�� �� �߰�
                lighting += specular * _SpecularColor.rgb; // ����ŧ�� ���� �߰�

                col *= _Brightness; // ��� ����
                col.rgb *= _Color.rgb * lighting; // ���� ���� ���
                col.rgb += _AmbientColor.rgb; // ȯ�� �� ���� �߰�

                return col; // ���� ���� ��ȯ
            }
            ENDCG
        }
    }
    FallBack "Diffuse" // ���� �ɼ�
}
