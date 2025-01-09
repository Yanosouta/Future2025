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

    private Button currentButton;

    // 表示・非表示にするパネルの参照
    [SerializeField, Label("メインのパネル")]
    public GameObject currentPanel;
    [SerializeField, Label("終了用のパネル")]
    public GameObject targetPanel;

    // コントローラーもの
    ControllerState m_State;

    void Start()
    {
        // コントローラー
        m_State = GetComponent<ControllerState>();

        // 最初のボタンにフォーカスを当てる
        currentButton = m_StartButton;
        m_StartButton.Select();

        // ボタンにクリックイベントを追加
        m_BookButton.onClick.AddListener(OnBookButtonClick);
        m_EndButton.onClick.AddListener(OnEndButtonClick);
        m_ONButton.onClick.AddListener(OnEnd_ONButtonClick);
        m_OFFButton.onClick.AddListener(OnEnd_OFFButtonClick);
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
        Debug.Log(m_State);
    }

    // 図鑑ボタンの処理
    void OnBookButtonClick()
    {
        // 図鑑移動の処理をここに！！
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
}
