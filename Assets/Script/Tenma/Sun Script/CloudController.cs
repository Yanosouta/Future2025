using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : DayWeatherManager
{
    [Header("Horizon")]
    public GameObject obj;

    void Update()
    {
        //‰J‚Ìê‡
        if (currentWeather == Weather.Rainy)
            obj.SetActive(true);

        //‰JˆÈŠO‚Ìê‡
        else
            obj.SetActive(false);
    }
}
