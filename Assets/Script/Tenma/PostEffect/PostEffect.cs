using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PostEffect : MonoBehaviour
{
    [SerializeField]
    protected PostEffectBase postEffect;

    public void Awake()
    {
        postEffect.OnCreate();
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        postEffect.OnRenderImage(source, destination);
    }
}
