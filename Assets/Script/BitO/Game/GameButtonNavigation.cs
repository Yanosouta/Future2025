using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonNavigation : MonoBehaviour
{
    [SerializeField, Label("ポーズ画面")]
    public GameObject MainPanel;
    [SerializeField, Label("セーブ画面")]
    public GameObject Panel_Button_2;
    [SerializeField, Label("操作方法画面")]
    public GameObject Panel_Button_4;

    [SerializeField, Label("ゲームに戻る")]
    public Button m_StartButton;
    [SerializeField, Label("セーブ")]
    public Button m_SaveButton;
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
    }

    void Update()
    {
        // 左右の矢印キーの入力をチェック
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentButton == m_StartButton)
            {
                currentButton = m_SaveButton;
                m_SaveButton.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentButton == m_SaveButton)
            {
                currentButton = m_StartButton;
                m_StartButton.Select();
            }
        }
    }

    // Button_3がクリックされた時に呼び出されるメソッド
    public void SwitchPanels()
    {
        // MainPanelを非表示
        MainPanel.SetActive(false);

        // Panel_Button_3を表示
        Panel_Button_2.SetActive(true);
    }

    public void OperationSelect()
    {
        // MainPanelを非表示
        MainPanel.SetActive(false);

        // Panel_Button_3を表示
        Panel_Button_4.SetActive(true);
    }

}
