using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMouseClick : MonoBehaviour
{
    // 実行時にマウスのクリックを無効にするフラグ
    private bool disableMouse = true;

    void Update()
    {
        if (disableMouse)
        {
            // マウスのクリック入力を無効化
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                // マウス入力を無視するため、何もしない
                Debug.Log("Mouse click disabled.");
            }
        }
    }

    // 必要に応じて無効化を切り替えられるようにするメソッド
    public void SetMouseDisabled(bool disabled)
    {
        disableMouse = disabled;
    }
}
