using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputKapiName : MonoBehaviour
{
    [SerializeField] InputField nameInputField; // InputField (TextMeshPro版)
    [SerializeField] Text displayNameText;      // 名前を表示するText
    private string playerNameKey = "PlayerName";           // PlayerPrefsのキー

    private KapiDicData KapiData;
    private SerectKapi SKapi;
    private KapiDictionary KapiNum;

    public int SaveNum = 0;
    public int PageNum = 0; //ページにおけるImageの番号



    // ゲーム開始時に保存された名前を表示
    private void Start()
    {
        //インスタンスの取得
        KapiData = new KapiDicData();
    }

    void Update()
    {
        if (KapiData == null)
        {
            KapiData = FindObjectOfType<KapiDicData>();
        }
        if (SKapi == null)
        {
            SKapi = FindObjectOfType<SerectKapi>();
        }
        if (KapiNum == null)
        {
            KapiNum = FindObjectOfType<KapiDictionary>();
            UpdateDisplayedName();
        }
    }

    // 名前を保存する
    public void SaveName()
    {
        string enteredName = nameInputField.text;

        if (!string.IsNullOrEmpty(enteredName))
        {
            KapiData.SaveCharacterName(KapiNum.GetKapiPage() + PageNum, enteredName); // 名前を保存
            UpdateDisplayedName();
        }
        else
        {
            Debug.LogWarning("名前が入力されていません！");
        }
    }



    // 保存された名前を読み込んで表示
    public void UpdateDisplayedName()
    {
        SaveNum = KapiNum.GetKapiPage() + PageNum;

        // PlayerPrefsから名前を取得 (デフォルト値は「キャラクター + インデックス」)
        string characterName = KapiData.LoadCharacterName(SaveNum);

        // テキストに名前を表示
        if (characterName != null)
        {
            nameInputField.text = $"{characterName}";
            Debug.Log(characterName);
        }
    }
}
