using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private FadeController m_fade;

    void Start()
    {
        m_fade = GetComponent<FadeController>();

        // シーン開始時にフェードインを行う
        StartCoroutine(FadeInAtSceneStart());
    }

    IEnumerator FadeInAtSceneStart()
    {
        // フェードイン処理
        yield return StartCoroutine(m_fade.FadeIn());
    }
}
