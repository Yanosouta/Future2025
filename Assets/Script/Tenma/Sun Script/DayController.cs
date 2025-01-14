using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Directional Light�ɃA�^�b�`����X�N���v�g�B
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class DayController : DayWeatherManager
{
    [Header("��]���x")]
    [SerializeField]
    private float rot = 0.01f;

    public LightShaft lightShaft;

    [Header("�V�C���̌��̌����l")]
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
        // �C���X�^���X�����݂���ꍇ�͐V�����I�u�W�F�N�g���폜
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
    }

    private void Start()
    {
        DirectionLight = this.GetComponent<Light>();
        sunColor = this.GetComponent<Light>().color;

        defIntensity = DirectionLight.intensity;
    }


    void Update()
    {
        //���z�̉�]
        currentAngle += rotate * rot * Time.deltaTime;
        if (currentAngle >= 360f) currentAngle -= 360f;

        transform.rotation = Quaternion.Euler(new Vector3(currentAngle, 0, 0));

        //�ߋ��̎��Ԃ��L��
        beforeTimeOfDay = currentTimeOfDay;

        //����15�x��] //���̏o��0�x(6��)  //�^����90�x(12��)  //���̓��肪180�x(18��)
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


        //�V�C���l�� 
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


        //��̊Ԃ����Ђ���̂悳�𒲐�
        if (currentTimeOfDay == TimeOfDay.Night)
        {
            DirectionLight.intensity = 0.1f;
            //Debug.Log(DirectionLight.intensity);
        }
        

        //�F�̌���
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


        //**********�R�}���h***********
        //30���ň��
        if (Input.GetKey(KeyCode.LeftShift) 
            && Input.GetKey(KeyCode.O))
        {
            SetDirectionLightRotate(0.2f);
        }

        //6���ň��
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) 
            && Input.GetKey(KeyCode.O))
        {
            SetDirectionLightRotate(1f);
        }

        //30�b�ň��
        if(Input.GetKey(KeyCode.LeftShift) 
            && Input.GetKey(KeyCode.P))
        {
            SetDirectionLightRotate(12f);
        }
    }

    /// <summary>
    /// ���z�̉�]���~����(���Ԍo�ߎ~�܂�)
    /// </summary>
    public void StopDirectionLightRotate()
    {
        rot = 0.0f;
    }

    /// <summary>
    /// ���z�̉�]���ĊJ����(���Ԍo�߂����ɖ߂�)
    /// </summary>
    public void ReStartDirectionLIghtRotate()
    {
        rot = 1.0f;
    }

    /// <summary>
    /// ���z�̉�]��{���ɂ���(f�{��)
    /// </summary>
    public void SetDirectionLightRotate(float f)
    {
        rot = f;
    }
}
