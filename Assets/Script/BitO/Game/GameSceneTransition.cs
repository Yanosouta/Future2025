using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneTransition : MonoBehaviour
{
    [SerializeField, Label("ƒV[ƒ“‘JˆÚæ‚Ì–¼‘O")]
    public string SceneName = "Title";

    public GameObject menuCanvas;   // Canvas(menu)‚ğ‚±‚±‚ÉŠ„‚è“–‚Ä‚Ü‚·
    private FadeController m_fade;

    void Start()
    {
        m_fade = GetComponent<FadeController>();
    }

    public void HideMenuAndStartFade()
    {
        Time.timeScale = 1.0f;
        menuCanvas.SetActive(false);
        StartCoroutine(m_fade.FadeOutAndChangeScene(SceneName));
    }
}
