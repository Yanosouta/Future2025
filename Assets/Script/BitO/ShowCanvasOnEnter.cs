using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowCanvasOnEnter : MonoBehaviour
{
    public GameObject canvas; // 非表示にしているCanvas
    public GameObject[] panels; // 4つのパネルを格納する配列
    public GameObject firstButton; // 最初にフォーカスを当てたいボタン


    public GameObject m_camera;
    // コントローラーもの
    ControllerState m_State;
    ControllerBase m_Controller;
    ControllerBase.ControllerButton m_Button;

    private FadeController fadeController;

    bool isActive = false;
    bool OnOFFFlg = false;
    private void Start()
    {
        m_State = m_camera.GetComponent<ControllerState>();
        m_Controller = m_camera.GetComponent<ControllerBase>();

        fadeController = FindObjectOfType<FadeController>();
    }

    void Update()
    {
        if (fadeController != null && fadeController.isFading) // フェード中は処理を無効化
        {
            return;
        }

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
        if (canvas != null && panels.Length == 4) // パネルが4つ登録されている場合
        {
            // 現在のアクティブ状態を反転させる
            isActive = canvas.activeSelf;
            canvas.SetActive(!isActive);

            if (isActive) // 表示状態だったら
            {
                Time.timeScale = 1.0f; // 時間を再開
            }
            else // 非表示だったら
            {
                Time.timeScale = 0.0f; // 時間を停止

                // パネルの制御
                panels[0].SetActive(true); // 1つ目のパネルを表示
                panels[1].SetActive(false); // 2つ目を非表示
                panels[2].SetActive(false); // 3つ目を非表示
                panels[3].SetActive(false); // 4つ目を非表示

                // フォーカスを最初のボタンに設定
                SetButtonFocus(firstButton);
            }
        }
    }

    private void SetButtonFocus(GameObject button)
    {
        if (button != null)
        {
            // 現在の選択を解除して新しいボタンを選択
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
}
