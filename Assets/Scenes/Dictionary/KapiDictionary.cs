using UnityEngine;
using UnityEngine.UI;

public class KapiDictionary : MonoBehaviour
{
    public Image[] imageSlots;      // 8つのImageオブジェクト
    public Sprite[] images;         // 全画像のリスト
    public Button nextButton;       // 次のページボタン
    public Button prevButton;       // 前のページボタン

    public bool[] RegistrationKapi; //カピが登録されているかの情報



    //カピ番号
    public int imageIndex = 0;


    private int currentPage = 0;    // 現在のページ番号
    private int imagesPerPage = 8;  // 1ページに表示する画像数

    private bool IsOnce = false;

    KapiDicData KapiData;
    KapinputMG KapiName;

    void Start()
    {
        // ボタンにリスナーを追加
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);

        // 最初のページでは「前へ」ボタンを無効にする
        prevButton.interactable = false;

        KapiData = new KapiDicData();
        //デバッグ用
        for (int i = 0; i < 24; i++)
        {
            if (KapiData.IsEntryKapi(i))
            {
                RegistrationKapi[i] = true;
            }
            else
            {
                RegistrationKapi[i] = false;
            }
        }

        UpdatePage();
    }

    void Update()
    {
        if (KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();
        if (KapiName == null)
            KapiName = FindObjectOfType<KapinputMG>();
        if (!IsOnce)
        {
            for (int i = 0; i < 24; i++)
            {
                if (KapiData.IsEntryKapi(i))
                {
                    RegistrationKapi[i] = true;
                }
                else
                {
                    RegistrationKapi[i] = false;
                }
            }
            UpdatePage();
        }
    }

    //ページ移動後の表示
    public void UpdatePage()
    {
        //デバッグ用
        for (int i = 0; i < 24; i++)
        {
            if (KapiData.IsEntryKapi(i))
            {
                RegistrationKapi[i] = true;
            }
            else
            {
                RegistrationKapi[i] = false;
            }
        }
        // 現在のページに対応する画像をセット
        for (int i = 0; i < imagesPerPage; i++)
        {
            imageIndex = currentPage * imagesPerPage;
            imageIndex += i;


            imageSlots[i].sprite = images[imageIndex];
            Color color = imageSlots[i].color;

            if (imageIndex < images.Length && RegistrationKapi[imageIndex] == true) //画像が存在していて、カピが登録されていれば
            {
                color.r = 1.0f;
                color.g = 1.0f;
                color.b = 1.0f;
                imageSlots[i].gameObject.SetActive(true);
            }
            else
            {
                // 画像がない場合はスロットを非表示に
                color.r = 0.0f;
                color.g = 0.0f;
                color.b = 0.0f;
            }
            imageSlots[i].color = color;
        }




            // ボタンの状態を更新
            prevButton.interactable = currentPage > 0;
        nextButton.interactable = (currentPage + 1) * imagesPerPage < images.Length;
    }

    //登録後の表示
    public void UpdateKapi()
    {
        // 現在のページに対応する画像をセット
        //デバッグ用
        for (int i = 0; i < 24; i++)
        {
            if (KapiData.IsEntryKapi(i))
            {
                RegistrationKapi[i] = true;
            }
            else
            {
                RegistrationKapi[i] = false;
            }
        }

        for (int i = 0; i < imagesPerPage; i++)
        {
            Color color = imageSlots[i].color;

            imageIndex = currentPage * imagesPerPage;
            imageIndex += i;

            if (imageIndex < images.Length && RegistrationKapi[imageIndex] == true) //画像が存在していて、カピが登録されていれば
            {
                color.r = 1.0f;
                color.g = 1.0f;
                color.b = 1.0f;
            }
            else
            {
                // 画像がない場合はスロットを非表示に
                color.r = 0.0f;
                color.g = 0.0f;
                color.b = 0.0f;
            }
            imageSlots[i].color = color;
        }
    }

    void NextPage()
    {
        if ((currentPage + 1) * imagesPerPage < images.Length)
        {
            currentPage++;
            UpdatePage();
            KapiName.UpdatePageName();
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
            KapiName.UpdatePageName();
        }
    }

    public int GetKapiPage()
    {
        int num;
        num = currentPage * imagesPerPage;
        return num;
    }

    public void ImageSlotsSort(bool Sort)
    {
        int Kapis = 24;
        Sprite C;

        if(Sort)
        {
            for (int i = 0; i < Kapis / 2; i++)
            {
                Debug.Log(i);
                Debug.Log((Kapis - 1) - i);
                C = images[i];
                images[i] = images[(Kapis - 1) - i];
                images[(Kapis - 1) - i] = C;
            }
        }
        //else
        //{
        //    int k;
        //    for (int i = 0; i < Kapis; i++)
        //    {
        //        if (images[i] == 0)
        //        {
        //            for (int j = i + 1; j < Kapis; j++)
        //            {
        //                if (IsKapi[j] == 1)
        //                {
        //                    images[i] = 1;
        //                    images[j] = 0;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
