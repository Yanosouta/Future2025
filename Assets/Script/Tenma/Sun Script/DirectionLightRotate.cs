using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionLightRotate : MonoBehaviour
{
    [Header("回転角度")]
    public float rotate = 0.5f;

    float rot = 1.0f;

    //夜はtrue
    bool bNight = false;

    public LightShaft lightShaft;

    public WaterEffect effect;

    [Header("エフェクト強さ")]
    [SerializeField]
    private float EffectStrength = 0.001f;
    Quaternion startangle;
    Color sunColor;
    float defIntensity;
    Light sun;

    public static DirectionLightRotate instance;
    

    private void Awake()
    {
        //Debug.Log(this.transform.eulerAngles.x);
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
        sun = this.GetComponent<Light>();
        defIntensity = sun.intensity;

        sunColor = this.GetComponent<Light>().color;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "Demo 1")
        {
            rot = 1.0f;
        }
        else
        {
            rot = 0.0f;
        }

        transform.Rotate(new Vector3(rotate * rot, 0, 0) * Time.deltaTime);

        //昼夜判定 夜であればtrue
        if (this.transform.eulerAngles.x > 0.0f && this.transform.eulerAngles.x < 180.0f)
        {
            bNight = false;
            sun.intensity = defIntensity;
            effect.strength = EffectStrength;
        }
        else //夜の間はIntensityを0にする
        {
            bNight = true;
            sun.intensity = 0;
            effect.strength = 0f;
        }

        //色を考える
        if (this.transform.eulerAngles.y > 90f && this.transform.eulerAngles.y < 200f)
        {
            if (sunColor.r != lightShaft.TargetColor.r) sunColor.r -= lightShaft.SubtractionSpeed;
            if (sunColor.g != lightShaft.TargetColor.g) sunColor.g -= lightShaft.SubtractionSpeed;
            if (sunColor.b != lightShaft.TargetColor.b) sunColor.b -= lightShaft.SubtractionSpeed;
        }
        else sunColor = lightShaft.retentionColor;
    }

    //太陽時間止める用の関数
    //例:時間を一時的に止めたい時
    public void StopRotate()
    {
        rot = 0.0f;
    }


    //太陽時間再開用の関数
    //例:時間止めた後に使用
    public void StartRotate()
    {
        rot = 1.0f;
    }


    //太陽の回転角度設定　※f倍速
    //例:夜の時間を早めたいとき
    public void SetRotate(float f)
    {
        rot = f;
    }

    //夜の時にtrueを返す
    public bool GetNight()
    {
        return bNight;
    }

    public void ResetRotate()
    {
        Debug.Log("リセットaaaaaaaaaaaaaaaaa");
        this.transform.Rotate(90.0f,0.0f,0.0f);

    }
}
