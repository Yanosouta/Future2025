using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PostEffectBase : ScriptableObject
{
    protected Material material;

    public virtual void OnCreate() { }
    public abstract void OnRenderImage(RenderTexture src, RenderTexture dst);
}
