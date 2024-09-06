Shader "Unlit/YellowShader"
{
    Properties
    {
        _DiffuseColor("Diffuse Color", Color) = (1, 1, 1, 1)
        _LightDirection("Light Direction", Vector) = (1, -1, -1, 0)
        _Brightness("Brightness", Float) = 1.0
        _AmbientColor("Ambient Color", Color) = (0.1, 0.1, 0.1, 1)
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
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            float4 _DiffuseColor;
            float4 _LightDirection;
            float _Brightness;
            float4 _AmbientColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_LightDirection.xyz);
                float lightIntensity = max(dot(i.normal, lightDir), 0);
                float4 col = _DiffuseColor * lightIntensity * _Brightness;
                col += _AmbientColor; // Add ambient light color
                return col;
            }
            ENDCG
        }
    }
}
