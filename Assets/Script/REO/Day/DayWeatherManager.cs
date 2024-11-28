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

    public enum Weather
    {
        Sunny = 0,
        Cloudy,
        Rainy
    }

    protected static TimeOfDay currentTimeOfDay;
    protected static Weather currentWeather;

    protected float rot = 1.0f;

    protected Text weatherTimeText; // 天気と時間を表示するUIテキスト

    public static DayWeatherManager instance;

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
        // 環境に応じた更新処理（例：ライトやエフェクトの変更など）をここに追加

    }



    /// <summary>
    /// 太陽の回転を停止する(時間経過止まる)
    /// </summary>
    public void StopDirectionLightRotate()
    {
        rot = 0.0f;
    }

    /// <summary>
    /// 太陽の回転を再開する(時間経過を元に戻す)
    /// </summary>
    public void ReStartDirectionLIghtRotate()
    {
        rot = 1.0f;
    }

    /// <summary>
    /// 太陽の回転を倍速にする(f倍速)
    /// </summary>
    public void SetDirectionLightRotate(float f)
    {
        rot = f;
    }
}
