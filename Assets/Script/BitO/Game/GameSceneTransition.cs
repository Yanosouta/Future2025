using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneTransition : MonoBehaviour
{
    [SerializeField, Label("シーン遷移先の名前")]
    public string SceneName = "Title";

    public GameObject menuCanvas;   // Canvas(menu)をここに割り当てます
    private FadeController m_fade;

    void Start()
    {
        m_fade = GetComponent<FadeController>();
    }

    public void HideMenuAndStartFade()
    {
        Time.timeScale = 1.0f; //開始

        menuCanvas.SetActive(false);
        StartCoroutine(m_fade.FadeOutAndChangeScene(SceneName));
    }
}
