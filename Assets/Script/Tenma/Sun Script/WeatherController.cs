using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Directional Light�ɃA�^�b�`����X�N���v�g�B
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class WeatherController : DayWeatherManager
{
    [Serializable]
    public class WeatherWeight
    {
        public Weather weather;  // �V�C�̎��
        public float weight;     // �m���i�d�݁j
    }

    [Serializable]
    public class RipplesWeight
    {
        public bool OnRain;  //�J������ȊO��
        public float waitTime; //�J�̊Ԋu
    }

    [Header("�V�C�Ǝ��Ԃ�\������UI�e�L�X�g")]
    [SerializeField]
    private Text text;

    [Header("RainEffect(Obj)")]
    public GameObject rainObj;

    [Header("�g��\���̗� 1.�J,2.�ʏ�")]
    public List<RipplesWeight> ripples = new List<RipplesWeight>
    {
        new RipplesWeight { OnRain = true, waitTime = 0.1f },
        new RipplesWeight { OnRain = false, waitTime = 1f }
    };

    [Header("�V�C���Ƃ̊m��")]
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

        // �C���X�^���X�����݂��Ȃ��ꍇ�́A���̃I�u�W�F�N�g���C���X�^���X�Ƃ��Đݒ�
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

        //UI�e�L�X�g
        weatherTimeText = text;
    }

    void Start()
    {
        mesh = rainObj.GetComponent<MeshRenderer>();
        RandomInput = rainObj.GetComponent<Es.WaveformProvider.Sample.RandomWaveInput>();

        if(RandomInput == null)
        {
            Debug.Log("RandomInput �Q�ƃG���[");
        }
    }


    void Update()
    {
        //�J�Ȃ�
        if (currentWeather == Weather.Rainy)
        {
            //�G��G�t�F�N�g���s
            mesh.enabled = true;

            //�g���\�����s
            RandomInput.SetWaitTime(ripples[0].waitTime);
        }
        else
        {
            //�G��G�t�F�N�g�I�t
            mesh.enabled = false;

            //�g��\����x��
            RandomInput.SetWaitTime(ripples[1].waitTime);
        }


        //�[�������ɕς��^�C�~���O�̂�
        if(beforeTimeOfDay == TimeOfDay.Evening && currentTimeOfDay == TimeOfDay.Night)
        {
            SetRandomWeather();
            //Debug.Log(futureWeather);
        }


        //�邩�璩�ɕς��^�C�~���O�̂�
        if (beforeTimeOfDay == TimeOfDay.Night && currentTimeOfDay == TimeOfDay.Morning)
        {
            //Debug.Log("�V�C�\��");
            currentWeather = futureWeather;
        }
        //Debug.Log(currentWeather);
    }

    // �V�C�������_���ɐݒ�
    public void SetRandomWeather()
    {
        // �ݐϏd�݂̍��v���v�Z
        float totalWeight = weatherWeights.Sum(w => w.weight);

        // 0���獇�v�d�݂͈̔͂ŗ����𐶐�
        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        // �d�݂͈̔͂Ɋ�Â��ēV�C��I�o
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
