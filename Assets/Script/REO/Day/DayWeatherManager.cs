using UnityEngine;
using UnityEngine.UI; // UIテキストの使用に必要
using System;
using System.Collections.Generic;

public class DayWeatherManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning = 0,   //6~10
        Afternoon, //10~16
        Evening,   //16~18
        Night      //18~6
    }
    //15度/h
    //0.004度/s

    public enum Weather
    {
        Sunny = 0,
        Cloudy,
        Rainy
    }

    protected static TimeOfDay beforeTimeOfDay;
    protected static TimeOfDay currentTimeOfDay;
    protected static Weather currentWeather;
    protected static Weather futureWeather;

    protected Text weatherTimeText; // 天気と時間を表示するUIテキスト
    public static DayWeatherManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

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
        // 初期値の設定
        currentTimeOfDay = TimeOfDay.Morning;
        currentWeather = Weather.Sunny;

        UpdateEnvironment();
    }

    private void Update()
    {
        // デバッグ用: 各キーを押したら天気変更
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("天気をSunnyに設定");
            SetWeather(Weather.Sunny);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("天気をCloudyに設定");
            SetWeather(Weather.Cloudy);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("天気をRainyに設定");
            SetWeather(Weather.Rainy);
        }
    }

    // 他のスクリプトから天気をセットするメソッド
    // ---------------使用例--------------------
    // 晴れに設定
    // DayWeaterManager weatherManager;
    // weatherManager.SetWeather(DayWeatherManager.Weather.Sunny);
    // -----------------------------------------
    public void SetWeather(Weather newWeather)
    {
        currentWeather = newWeather;
        UpdateEnvironment();
    }

    // 現在の天気を取得するメソッド
    // ---------------使用例--------------------
    //  DayWeatherManager.Weather currentWeather = weatherManager.GetCurrentWeather();
    // -----------------------------------------
    public Weather GetCurrentWeather()
    {
        return currentWeather;
    }


    public Weather GetFutureWeather()
    {
        return futureWeather;
    }


    public TimeOfDay GetBeforeTimeOfDay()
    {
        return beforeTimeOfDay;
    }


    // 現在の時間帯を取得するメソッド
    // ---------------使用例--------------------
    // DayWeatherManager.TimeOfDay currentTime = weatherManager.GetCurrentTimeOfDay();
    // -----------------------------------------
    public TimeOfDay GetCurrentTimeOfDay()
    {
        return currentTimeOfDay;
    }

    // 環境を更新するメソッド
    public void UpdateEnvironment()
    {
        //Debug.Log("Manajer Current time of day: " + currentTimeOfDay);
        //Debug.Log("Current weather: " + currentWeather);
        
        // テキストに現在の天気と時間を表示
        if (weatherTimeText != null)
        {
            weatherTimeText.text = "Time of Day: " + currentTimeOfDay + "\nWeather: " + currentWeather;
        }
    }
}
