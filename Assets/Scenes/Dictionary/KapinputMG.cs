using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KapinputMG : MonoBehaviour
{
    public InputField[] inputFields;    //名前入力欄
    public GameObject[] Object;

    private int currentPage = 0;    // 現在のページ番号
    private int imagesPerPage = 8;  // 1ページに表示する画像数

    private InputKapiName inputKapiName;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveNameInput(int idx)
    {
        if (idx >= 0 && idx < inputFields.Length && inputFields[idx] != null)
        {
            EventSystem.current.SetSelectedGameObject(inputFields[idx].gameObject);
            inputFields[idx].ActivateInputField();
        }
    }

    public void UpdatePageName()
    {
        for (int i = 0; i < imagesPerPage; i++)
        {
            inputKapiName = Object[i].GetComponent<InputKapiName>();
            inputKapiName.UpdateDisplayedName();
        }
    }
    public void HandleEndEdit()
    {
        // フォーカスを解除（選択状態も解除される）
        EventSystem.current.SetSelectedGameObject(null);
    }
}
