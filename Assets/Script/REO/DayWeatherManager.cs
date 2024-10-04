using UnityEngine;
using UnityEngine.UI; // UIテキストの使用に必要

public class DayWeatherManager : MonoBehaviour
{
    public enum TimeOfDay
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }

    public enum Weather
    {
        Sunny,
        Cloudy,
        Rainy
    }

    public TimeOfDay currentTimeOfDay { get; private set; }
    public Weather currentWeather { get; private set; }

    public Text weatherTimeText; // 天気と時間を表示するUIテキスト

    void Start()
    {
        // 初期値の設定
        currentTimeOfDay = TimeOfDay.Morning;
        currentWeather = Weather.Sunny;

        UpdateEnvironment();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // キー操作で時間帯を切り替える
        {
            CycleTimeOfDay();
        }

        if (Input.GetKeyDown(KeyCode.W)) // キー操作で天気をランダムに変更する
        {
            SetRandomWeather();
        }
    }

    // 時間帯を順番に変更
    public void CycleTimeOfDay()
    {
        currentTimeOfDay = (TimeOfDay)(((int)currentTimeOfDay + 1) % System.Enum.GetValues(typeof(TimeOfDay)).Length);
        UpdateEnvironment();
    }

    // 天気をランダムに設定
    public void SetRandomWeather()
    {
        int weatherCount = System.Enum.GetValues(typeof(Weather)).Length;
        currentWeather = (Weather)Random.Range(0, weatherCount);
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

    // 他のスクリプトから時間帯をセットするメソッド
    // ---------------使用例--------------------
    // 時間を夜に設定
    // DayWeaterManager weatherManager;
    // weatherManager.SetTimeOfDay(DayWeatherManager.TimeOfDay.Night);
    // -----------------------------------------
    public void SetTimeOfDay(TimeOfDay newTimeOfDay)
    {
        currentTimeOfDay = newTimeOfDay;
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
    void UpdateEnvironment()
    {
        Debug.Log("Current time of day: " + currentTimeOfDay);
        Debug.Log("Current weather: " + currentWeather);
        
        // テキストに現在の天気と時間を表示
        if (weatherTimeText != null)
        {
            weatherTimeText.text = "Time of Day: " + currentTimeOfDay + "\nWeather: " + currentWeather;
        }
        // 環境に応じた更新処理（例：ライトやエフェクトの変更など）をここに追加

    }
}
