using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PostEffect/LightShaft", order = 1)]
public class LightShaft : PostEffectBase
{
    public enum SunShaftsResolution
    {
        Low = 0,
        Normal = 1,
        High = 2,
    }

    public enum ShaftsScreenBlendMode
    {
        Screen = 0,
        Add = 1,
    }

    [Header("解像度")]
    public SunShaftsResolution resolution = SunShaftsResolution.Normal;

    [Header("ブレンドモード")]
    public ShaftsScreenBlendMode screenBlendMode = ShaftsScreenBlendMode.Screen;

    [Header("太陽の距離")]
    [Range(50, 1000)]
    public float distance = 100f;

    [Header("放射状ぼかしの繰り返し回数")]
    [Range(1, 4)]
    public int radialBlurIterations = 2;

    [Header("太陽の原色")]
    public Color retentionColor = Color.white;

    private Color sunColor = Color.white;

    [Header("太陽の閾値")]
    public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

    [Header("夕日の色")]
    public Color TargetColor = Color.white;

    [Header("減算する速度")]
    [Range(0, 2)]
    public float SubtractionSpeed = 0.1f;

    [Header("ぼかしの半径")]
    [Range(1, 10)]
    public float sunShaftBlurRadius = 2.5f;

    [Header("放射状の強度")]
    [Range(1, 5)]
    public float sunShaftIntensity = 1.15f;

    [Header("最大半径")]
    [Range(0, 5)]
    public float maxRadius = 0.75f;

    [Header("深度テクスチャを使用")]
    public bool useDepthTexture = true;

    GameObject MainCamera;
    GameObject DirectionalLight;
    Transform sun;
    Camera cam;

    public void OnEnable()
    {
        //カメラ取得
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (MainCamera != null) cam = MainCamera.GetComponent<Camera>();

        DirectionalLight = GameObject.FindGameObjectWithTag("DirectionalLight");

        if (DirectionalLight != null) sun = DirectionalLight.GetComponent<Transform>();

        //色を保持
        sunColor = retentionColor;
    }

    public override void OnCreate()
    {
        //マテリアルを生成
        material = new Material(Resources.Load<Shader>("Shaders/LightShaft"));
    }

    public override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // カメラの存在を確認
        if (cam == null)
        {
            // nullの場合、再取得
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            if (MainCamera != null) cam = MainCamera.GetComponent<Camera>();
        }

        //太陽の存在を確認
        if(sun == null)
        {
            //nullの場合、再取得
            DirectionalLight = GameObject.FindGameObjectWithTag("DirectionalLight");

            if (DirectionalLight != null) sun = DirectionalLight.GetComponent<Transform>();
        }

        //マテリアルの存在
        if (material == null)
        {
            Debug.LogError("LightShaftのMaterialがないよお");
            Graphics.Blit(source, destination);
            return;
        }

        // 解像度に基づくスケーリング
        int divider = (resolution == SunShaftsResolution.High) ? 1 : (resolution == SunShaftsResolution.Normal) ? 2 : 4;
        int rtW = source.width / divider;
        int rtH = source.height / divider;

        // 深度テクスチャを有効化
        if (useDepthTexture)
        {
            cam.depthTextureMode |= DepthTextureMode.Depth;
        }

        // 太陽の位置をビューポート座標で取得
        Transform sunTransform = GameObject.FindGameObjectWithTag("DirectionalLight").transform;
        Vector3 NegativeNormal = -sunTransform.forward;//Z軸のマイナス法線を取得
        sunTransform.position = NegativeNormal.normalized * distance; //マイナス法線*距離の座標を取得
        sunTransform.position += sunTransform.position;
        Vector3 sunViewportPos = sunTransform ? cam.WorldToViewportPoint(sunTransform.position) : new Vector3(0.5f, 0.5f, 0.0f);
        //ebug.Log(sunViewportPos);

        //太陽の色を考える
        if (sun.eulerAngles.y > 90f && sun.eulerAngles.y < 200f)
        {
            if (sunColor.r != TargetColor.r) sunColor.r -= SubtractionSpeed;
            if (sunColor.g != TargetColor.g) sunColor.g -= SubtractionSpeed;
            if (sunColor.b != TargetColor.b) sunColor.b -= SubtractionSpeed;

            //sunColor = Color.Lerp(sunColor, TargetColor, SubtractionSpeed * Time.deltaTime);
        }
        else sunColor = retentionColor;

        //Debug.Log(sunColor);

        // 深度バッファ
        var lrDepthBuffer = RenderTexture.GetTemporary(rtW, rtH, 0);
        material.SetVector("_SunPosition", new Vector4(sunViewportPos.x, sunViewportPos.y, sunViewportPos.z, maxRadius));
        material.SetVector("_SunThreshold", sunThreshold);

        // 深度テクスチャを使用するか
        if (!useDepthTexture)
        {
            var format = cam.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
            var tmpBuffer = RenderTexture.GetTemporary(source.width, source.height, 0, format);
            RenderTexture.active = tmpBuffer;
            GL.ClearWithSkybox(false, cam);

            material.SetTexture("_Skybox", tmpBuffer);
            Graphics.Blit(source, lrDepthBuffer, material, 3);
            RenderTexture.ReleaseTemporary(tmpBuffer);
        }
        else
        {
            Graphics.Blit(source, lrDepthBuffer, material, 2);
        }

        // 放射状のブレを適用
        float ofs = sunShaftBlurRadius * (1.0f / 768.0f);
        material.SetVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));

        var lrColorB = RenderTexture.GetTemporary(rtW, rtH, 0);

        for (int it = 0; it < radialBlurIterations; it++)
        {
            Graphics.Blit(lrDepthBuffer, lrColorB, material, 1);
            ofs = sunShaftBlurRadius * (((it * 2.0f + 1.0f) * 6.0f)) / 768.0f;
            material.SetVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));

            RenderTexture.ReleaseTemporary(lrDepthBuffer);
            lrDepthBuffer = RenderTexture.GetTemporary(rtW, rtH, 0);

            Graphics.Blit(lrColorB, lrDepthBuffer, material, 1);
            ofs = sunShaftBlurRadius * (((it * 2.0f + 2.0f) * 6.0f)) / 768.0f;
            material.SetVector("_BlurRadius4", new Vector4(ofs, ofs, 0.0f, 0.0f));
        }

        // 放射状の最終合成
        material.SetVector("_SunColor", new Vector4(sunColor.r, sunColor.g, sunColor.b, sunColor.a) * sunShaftIntensity);
        material.SetTexture("_ColorBuffer", lrDepthBuffer);

        int blendMode = (screenBlendMode == ShaftsScreenBlendMode.Screen) ? 0 : 4;
        Graphics.Blit(source, destination, material, blendMode);

        RenderTexture.ReleaseTemporary(lrDepthBuffer);
        RenderTexture.ReleaseTemporary(lrColorB);
    }
}
