using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvasOnEnter : MonoBehaviour
{
    public GameObject canvas; // 非表示にしているCanvas

    // コントローラーもの
    ControllerState m_State;

    private bool isPaused = false;

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
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (!isPaused)
        {
            isPaused = true;

            // ポーズ状態にする
            canvas.SetActive(true); // ポーズ画面を表示
            Time.timeScale = 0.0f; // ゲーム時間を停止
        }
        else
        {
           isPaused = false;

            // ポーズ解除
            canvas.SetActive(false); // ポーズ画面を非表示
            Time.timeScale = 1.0f; // ゲーム時間を再開
        }
    }

}
