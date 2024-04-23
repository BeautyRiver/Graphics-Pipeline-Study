Shader "BaseMaterial/CellShader.Shader"
{
   Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _DivideLevel("Divide Level", Int) = 5
        _Brightness("Brightness", Float) = 1.0
        _AmbientColor("AmbientColor", Color) = (1,1,1,1)
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _Shininess("Shininess", Float) = 20.0
    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
            LOD 200

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex; // ������ ���� �ؽ�ó
            int _DivideLevel; // ���� ����� ���� ���� ���� ����
            fixed4 _Color; // ǥ���� �⺻ ����
            float _Brightness; // ���� ������ ��� ����
            float4 _AmbientColor; // ȯ�� ���� ����
            float4 _SpecularColor; // ����ŧ�� ���� ����
            float _Shininess; // ǥ�� ���õ�, �ݻ� ���� ��ī�ο� ����

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                TRANSFER_SHADOW(o);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

                // Ambient
                half3 ambient = ShadeSH9(half4(i.worldNormal, 1));

                // Diffuse
                half diffuse = max(0, dot(i.worldNormal, _WorldSpaceLightPos0.xyz));

                // Specular
                half3 halfwayDir = normalize(_WorldSpaceLightPos0.xyz + viewDir);
                half specAngle = max(0, dot(i.worldNormal, halfwayDir));
                half specular = pow(specAngle, _Shininess);

                // Shadow
                fixed shadow = SHADOW_ATTENUATION(i);

                // Lighting and color calculation
                float3 lighting = diffuse * shadow * _LightColor0.rgb;
                lighting += ambient;
                lighting += specular * _SpecularColor.rgb;

                col *= _Brightness; // Apply brightness
                col.rgb *= _Color.rgb * lighting;
                col.rgb += _AmbientColor.rgb; // Add ambient color

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
