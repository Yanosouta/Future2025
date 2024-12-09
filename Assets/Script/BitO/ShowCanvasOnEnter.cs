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
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (canvas != null)
        {
            // 現在のアクティブ状態を反転させる
            bool isActive = canvas.activeSelf;
            canvas.SetActive(!isActive);

            // ゲーム内の時間を停止または再開
            if (isActive) // 表示状態だったら
            {
                Time.timeScale = 1.0f; // 時間を再開
            }
            else // 非表示だったら
            {
                Time.timeScale = 0.0f; // 時間を停止
            }
        }
    }

}
