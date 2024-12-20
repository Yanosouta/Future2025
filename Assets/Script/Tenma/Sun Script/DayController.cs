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
    private float rot = 0.01f;

    public LightShaft lightShaft;

    [Header("天気毎の光の減衰値")]
    [SerializeField]
    private float[] DecayRateIntencity = { 1.0f, 0.8f, 0.5f };

    public static new DayController instance;
    private Light DirectionLight;
    private float currentAngle;
    private float rotate = 1f;

    private float defIntensity;
    private Color sunColor;


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
        sunColor = this.GetComponent<Light>().color;

        defIntensity = DirectionLight.intensity;
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

        //夜の間だけひかりのつよさを調整
        if(currentTimeOfDay == TimeOfDay.Night)
        {
            DirectionLight.intensity = 0.2f;
        }
        else
        {
            DirectionLight.intensity = defIntensity;
        }

        //色の減衰
        if(currentTimeOfDay == TimeOfDay.Afternoon || currentTimeOfDay == TimeOfDay.Evening)
        {
            if (sunColor.r != lightShaft.TargetColor.r) 
                sunColor.r -= lightShaft.SubtractionSpeed;
            if (sunColor.g != lightShaft.TargetColor.g) 
                sunColor.g -= lightShaft.SubtractionSpeed;
            if (sunColor.b != lightShaft.TargetColor.b) 
                sunColor.b -= lightShaft.SubtractionSpeed;
        }
        else sunColor = lightShaft.retentionColor;



        //天気を考慮 
        switch (currentWeather)
        {
            case Weather.Sunny:
                DirectionLight.intensity = defIntensity * DecayRateIntencity[0];
                break;
            case Weather.Cloudy:
                DirectionLight.intensity = defIntensity * DecayRateIntencity[1];
                break;
            case Weather.Rainy:
                DirectionLight.intensity = defIntensity * DecayRateIntencity[2];
                break;
        }


        //**********コマンド***********
        //30分で一周
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.O))
        {
            SetDirectionLightRotate(0.2f);
        }

        //30秒で一周
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.P))
        {
            SetDirectionLightRotate(12f);
        }
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
