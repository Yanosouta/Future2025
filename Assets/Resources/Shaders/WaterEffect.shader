Shader "Hidden/WaterEffect"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _Strength("Strength", Float) = 0.01
        _WaterColour("Water Colour", Color) = (1.0, 1.0, 1.0, 1.0)
        _FogStrength("Fog Strength", Float) = 0.1
    }


    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            uniform sampler2D _MainTex;

            uniform sampler2D _BumpMap;
            uniform float _Strength;

            uniform float4 _WaterColor;
            uniform float _FogStrength;
            uniform sampler2D _CameraDepthTexture;

            fixed4 frag(v2f i) : SV_Target
            {
                //法線の再構成
                half3 normal = UnpackNormal(tex2D(_BumpMap, (i.uv + _Time.x) % 1.0));

                //uvを揺らす
                float2 uv = i.uv + normal * _Strength;

                fixed4 col = tex2D(_MainTex, uv);

                float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv));
                //深度を線形補間
                depth = Linear01Depth(depth);

                //色を線形補間
                col = lerp(col, _WaterColor, depth * _FogStrength);

                return col;
            }
            ENDCG
        }
    }
}
