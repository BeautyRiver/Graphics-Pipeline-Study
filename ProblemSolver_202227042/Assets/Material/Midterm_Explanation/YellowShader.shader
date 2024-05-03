Shader "Unlit/YellowShader"
{
    Properties
    {
        _DiffuseColor("DiffuseColor", Color) = (1,1,1,1) // �⺻ ����
        _LightDirection("LightDirection", Vector) = (1,-1,-1,0) // ���� ����
        _Brightness("Brightness", Float) = 1.0 // ��� ���� ����
        _AmbientColor("Ambient Color", Color) = (1,1,1,1) // �ֺ��� ����
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
                float4 vertex : POSITION; // ���� ��ġ
                float3 normal : NORMAL; // ������ ���� ����
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // ��ũ�� ��ǥ ��ġ
                float3 normal : NORMAL; // ��ȯ�� ���� ����
            };

            float4 _DiffuseColor; // ǥ���� ����
            float4 _LightDirection; // ������ ����
            float _Brightness; // ��� ����
            float4 _AmbientColor; // �ֺ��� ����

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // ������Ʈ ��ǥ�� Ŭ�� ��ǥ�� ��ȯ
                o.normal = v.normal; // ���� ������ ����
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_LightDirection.xyz); // ���� ���� ����ȭ
                float lightIntensity = max(dot(i.normal, lightDir), 0); // ������ ���� ������ ���� ���
                float3 ambientLight = _AmbientColor.rgb * _AmbientColor.a; // �ֺ��� ���� ���, ���Ĵ� ���� ����

                float4 col = _DiffuseColor * (_Brightness * lightIntensity + ambientLight); // ���� ���� ���

                return col; // ���� ��ȯ
            }
            ENDCG
        }
    }
}
