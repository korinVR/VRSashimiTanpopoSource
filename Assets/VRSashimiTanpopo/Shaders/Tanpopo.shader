// タンポポシェーダー
Shader "VRSashimiTanpopo/Tanpopo"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
        }

        ZWrite On
        // AlphaToMaskを使用してMSAAでエッジをなめらかにしつつ前後関係を正常に描画する。
        // ref. http://tsumikiseisaku.com/blog/alpha-to-coverage/
        AlphaToMask On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(const appdata_base v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, v2f o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            #define MIPMAP_BIAS -0.7

            fixed4 frag(const v2f i) : SV_Target
            {
                // デフォルトのミップマップレベルでは若干ぼやけるのでバイアスをかける。
                // ref. https://developer.oculus.com/blog/common-rendering-mistakes-how-to-find-them-and-how-to-fix-them/
                // （OpenGL ESではTexture.mipMapBiasが動作しないため、フラグメントシェーダーで対応）
                const float4 tex = tex2Dbias(_MainTex, half4(i.texcoord.x, i.texcoord.y, 0.0, MIPMAP_BIAS));
                fixed4 color = tex * _Color;
                return color;
            }
            ENDCG
        }

        Pass
        {
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(const appdata_base v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, v2f o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            #define SHADOW_CUTOFF 0.5

            float4 frag(v2f i) : COLOR
            {
                const float4 tex = tex2D(_MainTex, i.texcoord);
                // シャドウをアルファ値でカットアウトする。
                clip(tex.a - SHADOW_CUTOFF);
                return 0;
            }
            ENDCG
        }
    }
}