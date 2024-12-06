Shader "Custom/RainEffect"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DripRatio("滴る割合", Range(0,1)) = 0.5
        _MinSize("最小雨粒サイズ", Float) = 0.05
        _MaxSize("最大雨粒サイズ", Float) = 0.15
        _RaindropCount("雨粒の数", Int) = 50
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
            }

            Cull Off
            ZWrite Off
            Fog { Mode Off }
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _DripRatio;
                float _MinSize;
                float _MaxSize;
                int _RaindropCount;

                // 乱数生成関数（引数なし）
                float rand(float seed)
                {
                    return frac(sin(seed) * 43758.5453);
                }

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float4 color = tex2D(_MainTex, i.uv);

                    // 雨粒のループ
                    for (int j = 0; j < _RaindropCount; j++)
                    {
                        // 各雨粒のランダム位置とサイズ（時間を加えて動的に変化）
                        float seed = float(j);
                        float2 raindropPos = float2(rand(seed), rand(seed * 1.3));

                        // 滴る雨粒は徐々にy軸方向に移動
                        if (rand(seed * 3.1) < _DripRatio)
                        {
                            raindropPos.y -= frac(_Time.y * 0.1); // 徐々にy軸方向へ
                            raindropPos.y = fmod(raindropPos.y + 1.0, 1.0); // 画面端に達したら再び上から現れる
                        }

                        float size = lerp(_MinSize, _MaxSize, rand(seed * 2.1));
                        float dist = distance(i.uv, raindropPos);

                        if (dist < size)
                        {
                            // 屈折効果を調整してUVオフセットを明確に
                            float refractionStrength = 0.05; // 屈折の強さを調整
                            float2 refractedUV = i.uv + normalize(i.uv - raindropPos) * refractionStrength * (size - dist);

                            // 雨粒のエリアで屈折効果を適用
                            color.rgb = tex2D(_MainTex, refractedUV).rgb;
                        }
                    }
                    return color;
                }
                ENDCG
            }
        }
}