using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameButtonNavigation : MonoBehaviour
{
    [SerializeField, Label("操作方法画面")]
    public GameObject Panel_Button_4;

    [SerializeField, Label("ゲームに戻る")]
    public Button m_StartButton;
    [SerializeField, Label("図鑑")]
    public Button m_BookButton;
    [SerializeField, Label("操作方法")]
    public Button m_OperationButton;
    [SerializeField, Label("タイトルに戻る")]
    public Button m_TitleButton;
    [SerializeField, Label("終了")]
    public Button m_EndButton;

    private List<Button> buttons; // ボタンリスト
    private int currentIndex = 0; // 現在のフォーカス位置

    // 表示・非表示にするパネルの参照
    [SerializeField, Label("メインのパネル")]
    public GameObject currentPanel;
    [SerializeField, Label("終了用のパネル")]
    public GameObject targetPanel;
    [SerializeField, Label("図鑑のパネル")]
    public GameObject BookPanel;

    // コントローラーもの
    ControllerState m_State;

    [SerializeField, Label("ポーズ画面指定")]
    public GameObject canvas;
   
    private void Start()
    {
        // コントローラー
        m_State = GetComponent<ControllerState>();

        //=============================================
        // ボタンリストを初期化
        buttons = new List<Button> { m_StartButton, m_BookButton, m_OperationButton, m_TitleButton, m_EndButton };

        // 最初のボタンにフォーカスを当てる
        currentIndex = 0;
        buttons[currentIndex].Select();   
        //=============================================

        // ボタンクリック時
        m_StartButton.onClick.AddListener(MenuClose);
        m_EndButton.onClick.AddListener(OnEnd_ONButtonClick);
        m_OperationButton.onClick.AddListener(OperationOpen);

        m_BookButton.onClick.AddListener(OpenBook);
    }

    void Update()
    {
        // 十字キーの入力を取得
        if (m_State.GetButtonUp())
        {
            MoveFocus(-1); // 上キーでフォーカスを移動
        }
        else if (m_State.GetButtonDown())
        {
            MoveFocus(1); // 下キーでフォーカスを移動
        }

    }

    // フォーカス移動ロジック
    void MoveFocus(int direction)
    {
        // 現在のインデックスを更新
        currentIndex = (currentIndex + direction + buttons.Count) % buttons.Count;

        // 新しいボタンにフォーカスを移動
        buttons[currentIndex].Select();
    }

    // ポーズ画面を閉じる
    public void MenuClose()
    {
        // currentPanelを非表示
        Time.timeScale = 1.0f; // ゲーム時間を再開

        canvas.SetActive(false);
    }

    // 操作画面を開く
    public void OperationOpen()
    {
        // currentPanelを非表示
        currentPanel.SetActive(false);

        // Panel_Button_4を表示
        Panel_Button_4.SetActive(true);   
    }

    void OnEnd_ONButtonClick()
    {
        // ゲームを終了
        Application.Quit();

        // Unityエディタ上で実行している場合は停止させる
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif    
    }

    public void OnEnd_OFFButtonClick()
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
        }

        if (currentPanel != null)
        {
            currentPanel.SetActive(true);

        }
    }
    void OpenBook()
    {
        // currentPanelを非表示
        currentPanel.SetActive(false);

        // Panel_Button_4を表示
        BookPanel.SetActive(true);

    }
}
