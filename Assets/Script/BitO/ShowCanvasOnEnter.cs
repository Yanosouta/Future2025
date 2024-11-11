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
        // Enterキーが押されたときにCanvasを表示する
        if (m_State.GetButtonMenu() || Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("押されています");
            canvas.SetActive(true); // Canvasのアクティブ状態を切り替え
        }
    }
}
