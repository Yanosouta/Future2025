using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorLIght : MonoBehaviour
{
    private DayWeatherManager Manager;

    public new Light light;

    public Material material;

    [Header("点滅回数")]
    [Range(0,5)]
    public int blinkCount = 3;

    [Header("点滅間隔")]
    [Range(0,1)]
    public float blinkInterval = 0.5f;

    [Header("光の強度")]
    [Range(0, 5)]
    public float intensityIncrement = 1.0f;

    //昼夜判定用
    private bool bDecision = false;

    private Color emissionColor;
    private Color defaultColor;

    private void Start()
    {
        Manager = FindObjectOfType<DayWeatherManager>();

        ChangeEmissionColor(Color.white);
        defaultColor = emissionColor = material.GetColor("_EmissionColor");
        emissionColor *= Mathf.Pow(2.0f, intensityIncrement);
    }

    void Update()
    {
        //昼から夜に変わった時
        if (Manager.GetCurrentTimeOfDay() == DayWeatherManager.TimeOfDay.Night 
            && Manager.GetBeforeTimeOfDay() != DayWeatherManager.TimeOfDay.Night)
        {
            //Lightを点滅させてから点ける
            StartCoroutine(BlinkAndTurnOn());
        }

        //昼はライトを消す
        if(Manager.GetCurrentTimeOfDay() != DayWeatherManager.TimeOfDay.Night)
        {
            light.enabled = false;
            ChangeEmissionColor(defaultColor);
        }

        //フラグの更新
        //bDecision = Manager.GetNight();
    }

    private IEnumerator BlinkAndTurnOn()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            light.enabled = !light.enabled;
            ChangeEmissionColor(emissionColor);
            yield return new WaitForSeconds(blinkInterval);
            light.enabled = !light.enabled;
            ChangeEmissionColor(defaultColor);
            yield return new WaitForSeconds(blinkInterval);
        }
        light.enabled = true;
        ChangeEmissionColor(emissionColor);
    }

    private void ChangeEmissionColor(Color c)
    {
        material.SetColor("_EmissionColor", c);
        material.EnableKeyword("_EMISSION");
    }
}
