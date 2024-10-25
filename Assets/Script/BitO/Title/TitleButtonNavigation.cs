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
    [SerializeField, Label("続きから")]
    public Button m_ContinuationButton;
    [SerializeField, Label("図鑑")]
    public Button m_BookButton;
    [SerializeField, Label("終了")]
    public Button m_EndButton;

    private Button currentButton;

    void Start()
    {
        // 最初のボタンにフォーカスを当てる
        currentButton = m_StartButton;
        m_StartButton.Select();

        // ボタンにクリックイベントを追加
        m_ContinuationButton.onClick.AddListener(OnContinuationButtonClick);
        m_BookButton.onClick.AddListener(OnBookButtonClick);
        m_EndButton.onClick.AddListener(OnEndButtonClick);
    }

    void Update()
    {
        // 左右の矢印キーの入力をチェック
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentButton == m_StartButton)
            {
                currentButton = m_ContinuationButton;
                m_ContinuationButton.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentButton == m_ContinuationButton)
            {
                currentButton = m_StartButton;
                m_StartButton.Select();
            }
        }
    }

    // 続きからボタンの処理
    void OnContinuationButtonClick()
    {
        // 続きからの処理をここに！！

    }

    // 図鑑ボタンの処理
    void OnBookButtonClick()
    {
        // 図鑑移動の処理をここに！！
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
