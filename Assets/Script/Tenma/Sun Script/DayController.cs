using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Directional Lightにアタッチするスクリプト。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class DayController : DayWeatherManager
{
    [Header("回転速度")]
    [SerializeField]
    private float rotate = 0.5f;

    [Header("太陽の光の強さ")]
    [SerializeField]
    private float[] TimeIntensity = { 1f, 1f, 1f, 1f };

    [Header("曇と雨の光の減衰値")]
    [SerializeField]
    private float[] DecayRateIntencity = { 0.8f, 0.5f };

    public static new DayController instance;
    private Light DirectionLight;
    private float totalIntencity = 1.0f;
    private float currentAngle;

    private void Awake()
    {
        //Debug.Log(currentAngle);
        //startangle = this.transform.rotation;
        // インスタンスが存在する場合は新しいオブジェクトを削除
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // インスタンスが存在しない場合は、このオブジェクトをインスタンスとして設定
        instance = this;

        // ルートオブジェクトにDontDestroyOnLoadを適用
        if (transform.parent == null) // ルートオブジェクトか確認
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            // ルートオブジェクトでなければルートを取得
            GameObject rootObject = transform.root.gameObject;
            DontDestroyOnLoad(rootObject); // ルートオブジェクトに適用
        }
    }

    private void Start()
    {
        DirectionLight = this.GetComponent<Light>();
    }


    void Update()
    {
        //太陽の回転
        currentAngle += rotate * rot * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        transform.rotation = Quaternion.Euler(new Vector3(currentAngle, 0, 0));

        //毎時15度回転 //日の出が0度(6時)  //真昼が90度(12時)  //日の入りが180度(18時)
        if (currentAngle > 0.0f && currentAngle < 60.0f)
        {//6~10
            currentTimeOfDay = TimeOfDay.Morning;
            UpdateEnvironment();
        }
        else if (currentAngle > 60.0f && currentAngle < 150.0f)
        {//10~16
            currentTimeOfDay = TimeOfDay.Afternoon;
            UpdateEnvironment();
        }
        else if (currentAngle > 150.0f && currentAngle < 180.0f)
        {//16~18
            currentTimeOfDay = TimeOfDay.Evening;
            UpdateEnvironment();
        }
        else if (currentAngle > 180.0f && currentAngle < 360.0f)
        {//18~6
            currentTimeOfDay = TimeOfDay.Night;
            UpdateEnvironment();
        }

        //太陽の光の強さ
        DirectionLightIntencity();

        //光の強さの更新
        DirectionLight.intensity = totalIntencity;

        //Debug.Log(currentTimeOfDay);
        //Debug.Log(currentAngle);
        //Debug.Log(currentWeather);
        //Debug.Log(DirectionLight.intensity);
    }



    private void DirectionLightIntencity()
    {
        //時間帯別に設定
        switch (currentTimeOfDay)
        {
            //朝
            case TimeOfDay.Morning:
                totalIntencity = TimeIntensity[0];
                break;

            //昼
            case TimeOfDay.Afternoon:
                totalIntencity = TimeIntensity[1];
                break;

            //夕
            case TimeOfDay.Evening:
                totalIntencity = TimeIntensity[2];
                break;

            //夜
            case TimeOfDay.Night:
                totalIntencity = TimeIntensity[3];
                break;
        }

        //天気別に設定
        switch (currentWeather)
        {
            //晴
            case Weather.Sunny:
                break;

            //曇
            case Weather.Cloudy:
                totalIntencity /= DecayRateIntencity[0];
                break;

            //雨
            case Weather.Rainy:
                totalIntencity /= DecayRateIntencity[1];
                break;
        }
    }
}
