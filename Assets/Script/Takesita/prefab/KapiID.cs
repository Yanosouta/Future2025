using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UIにアクセスするための名前空間
public class KapiID : MonoBehaviour
{
    public int KapiId;
    public string KapiName;
    // Start is called before the first frame update
    public Text textComponent;  // インスペクターからTextコンポーネントを指定

    void Start()
    {
        if (textComponent != null)
        {
            //textComponent.text = "新しいテキスト";  // 文字を変更
            NameChange();
        }
    }

    void NameChange()
    {
        textComponent.text = KapiName;  // 文字を変更
    }
}
