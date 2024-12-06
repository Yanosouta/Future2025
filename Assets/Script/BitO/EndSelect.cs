using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSelect : MonoBehaviour
{
    [SerializeField, Label("終了しない")]
    public Button m_ONButton;
    [SerializeField, Label("終了する")]
    public Button m_OFFButton;

    private Button currentButton;

    // 表示・非表示にするパネルの参照
    [SerializeField, Label("メインのパネル")]
    public GameObject currentPanel;
    [SerializeField, Label("終了用のパネル")]
    public GameObject targetPanel;

    void Start()
    {
        // 最初のボタンにフォーカスを当てる
        currentButton = m_ONButton;
        m_ONButton.Select();

        // ボタンにクリックイベントを追加
        m_ONButton.onClick.AddListener(OnButtonClick);
        m_OFFButton.onClick.AddListener(OFFButtonClick);
    }

    void Update()
    {
        // 左右の矢印キーの入力をチェック
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentButton == m_ONButton)
            {
                currentButton = m_OFFButton;
                m_OFFButton.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentButton == m_OFFButton)
            {
                currentButton = m_ONButton;
                m_ONButton.Select();
            }
        }
    }

    void OnButtonClick()
    {
        // ゲームを終了
        Application.Quit();

        // Unityエディタ上で実行している場合は停止させる
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif    
    }


    public void OFFButtonClick()
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

}
