using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : DayWeatherManager
{
    [Header("Horizon")]
    public GameObject obj;

    void Update()
    {
        //�J�̏ꍇ
        if (currentWeather == Weather.Rainy)
            obj.SetActive(true);

        //�J�ȊO�̏ꍇ
        else
            obj.SetActive(false);
    }
}
