using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseSelect : MonoBehaviour
{
    public Button button1; // Button1をアタッチ
    public Button button2; // Button2をアタッチ
    private Canvas m_canvas;

    private Button currentButton;

    void Start()
    {
        // 最初のボタンにフォーカスを当てる
        currentButton = button1;
        button1.Select();

        button1.onClick.AddListener(OnButton1Click);
        button2.onClick.AddListener(OnButton2Click);
    }

    void Update()
    {
        // 左右の矢印キーの入力をチェック
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentButton == button1)
            {
                currentButton = button2;
                button2.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentButton == button2)
            {
                currentButton = button1;
                button1.Select();
            }
        }      
    }

    void OnButton1Click()
    {
        if(m_canvas != null)
        {
            m_canvas.gameObject.SetActive(false);
        }
    }

    // button2をクリックしたときの処理 (ゲーム終了)
    void OnButton2Click()
    {
        // ゲームを終了
        Application.Quit();

        // Unityエディタ上で実行している場合は停止させる
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
