using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ButtonNavigation : MonoBehaviour
{
    public Button button1;
    public Button button2;

    private Button currentButton;
    private FadeController m_fade;

    void Start()
    {
        m_fade = GetComponent<FadeController>();

        // 最初のボタンにフォーカスを当てる
        currentButton = button1;
        button1.Select();

        // ボタンにクリックイベントを追加
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
        // フェードアウトしてからシーン遷移
        StartCoroutine(FadeAndLoadScene());
    }

    IEnumerator FadeAndLoadScene()
    {
        // フェードアウトを待つ
        yield return StartCoroutine(m_fade.FadeOut());

        // "Game"シーンに遷移
        SceneManager.LoadScene("Game");
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
