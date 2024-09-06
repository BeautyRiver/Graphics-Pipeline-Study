Shader "My/MyPhong"
{
    Properties
    {
        _Color("Main Color", Color) = (1.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
        Pass
        {

            Tags{ "LightMode" = "ForwardBase" }


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                float3 lightDir = normalize(_WorldSpaceLightPos0);
                o.lightDir = lightDir;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.worldNormal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 주변광
                float4 ambientReflection = 1.0 * UNITY_LIGHTMODEL_AMBIENT;

                // 확산광
                float3 worldNormal = normalize(i.worldNormal);
                float3 lightDir = i.lightDir;
                float3 diffuseReflection = 1.0 * _LightColor0.rgb * saturate(dot(worldNormal, lightDir));

                // 반사광
                float3 reflectedDir = reflect(-lightDir, worldNormal);
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldNormal);
                float reflectIntensity = saturate(dot(reflectedDir, viewDir));

                float n = 4.0;
                reflectIntensity = pow(reflectIntensity, n);
                float3 specularReflection = 1.0 * _LightColor0 * reflectIntensity;

                fixed4 col = _Color * float4(ambientReflection + diffuseReflection, 1.0) + float4(specularReflection, 1.0);
                return col;
            }
            ENDCG
        }
    }
}

