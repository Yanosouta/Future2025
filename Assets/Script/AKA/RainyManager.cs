using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyManager : MonoBehaviour
{
    private DayWeatherManager manager;
    void Start()
    {
        //manager = GameObject.FindWithTag("Manager").GetComponent<DayWeatherManager>();
        manager = GetComponent<DayWeatherManager>();
        if(manager == null)
        {
            Debug.Log("Managerが見つかりません");
        }
    }

    public GameObject RainEffect; // 雨のエフェクト用のGameObject
    public ParticleSystem RainParticle; // 雨のパーティクルシステム

    // Update is called once per frame
    void Update()
    {
        if(manager.GetCurrentWeather() == DayWeatherManager.Weather.Rainy)
        {
            // 天気がRainyの場合にパーティクルを再生
            if (RainParticle != null && !RainParticle.isPlaying)
            {
                RainParticle.Play();
                Debug.Log("Rainy状態: 雨のパーティクルを再生");
            }
        }

        else
        {
            if (RainParticle != null && RainParticle.isPlaying)
            {
                RainParticle.Stop();
                Debug.Log("Rainyではない: 雨のパーティクルを停止");
            }
        }   
    }
}