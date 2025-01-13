using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField, Label("フェード用のImage")]
    public Image fadeImage;

    [SerializeField, Label("フェードにかかる時間")]
    public float fadeDuration = 1.0f;

    private Button[] buttons;
    public bool isFading = false; // フェード中かどうかを示すフラグ

    private void Start()
    {
        buttons = FindObjectsOfType<Button>();

        // ゲーム開始時にフェードインを実行
        StartCoroutine(FadeIn());
    }

    // ボタンを有効化または無効化するメソッド
    private void SetButtonsInteractable(bool interactable)
    {
        foreach (var button in buttons)
        {
            button.interactable = interactable;
        }
    }

    // フェードイン処理
    public IEnumerator FadeIn()
    {
        isFading = true; // フェード開始
        SetButtonsInteractable(false); // フェード完了後にボタンを有効化

        float timer = 0;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration); // Alpha値を1から0へ徐々に変更
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color; // 完全に透明に

        SetButtonsInteractable(true); // フェード完了後にボタンを有効化
        isFading = false; // フェード終了
    }

    // フェードアウト処理とシーン遷移
    public IEnumerator FadeOutAndChangeScene(string sceneName)
    {
        isFading = true; // フェード開始
        SetButtonsInteractable(false); // フェード中はボタンを無効化

        float timer = 0;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration); // Alpha値を0から1へ徐々に変更
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color; // 完全に不透明に

        // フェードアウトが完了したらシーン遷移
        SceneManager.LoadScene(sceneName);
        isFading = false; // フェード終了
    }
}
