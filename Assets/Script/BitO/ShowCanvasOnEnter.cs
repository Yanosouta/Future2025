using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvasOnEnter : MonoBehaviour
{
    public GameObject canvas; // 非表示にしているCanvas

    // コントローラーもの
    ControllerState m_State;

    private void Start()
    {
        // コントローラー
        m_State = GetComponent<ControllerState>();
    }
    void Update()
    {
        // Enterキーが押された場合またはコントローラーのボタンが押された場合
        if (m_State.GetButtonMenu() || Input.GetKeyDown(KeyCode.A))
        {
            // Canvasのアクティブ状態を切り替える
            canvas.SetActive(!canvas.activeSelf);
        }
    }
}
