using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvasOnEnter : MonoBehaviour
{
    public GameObject canvas; // 非表示にしているCanvas

    void Update()
    {
        // Enterキーが押されたときにCanvasを表示する
        if (Input.GetKeyDown(KeyCode.Return))
        {
            canvas.SetActive(!canvas.activeSelf); // Canvasのアクティブ状態を切り替え
        }
    }
}
