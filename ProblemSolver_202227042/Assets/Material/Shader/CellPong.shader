Shader "BaseMaterial/CellShader.Shader"
{
    Properties
    {
        _DiffuseColor("Diffuse Color", Color) = (1,1,0,1) // ��ü�� �⺻ ���� ����
        _LightDirection("Light Direction", Vector) = (1,1,1,0) // ���� ���� ���� ����
        _SpecularColor("Specular Color", Color) = (1,1,1,1) // �ݻ籤�� ���� ����
        _Shininess("Shininess", Range(0.1, 100)) = 10 // �ݻ籤�� ���� ���� (�������� ��¦���� ������)
        _AmbientColor("Ambient Color", Color)=(1,1,1,1) // �ֺ����� ���� ����
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" } // ������ Ÿ���� �������ϰ� ����

        Pass
        {
            CGPROGRAM
            #pragma vertex vert // ���ؽ� ���̴� �Լ� ����
            #pragma fragment frag // �����׸�Ʈ ���̴� �Լ� ����
           
            #include "UnityCG.cginc" // Unity�� �⺻ ���̴� �Լ��� ������ ����

            struct appdata
            {
                float4 vertex : POSITION; // �޽��� ���� ��ġ
                float3 normal : NORMAL; // �޽��� ���� ����
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // ��ũ�� ��ǥ��� ��ȯ�� ���� ��ġ
                float3 normal : NORMAL; // ���� ����
                float3 viewDir : TEXCOORD0; // ī�޶󿡼� ���������� ���� ����
            };

            // ���̴��� �� ������Ƽ�� ������ ����
            float4 _DiffuseColor;
            float4 _LightDirection;
            float4 _SpecularColor;
            float _Shininess;
            float4 _AmbientColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // ��ü�� ��ġ�� ī�޶� �信 ���� ����
                o.normal = v.normal; // ������ ���� ���͸� �״�� ���
                o.viewDir = normalize(_WorldSpaceCameraPos - v.vertex.xyz); // ī�޶󿡼� ���������� ���� ���� ���
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 ambient = _AmbientColor * 0.4f; // �ֺ��� ���� ������ ���Ͽ� ����

                float4 lightDir = normalize(_LightDirection); // ���� ���� ���͸� ����ȭ
                float lightIntensity = max(dot(i.normal, lightDir), 0); // ���� ����� ���� ������ �������� ���� ���� ���
                float4 diffuse = _DiffuseColor * lightIntensity; // Ȯ�걤 ��� (��ü ���� ���� ������ ����)

                float3 viewDir = normalize(i.viewDir); // ī�޶� ���� ���� ����ȭ
                float3 halfwayDir = normalize(lightDir + viewDir); // �������� ���� ���

                float specularIntensity = pow(max(dot(i.normal, halfwayDir), 0.0), _Shininess); // �ݻ籤 ���� ���
                float4 specular = _SpecularColor.rgba * specularIntensity; // �ݻ籤 ���� ���

                float4 color = diffuse + specular + ambient; // ���� ������ Ȯ�걤, �ݻ籤, �ֺ����� ���ؼ� ���

                float threshold = 0.1; // ������ ��� �Ӱ谪 ����
                float4 banding = floor(color / threshold); // ������ �Ӱ谪���� ������ ��� ȿ�� ����
                float4 col = banding * threshold; // ��� ����� ���� ���

                return col; // ���� ���� ��ȯ
            }
            ENDCG
        }
    }
}
