using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonNavigation : MonoBehaviour
{
    [SerializeField, Label("ポーズ画面")]
    public GameObject MainPanel;
    [SerializeField, Label("操作方法画面")]
    public GameObject Panel_Button_4;

    [SerializeField, Label("ゲームに戻る")]
    public Button m_StartButton;
    [SerializeField, Label("図鑑")]
    public Button m_BookButton;
    [SerializeField, Label("操作方法")]
    public Button m_OperationButton;
    [SerializeField, Label("終了")]
    public Button m_EndButton;

    private Button currentButton;

    private void Start()
    {
        // 最初のボタンにフォーカスを当てる
        currentButton = m_StartButton;
        m_StartButton.Select();
        m_EndButton.onClick.AddListener(OnEndButtonClick);

    }

    void Update()
    {
        // 左右の矢印キーの入力をチェック
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentButton == m_StartButton)
            {
                currentButton = m_BookButton;
                m_BookButton.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentButton == m_BookButton)
            {
                currentButton = m_StartButton;
                m_StartButton.Select();
            }
        }
    }

    // ポーズ画面を閉じる
    public void MenuClose()
    {
        // MainPanelを非表示
        MainPanel.SetActive(false);
    }

    // 操作画面を開く/閉じる
    public void OperationSelect()
    {
        // MainPanelを非表示
        MainPanel.SetActive(false);

        // Panel_Button_3を表示
        Panel_Button_4.SetActive(true);
    }

    // 終了ボタン処理
    void OnEndButtonClick()
    {
        // ゲームを終了
        Application.Quit();

        // Unityエディタ上で実行している場合は停止させる
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
