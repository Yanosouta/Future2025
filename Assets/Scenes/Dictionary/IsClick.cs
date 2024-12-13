using UnityEngine;
using UnityEngine.EventSystems;

public class IsClick : MonoBehaviour
{
    public int KapiIdx = 0;
    SerectKapi DKapi;
    KapiDicData KapiData;
    KapiDictionary KapiDic;

    void Start()
    {
        DKapi = new SerectKapi();
        KapiDic = new KapiDictionary();
    }

    void Update()
    {
        if(DKapi == null)
            DKapi = FindObjectOfType<SerectKapi>();
        if(KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();
        if (KapiDic == null)
            KapiDic = FindObjectOfType<KapiDictionary>();　
    }

    public void KapiClick()
    {
        Debug.Log(DKapi);
       // DKapi.checkKapiPage(KapiIdx);
    }

    public void DestroyKapiClic()
    {
        KapiData.DestroyKapi();
        KapiDic.UpdatePage();
    }

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
        //KapiDic.UpdatePage();
    }

    public void ASortKapi()
    {
        KapiData.SortKapi(0);
        KapiDic.ImageSlotsSort(true);
        KapiDic.UpdatePage();
    }

    public void DSortKapi()
    {
        KapiData.SortKapi(2);
        KapiDic.UpdatePage();
    }

    public void MarkSortKapi()
    {
       // KapiData.SortKapi(2);
        //KapiDic.ImageSlotSort(false);
        //KapiDic.UpdatePage();
    }
}
