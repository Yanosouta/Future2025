Shader "Custom/WaterFragmentShader"
{
    Properties
    {
        [HideInInspector]
        _WaveInputTex("Wave Input Texture", 2D) = "black" {}
        _BumpAmt("BumpAmt", Range(0,10000)) = 0
       
        _WaveTex("Wave",2D) = "gray" {}
       
        _WaveDecay("WaveDecay", Float) = 0.1
        _VertexDisplacementScale("Displacement Scale", Float) = 0.5
        _ParallaxScale("Parallax Scale", Float) = 1
        _NormalScaleFactor("Normal Scale Factor", Float) = 1


        _Scale("Scale", Float) = 50
        _TimeScale("TimeScale", Vector) = (0,1,0,0)
        _Distortion("Distortion", Float) = 1.0
        _Spec("Specular", Color) = (0.5, 0.5, 0.5, 1)
        _Smooth("Smoothness", Range(0, 1)) = 1
        _Alpha("Alpha", Range(0,1)) = 0.5
        _Complex("Complex", Float) = 5
        _Effectivity("Effectivity", Float) = 0.5
    }

    Category
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        CGINCLUDE
        #include "UnityCG.cginc"
        #include "Assets/WaveformProvider/Shader/Lib/WaveUtil.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float4 pos : SV_POSITION;
            float4 screenPos : TEXCOORD1;
            float2 uv : TEXCOORD2;
            float3 worldPos : TEXCOORD3;
        };

        sampler2D _WaveInputTex;
        float4 _WaveInputTex_ST;
        float _BumpAmt;
        float _WaveDecay;
        float _VertexDisplacementScale;
        float _ParallaxScale;
        float _NormalScaleFactor;

        float _Scale;
        float4 _TimeScale;
        float _Distortion;
        fixed4 _Spec;
        float _Smooth;
        float _Complex;
        float _Effectivity;
        float _Alpha;

        //wave texture definition.
        WAVE_TEX_DEFINE(_WaveTex)

        // ランダム関数
        fixed2 random2(fixed2 st)
        {
            st = fixed2(dot(st, fixed2(127.1, 311.7)),
                dot(st, fixed2(269.5, 183.3)));
            return -1.0 + 2.0 * frac(sin(st) * 43758.5453123);
        }

        // 疑似的な法線マップの作成
        float perlin(fixed2 st)
        {
            fixed2 p = floor(st);
            fixed2 f = frac(st);
            fixed2 u = f * f * (3.0 - 2.0 * f);

            float v00 = dot(random2(p + fixed2(0, 0)), fixed2(1, 1));
            float v10 = dot(random2(p + fixed2(1, 0)), fixed2(1, 1));
            float v01 = dot(random2(p + fixed2(0, 1)), fixed2(1, 1));
            float v11 = dot(random2(p + fixed2(1, 1)), fixed2(1, 1));

            return lerp(lerp(v00, v10, u.x),
                lerp(v01, v11, u.x),
                u.y) + 0.5f;
        }

        // フラクタルノイズ関数
        float fBm(fixed2 st)
        {
            float f = 0;
            fixed2 q = st;
            for (int i = 1; i <= _Complex; i++)
            {
                f += pow(_Effectivity, i) * perlin(q);
                q /= _Effectivity;
            }
            return f;
        }


        ENDCG
    }

    SubShader
    {
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        // GrabPass を使用してスクリーンのテクスチャを取得
        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _GrabTexture : register(s0);

            // 頂点シェーダー
            v2f vert(appdata v)
            {
                v2f o;
                // 波の高さを計算して頂点を変位
                float waveHeight = fBm(v.uv * _Scale + _TimeScale.xy * _Time.y);
                float wave = WAVE_HEIGHT(_WaveTex, v.uv) * _VertexDisplacementScale;
                float3 displacedPos = v.vertex.xyz;
                displacedPos.y += (waveHeight + wave);

                //座標変換
                o.pos = UnityObjectToClipPos(float4(displacedPos, 1.0));

                //スクリーン位置
                o.screenPos = ComputeScreenPos(o.pos);

                //UV変換
                o.uv = v.uv;

                //ワールド座標変換
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            // フラグメントシェーダー
            fixed4 frag(v2f i) : SV_Target
            {
                float2 bump = WAVE_NORMAL_ADJ(_WaveTex, i.uv, _ParallaxScale, _NormalScaleFactor) * _WaveDecay;

                float2 offset = bump * _BumpAmt;

                // UV スケーリングと時間によるアニメーション
                float2 uv = i.uv;
                uv *= _Scale;
                uv += _TimeScale.xy * _Time.y;

                // フラクタルノイズによる波の高さ
                float kiten = fBm(uv);

                // 法線の計算
                float gradX = fBm(uv + float2(1.0, 0.0));
                float gradY = fBm(uv + float2(0.0, 1.0));
                float3 vecX = float3(1.0, 0.0, gradX - kiten);
                float3 vecY = float3(0.0, 1.0, gradY - kiten);
                float3 norm = normalize(cross(vecX, vecY));

                // スクリーン空間の UV を計算して GrabPass を使用した揺れを追加
                //float2 grabUV = i.screenPos.xy / i.screenPos.w;
                float2 grabUV = offset * i.screenPos.z + i.screenPos.xy;
                grabUV = grabUV / i.screenPos.w;

                // 法線を使用してスクリーン座標を歪め、屈折効果を適用
                grabUV += norm.rg * _Distortion;

                // GrabPass からテクスチャをサンプリングし、揺れを適用
                fixed3 refractedColor = tex2D(_GrabTexture, grabUV).rgb;

                // 最終的なアルベドとスペキュラを計算
                float3 albedo = refractedColor;
                float alpha = _Alpha;

                // Specular と Smoothness を含む最終色
                fixed4 finalColor = fixed4(albedo, alpha);
                finalColor.rgb *= _Spec.rgb;
                finalColor.a = alpha;

                return finalColor;
            }
            ENDCG
        }
    }

    SubShader
    {
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        // GrabPass を使用してスクリーンのテクスチャを取得
        GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _GrabTexture : register(s0);

            v2f vert(appdata v)
            {
                v2f o;
                // 波の高さを計算して頂点を変位
                float waveHeight = fBm(v.uv * _Scale + _TimeScale.xy * _Time.y);
                float3 displacedPos = v.vertex.xyz;
                displacedPos.y += waveHeight;
                o.pos = UnityObjectToClipPos(float4(displacedPos, 1.0));
                o.screenPos = ComputeScreenPos(o.pos);

                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 bump = WAVE_NORMAL_ADJ(_WaveTex, i.uv, _ParallaxScale, _NormalScaleFactor) * _WaveDecay;

                float2 offset = bump * _BumpAmt;

                // UV スケーリングと時間によるアニメーション
                float2 uv = i.uv;
                uv *= _Scale;
                uv += _TimeScale.xy * _Time.y;

                // フラクタルノイズによる波の高さ
                float kiten = fBm(uv);

                // 法線の計算
                float gradX = fBm(uv + float2(1.0, 0.0));
                float gradY = fBm(uv + float2(0.0, 1.0));
                float3 vecX = float3(1.0, 0.0, gradX - kiten);
                float3 vecY = float3(0.0, 1.0, gradY - kiten);
                float3 norm = normalize(cross(vecX, vecY));

                // スクリーン空間の UV を計算して GrabPass を使用した揺れを追加
                //float2 grabUV = i.screenPos.xy / i.screenPos.w;
                float2 grabUV = offset * i.screenPos.z + i.screenPos.xy;
                grabUV = grabUV / i.screenPos.w;

                // 法線を使用してスクリーン座標を歪め、屈折効果を適用
                grabUV += norm.rg * _Distortion;

                // GrabPass からテクスチャをサンプリングし、揺れを適用
                fixed3 refractedColor = tex2D(_GrabTexture, grabUV).rgb;

                // 最終的なアルベドとスペキュラを計算
                float3 albedo = refractedColor;
                float alpha = _Alpha;

                // Specular と Smoothness を含む最終色
                fixed4 finalColor = fixed4(albedo, alpha);
                finalColor.rgb *= _Spec.rgb;
                finalColor.a = alpha;

                return finalColor;
            }

            ENDCG
        }
    }
}
