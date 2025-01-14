using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class TitleButtonNavigation : MonoBehaviour
{
    [SerializeField, Label("初めから")]
    public Button m_StartButton;
    [SerializeField, Label("図鑑")]
    public Button m_BookButton;
    [SerializeField, Label("終了")]
    public Button m_EndButton;

    [SerializeField, Label("終了しない")]
    public Button m_OFFButton;
    [SerializeField, Label("終了する")]
    public Button m_ONButton;
    [SerializeField, Label("図鑑画面のボタン")]
    public Button m_OpOFFBookButton;

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

    private void OnEnable()
    {
        StartCoroutine(SelectButtonOnEnable());
    }

    private IEnumerator SelectButtonOnEnable()
    {
        yield return null; // フレーム待機
        m_StartButton.Select();
    }

    void Start()
    {
        // コントローラー
        m_State = GetComponent<ControllerState>();

        //=============================================
        // ボタンについて
        // ボタンリストを初期化
        buttons = new List<Button> { m_StartButton, m_BookButton, m_EndButton };

        // 最初のボタンにフォーカスを当てる
        currentIndex = 0;
        buttons[currentIndex].Select();

        // ボタンにクリックイベントを追加
        m_EndButton.onClick.AddListener(OnEndButtonClick);
        m_ONButton.onClick.AddListener(OnEnd_ONButtonClick);
        m_OFFButton.onClick.AddListener(OnEnd_OFFButtonClick);
        m_BookButton.onClick.AddListener(OpenBook);
        m_OpOFFBookButton.onClick.AddListener(CloseBook);
        //=============================================
    }

    void Update()
    {
        // 上キーが押された場合
        if (m_State.GetButtonUp())
        {
            MoveFocus(-1); // 上に移動
        }
        // 下キーが押された場合
        else if (m_State.GetButtonDown())
        {
            MoveFocus(1); // 下に移動
        }
        if (m_State.GetButtonMenu())
        {
            BookPanel.SetActive(false);

            // メインパネルを表示
            if (currentPanel != null)
            {
                currentPanel.SetActive(true);
            }

            // メインパネルの最初のボタンにフォーカスを移動
            if (m_StartButton != null)
            {
                m_StartButton.Select();
            }
        }
    }

    private void MoveFocus(int direction)
    {
        // インデックスを更新し、ループさせる
        currentIndex = (currentIndex + direction + buttons.Count) % buttons.Count;

        // 新しいボタンにフォーカスを移動
        buttons[currentIndex].Select();
    }

    // 終了ボタン処理
    void OnEndButtonClick()
    {

        // 現在のパネルを非表示
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }

        // ターゲットパネルを表示
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);

            // ターゲットパネル内の最初のボタンにフォーカスを移動
            Button targetButton = targetPanel.GetComponentInChildren<Button>();
            if (targetButton != null)
            {
                targetButton.Select();
            }
        }
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

            // ターゲットパネル内の最初のボタンにフォーカスを移動
            Button currentButton = currentPanel.GetComponentInChildren<Button>();
            if (currentButton != null)
            {
                currentButton.Select();
            }
        }
    }
    void OpenBook()
    {
        // currentPanelを非表示
        currentPanel.SetActive(false);

        // Panel_Button_4を表示
        BookPanel.SetActive(true);
    }
    void CloseBook()
    {
        BookPanel.SetActive(false);

        currentPanel.SetActive(true);

    }
}
