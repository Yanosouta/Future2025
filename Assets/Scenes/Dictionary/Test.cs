using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public int KapiIdx = 0;
    KapiDicData KapiData;
   

    void Update()
    {
        if (KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();
    }

    public void RegistrationKapi()
    {
        int num = 3;

        KapiData.MarkAsKapi(num);
    }
}