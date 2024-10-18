using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject MainPanel;      // MainPanelのGameObject
    public GameObject Panel_Button_3; // Panel_Button_3のGameObject

    // Button_3がクリックされた時に呼び出されるメソッド
    public void SwitchPanels()
    {
        // MainPanelを非表示
        MainPanel.SetActive(false);

        // Panel_Button_3を表示
        Panel_Button_3.SetActive(true);
    }
}
