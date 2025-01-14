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

    [Serializable]
    public class RipplesWeight
    {
        public bool OnRain;  //雨かそれ以外か
        public float waitTime; //雨の間隔
    }

    [Header("天気と時間を表示するUIテキスト")]
    [SerializeField]
    private Text text;

    [Header("RainEffect(Obj)")]
    public GameObject rainObj;

    [Header("波紋表現の量 1.雨,2.通常")]
    public List<RipplesWeight> ripples = new List<RipplesWeight>
    {
        new RipplesWeight { OnRain = true, waitTime = 0.1f },
        new RipplesWeight { OnRain = false, waitTime = 1f }
    };

    [Header("天気ごとの確率")]
    public List<WeatherWeight> weatherWeights = new List<WeatherWeight>
    {
        new WeatherWeight { weather = Weather.Sunny, weight = 50.0f },
        new WeatherWeight { weather = Weather.Cloudy, weight = 30.0f },
        new WeatherWeight { weather = Weather.Rainy, weight = 20.0f }
    };

    private MeshRenderer mesh;
    private Es.WaveformProvider.Sample.RandomWaveInput RandomInput;

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
        mesh = rainObj.GetComponent<MeshRenderer>();
        RandomInput = rainObj.GetComponent<Es.WaveformProvider.Sample.RandomWaveInput>();

        if(RandomInput == null)
        {
            Debug.Log("RandomInput 参照エラー");
        }
    }


    void Update()
    {
        //雨なら
        if (currentWeather == Weather.Rainy)
        {
            //濡れエフェクト実行
            mesh.enabled = true;

            //波紋を表現実行
            RandomInput.SetWaitTime(ripples[0].waitTime);
        }
        else
        {
            //濡れエフェクトオフ
            mesh.enabled = false;

            //波紋表現を遅く
            RandomInput.SetWaitTime(ripples[1].waitTime);
        }


        //夕方から夜に変わるタイミングのみ
        if(beforeTimeOfDay == TimeOfDay.Evening && currentTimeOfDay == TimeOfDay.Night)
        {
            SetRandomWeather();
            //Debug.Log(futureWeather);
        }


        //夜から朝に変わるタイミングのみ
        if (beforeTimeOfDay == TimeOfDay.Night && currentTimeOfDay == TimeOfDay.Morning)
        {
            //Debug.Log("天気予報");
            currentWeather = futureWeather;
        }
        //Debug.Log(currentWeather);
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
