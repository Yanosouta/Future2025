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
    KapiDicData KapiData;

    void Start()
    {
        if (textComponent != null)
        {
            //textComponent.text = "新しいテキスト";  // 文字を変更
            NameChange(KapiName);
        }
    }

    void Update()
    {
        if (KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();


        textComponent.text = KapiData.LoadCharacterName(KapiId);
    }

    public void NameChange(string name)
    {
        textComponent.text = name;  // 文字を変更
    }
}
