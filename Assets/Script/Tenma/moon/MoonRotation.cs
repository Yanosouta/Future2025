using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonRotation : MonoBehaviour
{
    public Light directionalLight;  // ディレクションライト（太陽）
    public Transform center;         // 月が回転する中心（地球）
    public float orbitDistance = 10f; // 月の軌道半径（地球からの距離）

    void Update()
    {
        if (directionalLight != null && center != null)
        {
            // ディレクションライトの回転方向を取得
            Vector3 lightDirection = directionalLight.transform.forward;

            // ライトの向きの逆側に月を配置する位置を計算
            Vector3 moonPosition = center.position + lightDirection.normalized * orbitDistance;

            // 月の位置を更新
            transform.position = moonPosition;

            // 常に月が地球を向くように回転を更新
            transform.LookAt(center);
        }
    }
}

