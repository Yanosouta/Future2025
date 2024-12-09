using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class tesuto : MonoBehaviour
{
    [SerializeField, Label("ポーズ画面")]
    public GameObject MainPanel;
    [SerializeField, Label("ゲームに戻る")]
    public Button m_StartButton;
    [SerializeField, Label("図鑑")]
    public Button m_BookButton;
    [SerializeField, Label("操作方法")]
    public Button m_OperationButton;
    [SerializeField, Label("終了")]
    public Button m_EndButton;

    [SerializeField, Label("終了しない")]
    public Button m_OFFButton;
    [SerializeField, Label("終了する")]
    public Button m_ONButton;

    private Button currentButton;
    ShowCanvasOnEnter CanvasOnEnter;

    // コントローラーもの
    ControllerState m_State;


    private void Start()
    {
        // コントローラー
        m_State = GetComponent<ControllerState>();

        // 最初のボタンにフォーカスを当てる
        currentButton = m_StartButton;
        m_StartButton.Select();
        m_StartButton.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        // 左右の矢印キーの入力をチェック
        if (m_State.GetButtonDown())
        {
            if (currentButton == m_StartButton)
            {
                currentButton = m_BookButton;
                m_BookButton.Select();
            }
        }
        else if (m_State.GetButtonUp())
        {
            if (currentButton == m_BookButton)
            {
                currentButton = m_StartButton;
                m_StartButton.Select();
            }
        }
    }
    public void OnButtonClick()
    {
        // MainPanelを非表示
        Time.timeScale = 1.0f; // ゲーム時間を再開
        MainPanel.SetActive(false);
    }
}
