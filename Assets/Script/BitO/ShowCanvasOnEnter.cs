using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvasOnEnter : MonoBehaviour
{
    public GameObject canvas; // 非表示にしているCanvas

    public GameObject m_camera;
    // コントローラーもの
    ControllerState m_State;
    ControllerBase m_Controller;
    ControllerBase.ControllerButton m_Button;

    bool isActive = false;
    bool OnOFFFlg = false;
    private void Start()
    {
        m_State = m_camera.GetComponent<ControllerState>();
        m_Controller = m_camera.GetComponent<ControllerBase>();
        // コントローラー
        //m_State = GetComponent<ControllerState>();
    }

    void Update()
    {
        m_Button = m_Controller.GetButton();
        // Enterキーが押された場合またはコントローラーのボタンが押された場合
        if (m_State.GetButtonMenu())
        {
            OnOFFFlg = true;
        }
        ONToOFF();
    }
    private void ONToOFF()
    {
        if(OnOFFFlg)
        {
            if (m_State.GetButtonDoNot())
            {
                TogglePause();
                OnOFFFlg = false;
            }
        }
    }

    private void TogglePause()
    {
        if (canvas != null)
        {
            
            // 現在のアクティブ状態を反転させる
            isActive = canvas.activeSelf;
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
