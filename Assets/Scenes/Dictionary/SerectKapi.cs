using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerectKapi : MonoBehaviour
{
    KapiDicData KapiData;
    KapiDictionary KapiNum;
    RectTransform rectTransform;
    KapinputMG Nadukeoya;

    public int SerectKapinum = 0;
    public Image targetImage;   //選択UI
    public Vector2[] Vec;

    private int Kapidx = 0;

    // Start is called before the first frame update
    void Start()
    {
        //インスタンスの取得
        KapiData = new KapiDicData();

        if (targetImage != null)
        {
            // RectTransformを取得
            rectTransform = targetImage.GetComponent<RectTransform>();

            // 新しい位置を設定 (アンカー基準のローカル座標)
            rectTransform.anchoredPosition = new Vector2(Vec[0].x,Vec[0].y);
        }
        SerectKapinum = 0;
    }

    void Update()
    {
        //キー入力されたか
        bool IsInput = false;

        if (KapiData == null)
        {
            KapiData = FindObjectOfType<KapiDicData>();
        }
        if (KapiNum == null)
        {
            KapiNum = FindObjectOfType<KapiDictionary>();
        }
        if(Nadukeoya == null)
        {
            Nadukeoya = FindObjectOfType<KapinputMG>();
        }


        int PageNum = 0;
        //→
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PageNum = KapiNum.GetKapiPage();
            //選択カピの変更
            Kapidx++;
            if (Kapidx > 7)
                Kapidx = 0;
            //クリックしたカピの番号保持
            SerectKapinum = PageNum + Kapidx;

            Debug.Log("現在選択中カピ:" + SerectKapinum);
            IsInput = true;
        }
        //←
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PageNum = KapiNum.GetKapiPage();
            //選択カピの変更
            Kapidx--;
            if (Kapidx < 0)
                Kapidx = 7;
            //クリックしたカピの番号保持
            SerectKapinum = PageNum  + Kapidx;

            Debug.Log("現在選択中カピ:" + SerectKapinum);
            IsInput = true;
        }
        //↑
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PageNum = KapiNum.GetKapiPage();
            //選択カピの変更
            Kapidx -= 4;
            if (Kapidx < 0)
                Kapidx = 8 - (Kapidx * -1);
            //クリックしたカピの番号保持
            SerectKapinum = PageNum + Kapidx;

            Debug.Log("現在選択中カピ:" + SerectKapinum);
            IsInput = true;
        }
        //↓
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PageNum = KapiNum.GetKapiPage();
            //選択カピの変更
            Kapidx += 4;
            if (Kapidx > 7)
                Kapidx = 0 + (Kapidx - 7) -1;
            //クリックしたカピの番号保持
            SerectKapinum = PageNum + Kapidx;

            Debug.Log("現在選択中カピ:" + SerectKapinum);
            IsInput = true;
        }


        if(Input.GetKeyDown(KeyCode.Return))//エンターが入力されたら選択中カピの名前入力を可能に
        {
            Nadukeoya.ActiveNameInput(SerectKapinum);
        }


        if (IsInput)
        {
            rectTransform.anchoredPosition = new Vector2(Vec[Kapidx].x, Vec[Kapidx].y);
        }
    }


    public int GetKapiNumber()
    {
        return SerectKapinum;
    }

}
