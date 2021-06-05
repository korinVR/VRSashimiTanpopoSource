Shader "VRSashimiTanpopo/ConveyorIron"
{
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 reflWorld : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(const appdata_base v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, v2f o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = worldPos;
                o.reflWorld = reflect(-worldViewDir, worldNormal);
                return o;
            }
            
            #include "SashimiMachineShadow.cginc"

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample reflection probe.
                half4 color = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, i.reflWorld, 0);
                color.rgb = DecodeHDR(color, unity_SpecCube0_HDR);

                color.rgb *= getSashimiMachineAttenuation(i.worldPos);
                return color;
            }
            ENDCG
        }
    }
}