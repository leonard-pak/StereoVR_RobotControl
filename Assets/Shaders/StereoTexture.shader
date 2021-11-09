Shader "Test/StereoTexture"
{
    Properties
    {
        _MainTexLeft ("Left", 2D) = "white" {}
        _MainTexRight ("Right", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTexLeft;
            sampler2D _MainTexRight;
            half4 _MainTexLeft_ST;
            half4 _MainTexRight_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

           fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                return lerp(tex2D(_MainTexLeft, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTexLeft_ST)),
                            tex2D(_MainTexRight, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTexRight_ST)),
                            unity_StereoEyeIndex);
            }
            ENDCG
        }
    }
}
