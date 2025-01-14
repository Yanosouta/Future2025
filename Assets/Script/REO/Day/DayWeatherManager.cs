using UnityEngine;
using UnityEngine.UI; // UI�e�L�X�g�̎g�p�ɕK�v
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
    //15�x/h
    //0.004�x/s

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

    protected Text weatherTimeText; // �V�C�Ǝ��Ԃ�\������UI�e�L�X�g
    public static DayWeatherManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // ���[�g�I�u�W�F�N�g��DontDestroyOnLoad��K�p
        if (transform.parent == null) // ���[�g�I�u�W�F�N�g���m�F
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            // ���[�g�I�u�W�F�N�g�łȂ���΃��[�g���擾
            GameObject rootObject = transform.root.gameObject;
            DontDestroyOnLoad(rootObject); // ���[�g�I�u�W�F�N�g�ɓK�p
        }
    }

    private void Start()
    {
        // �����l�̐ݒ�
        currentTimeOfDay = beforeTimeOfDay = TimeOfDay.Morning;
        currentWeather = futureWeather = Weather.Sunny;

        UpdateEnvironment();
    }

    private void Update()
    {
        // �f�o�b�O�p: �e�L�[����������V�C�ύX
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("�V�C��Sunny�ɐݒ�");
            SetWeather(Weather.Sunny);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("�V�C��Cloudy�ɐݒ�");
            SetWeather(Weather.Cloudy);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("�V�C��Rainy�ɐݒ�");
            SetWeather(Weather.Rainy);
        }
    }

    // ���̃X�N���v�g����V�C���Z�b�g���郁�\�b�h
    // ---------------�g�p��--------------------
    // ����ɐݒ�
    // DayWeaterManager weatherManager;
    // weatherManager.SetWeather(DayWeatherManager.Weather.Sunny);
    // -----------------------------------------
    public void SetWeather(Weather newWeather)
    {
        currentWeather = newWeather;
        UpdateEnvironment();
    }

    // ���݂̓V�C���擾���郁�\�b�h
    // ---------------�g�p��--------------------
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


    // ���݂̎��ԑт��擾���郁�\�b�h
    // ---------------�g�p��--------------------
    // DayWeatherManager.TimeOfDay currentTime = weatherManager.GetCurrentTimeOfDay();
    // -----------------------------------------
    public TimeOfDay GetCurrentTimeOfDay()
    {
        return currentTimeOfDay;
    }

    // �����X�V���郁�\�b�h
    public void UpdateEnvironment()
    {
        //Debug.Log("Manajer Current time of day: " + currentTimeOfDay);
        //Debug.Log("Current weather: " + currentWeather);
        
        // �e�L�X�g�Ɍ��݂̓V�C�Ǝ��Ԃ�\��
        if (weatherTimeText != null)
        {
            weatherTimeText.text = "Time of Day: " + currentTimeOfDay + "\nWeather: " + currentWeather;
        }
    }
}
