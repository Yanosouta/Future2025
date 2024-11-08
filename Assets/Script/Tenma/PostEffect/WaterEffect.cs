using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PostEffect/WaterEffect", order = 1)]
public class WaterEffect : PostEffectBase
{
    [Header("法線マップ")]
    [SerializeField]
    private Texture2D normalMap;

    [Header("強さ")]
    [Range(0, 1)]
    public float strength = 0.01f;

    [HideInInspector]
    public float defStrength;

    [Header("色")]
    [SerializeField]
    private Color waterColor = Color.white;

    [Header("フォグの強さ")]
    [SerializeField]
    private float fogStrength = 0.1f;

    [Header("用途が水中か")]
    [SerializeField]
    private bool isUnder = true;


    public override void OnCreate()
    {
        material = new Material(Resources.Load<Shader>("Shaders/WaterEffect"));
        
        material.SetTexture("_BumpMap", normalMap);
        material.SetFloat("_Strength", strength);
        material.SetColor("_WaterColour", waterColor);
        material.SetFloat("_FogStrength", fogStrength);
        defStrength = strength;
    }

    public override void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //用途が水中でない時
        if(!isUnder)
        {   
            material.SetFloat("_Strength", strength);
        }

        Graphics.Blit(src, dst, material);
    }
}
