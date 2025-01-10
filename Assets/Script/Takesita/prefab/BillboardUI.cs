using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // テキストオブジェクトを常にカメラの方向に向ける
        transform.LookAt(cameraTransform);
        // Y軸の回転だけを保持
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y-180, 0);
    }
}
