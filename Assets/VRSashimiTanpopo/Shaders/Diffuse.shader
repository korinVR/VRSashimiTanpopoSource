Shader "VRSashimiTanpopo/Diffuse"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            fixed4 _Color;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                LIGHTING_COORDS(3, 4)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(const appdata_base v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, v2f o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            #include "SashimiMachineShadow.cginc"

            fixed4 frag(const v2f i) : SV_Target
            {
                const float3 normal = normalize(i.worldNormal);
                const float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                const float diffuse = max(0, dot(normal, lightDir));

                const float4 tex = tex2D(_MainTex, i.texcoord);
                fixed4 color = tex * _Color * diffuse * LIGHT_ATTENUATION(i) + UNITY_LIGHTMODEL_AMBIENT;
                
                color.rgb *= getSashimiMachineAttenuation(i.worldPos);
                return color;
            }
            ENDCG
        }
    }
}
