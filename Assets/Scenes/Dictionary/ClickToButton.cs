using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickRoButton : MonoBehaviour
{
    public int KapiIdx = 0;
    SerectKapi DKapi;
    KapiDicData KapiData;
    KapiDictionary KapiDic;

    public GameObject m_camera;
    // コントローラーもの
    ControllerState m_State;

    void Start()
    {
        // コントローラー
        m_State = m_camera.GetComponent<ControllerState>();
        DKapi = new SerectKapi();
        KapiDic = new KapiDictionary();
    }

    void Update()
    {
        if (DKapi == null)
            DKapi = FindObjectOfType<SerectKapi>();
        if (KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();
        if (KapiDic == null)
            KapiDic = FindObjectOfType<KapiDictionary>();

        //ページ移動
        //if (m_State.GetButtonL())
        //{
        //    KapiDic.NextPage();
        //}
        //if (m_State.GetButtonR())
        //{
        //    KapiDic.PreviousPage();
        //}
        ////昇順降順
        //if (m_State.GetButtonX())
        //{
        //    ASortKapi();
        //}
        //登録(デバッグ用なのでコントローラ対応不必要)
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            RegistrationKapi();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DestroyKapi();
        }

    }

    //削除(基本これ呼びだすことない)
    public void DestroyKapi()
    {
        KapiData.DestroyKapi();
        KapiDic.UpdatePage();
    }

    //登録(Discordに登録の仕方書いた)
    public void RegistrationKapi()
    {
        int num = 0;
        num = DKapi.GetKapiNumber();

        KapiData.MarkAsKapi(num);
        if (KapiDic.imageSlots != null)
        {
            KapiDic.UpdatePage();
        }
        else
        {
            Debug.LogError("UpdateKapiを呼び出す前にimageSlotsがnullです。");
        }
    }

    //昇順
    public void ASortKapi()
    {
        KapiData.SortKapi(0);
        KapiDic.ImageSlotsSort(true);
        KapiDic.UpdatePage();
    }

    //降順
    public void DSortKapi()
    {
        KapiData.SortKapi(2);
        KapiDic.UpdatePage();
    }
}
