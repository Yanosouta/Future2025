using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneTransition : MonoBehaviour
{
    [SerializeField, Label("シーン遷移先の名前")]
    public string SceneName = "Game";

    private FadeController m_fade;

    void Start()
    {
        m_fade = GetComponent<FadeController>();
    }

    public void HideMenuAndStartFade()
    {
        StartCoroutine(m_fade.FadeOutAndChangeScene(SceneName));
    }
}
