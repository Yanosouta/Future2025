using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feed : MonoBehaviour
{
    public bool isTargeted = false; // このエサがターゲットになっているかどうか
    public bool isEaten = false; // エサが食べられたかどうか

    // エサが地面に到達したかどうかの判定
    public bool HasLanded()
    {
        return transform.position.y <= 0.1f; // 地面に触れたと仮定
    }
}
