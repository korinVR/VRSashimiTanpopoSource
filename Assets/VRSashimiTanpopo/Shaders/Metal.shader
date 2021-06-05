Shader "FrameSynthesis/Metal"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _SpecColor("Specular Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "LightMode"="ForwardBase"
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            half4 _Color;
            half4 _SpecColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal: NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 pos : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(const appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, v2f o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(const v2f i) : SV_Target
            {
                const float3 worldViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                const float3 reflDir = reflect(-worldViewDir, i.worldNormal);

                float4 refColor = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflDir, 1);
                refColor.rgb = DecodeHDR(refColor, unity_SpecCube0_HDR);

                const float3 normal = normalize(i.worldNormal);
                const float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
                const float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

                const float3 halfDir = normalize(lightDir + viewDir);
                const float NdotH = dot(normal, halfDir);
                const float3 specularPower = pow(max(0, NdotH), 10.0);

                const float4 specular = float4(specularPower, 1.0) * _SpecColor;

                const float4 color = _Color * refColor + specular;
                
                return color;
            }
            ENDCG
        }
    }
}