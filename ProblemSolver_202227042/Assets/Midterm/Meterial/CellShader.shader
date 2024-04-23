Shader "Unlit/YellowShader"
{
    Properties
    {
        // ����ڰ� ���� ������ ���̴� �Ӽ���
        _DiffuseColor("DiffuseColor", Color) = (1,1,0,1) // �л걤 ����
        _LightDirection("LightDirection", Vector) = (1,1,1,0) // ���� ����
        _SpecularColor("SpecularColor", Color) = (1,1,1,1) // �ݻ籤 ����
        _Shininess("Shininess", Range(0.1, 100)) = 10    // �ݻ籤�� ���� ����
        _AmbientColor("AmbientColor", Color) = (1,1,1,1) // �ֺ��� ����
        _Brightness("Brightness", Float) = 1.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // ������ ���� ����

        Pass
        {
            CGPROGRAM
            // ����: ���� �� �����׸�Ʈ ���̴� �Լ�
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // Unity�� ���� �׷��� ��Ŭ��� ����

            // �Է� ����ü: �޽��� ���� ������
            struct appdata
            {
                float4 vertex : POSITION; // ���� ��ġ
                float3 normal : NORMAL; // ���� ����
            };

            // ��� ����ü: ���� ���̴����� �����׸�Ʈ ���̴��� ������ ����
            struct v2f
            {
                float4 vertex : SV_POSITION; // ��ũ�� ���������� ��ġ
                float3 normal : NORMAL; // ��ȯ�� ���� ����
                float3 viewDir : TEXCOORD0; // ī�޶󿡼� ���������� ����
            };

            // ���̴� �Ӽ�
            float4 _DiffuseColor;
            float4 _LightDirection;
            float4 _SpecularColor;
            float _Shininess;
            float4 _AmbientColor;
            float _Brightness;

            // ���� ���̴�: �޽��� �� ���� ó��
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // ������Ʈ �������� Ŭ�� �������� ��ȯ
                o.normal = v.normal; // ���� ����
                o.viewDir = normalize(_WorldSpaceCameraPos - v.vertex.xyz); // ī�޶� ���� ���
                return o;
            }

            // �����׸�Ʈ ���̴�: �ȼ��� ���� ���
            fixed4 frag (v2f i) : SV_Target
            {
                // �ֺ��� ���
                float4 ambient = _AmbientColor * 0.4f; // �ֺ��� ���� ����

                // ���� ����� ����ȭ
                float4 lightDir = normalize(_LightDirection);
                // ������ ���� �������κ��� �л걤 ���
                float lightIntensity = max(dot(i.normal, lightDir), 0);
                float4 diffuse = _DiffuseColor * lightIntensity;

                // �� ����� �ݻ籤 ���
                float3 viewDir = normalize(i.viewDir);
                float3 halfwayDir = normalize(lightDir + viewDir);
                float specularIntensity = pow(max(dot(i.normal, halfwayDir), 0.0), _Shininess);
                float4 specular = _SpecularColor.rgba * specularIntensity;

                // ���� ���� ���
                float4 color = diffuse + specular + ambient;

                // ���� ��� ȿ��
                float threshold = 0.1; // �Ӱ谪 ����
                float4 banding = floor(color / threshold); // ������ �Ӱ谪���� ������ �ܰ�ȭ
                float4 col = banding * threshold; // ��� ����
                col *= _Brightness;
                return col; // ���� ���� ��ȯ
            }
            ENDCG
        }
    }
}
