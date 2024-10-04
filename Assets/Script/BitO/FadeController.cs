using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField, Label("フェード用のImage")]
    public Image fadeImage;

    [SerializeField, Label("フェードにかかる時間")]
    public float fadeDuration = 1.0f;

    private void Start()
    {
        // ゲーム開始時にフェードインを実行
        StartCoroutine(FadeIn());
    }

    // フェードイン処理
    public IEnumerator FadeIn()
    {
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
    }

    // フェードアウト処理
    public IEnumerator FadeOut()
    {
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
    }
}
