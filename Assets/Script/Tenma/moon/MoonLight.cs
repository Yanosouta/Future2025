using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : MonoBehaviour
{
    public GameObject moonLight;
    public GameObject directionalLight;

    DirectionLightRotate obj;

    private void Start()
    {
        //コンポーネント取得
        obj = directionalLight.GetComponent<DirectionLightRotate>();
    }

    void Update()
    {
        //取得出来ていないなら再度取得
        if(obj == null)
            obj = directionalLight.GetComponent<DirectionLightRotate>();

        //夜ならON,昼ならOFF
        if (obj.GetNight())
            moonLight.SetActive(true);
        else
            moonLight.SetActive(false);
    }
}
