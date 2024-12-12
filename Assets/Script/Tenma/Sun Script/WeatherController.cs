using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Directional Lightにアタッチするスクリプト。
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class WeatherController : DayWeatherManager
{
    [Serializable]
    public class WeatherWeight
    {
        public Weather weather;  // 天気の種類
        public float weight;     // 確率（重み）
    }

    [Header("天気と時間を表示するUIテキスト")]
    [SerializeField]
    private Text text;

    [Header("RainEffect(Obj)")]
    public GameObject rainObj;

    [Header("天気ごとの確率")]
    public List<WeatherWeight> weatherWeights = new List<WeatherWeight>
    {
        new WeatherWeight { weather = Weather.Sunny, weight = 50.0f },
        new WeatherWeight { weather = Weather.Cloudy, weight = 30.0f },
        new WeatherWeight { weather = Weather.Rainy, weight = 20.0f }
    };

    private TimeOfDay beforeTimeOfDay;
    private MeshRenderer mesh;

    public static new WeatherController instance;

    private void Awake()
    {
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

        //UIテキスト
        weatherTimeText = text;
    }

    void Start()
    {
        beforeTimeOfDay = currentTimeOfDay;
        mesh = rainObj.GetComponent<MeshRenderer>();
    }


    void Update()
    {
        //Debug.Log("Current time of day: " + currentTimeOfDay);
        //Debug.Log("Before time of day: " + beforeTimeOfDay);
        if (currentWeather == Weather.Rainy) mesh.enabled = true;
        else mesh.enabled = false;

        //夕方から夜に変わるタイミングのみ
        if(beforeTimeOfDay == TimeOfDay.Evening && currentTimeOfDay == TimeOfDay.Night)
        {
            SetRandomWeather();
            Debug.Log(futureWeather);
        }


        //夜から朝に変わるタイミングのみ
        if (beforeTimeOfDay == TimeOfDay.Night && currentTimeOfDay == TimeOfDay.Morning)
        {
            //Debug.Log("天気予報");
            currentWeather = futureWeather;
        }
        //Debug.Log(currentWeather);

        beforeTimeOfDay = currentTimeOfDay;
    }

    // 天気をランダムに設定
    public void SetRandomWeather()
    {
        // 累積重みの合計を計算
        float totalWeight = weatherWeights.Sum(w => w.weight);

        // 0から合計重みの範囲で乱数を生成
        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        // 重みの範囲に基づいて天気を選出
        float cumulativeWeight = 0;
        foreach (var weatherWeight in weatherWeights)
        {
            cumulativeWeight += weatherWeight.weight;
            if (randomValue < cumulativeWeight)
            {
                futureWeather = weatherWeight.weather;
                break;
            }
        }

        UpdateEnvironment();
    }
}
