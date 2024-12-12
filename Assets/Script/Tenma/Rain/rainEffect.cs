using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class rainEffect : MonoBehaviour
{
    [SerializeField]
    private InkCanvas canvas;

    [SerializeField]
    private Brush brush;

    [SerializeField]
    private float minSize;

    [SerializeField]
    private float maxSize;

    [SerializeField, Range(0, 100)]
    private float aboutCallPercentOnFrame = 10;

    [SerializeField]
    private float radius = 0.2f;

    public float resetInterval = 4f; // リセット間隔（秒数）
    private bool canReset = true; // リセット可能かどうかのフラグ
    private int originalCullingMask; // 元のカリングマスクを保持
    public string hiddenLayerName = "Rain"; // 非表示にするレイヤー名
    private Camera parentCamera; // 親カメラへの参照
    private Material mat;
    private float IncrementCount = 0f;
    private float DecrementCount = 50f;
    private DayWeatherManager manager;

    private void Awake()
    {
        mat = this.gameObject.GetComponent<Renderer>().material;

        if(mat == null)
        {
            Debug.Log("Materialを読み込めませんでした");
        }
    }

    private void Start()
    {
        // 親のカメラコンポーネントを取得
        parentCamera = GetComponentInParent<Camera>();

        // 親カメラが見つからない場合はエラーログを表示
        if (parentCamera == null)
        {
            Debug.LogError("カメラが見つかりません！");
            return;
        }

        // レイヤーを設定
        gameObject.layer = LayerMask.NameToLayer(hiddenLayerName);
        originalCullingMask = parentCamera.cullingMask; // 元のマスクを保存

        manager = GameObject.FindWithTag("Manager").GetComponent<DayWeatherManager>();
        if(manager == null)
        {
            Debug.Log("Managerが見つかりませんでした");
        }
    }

    private void Update()
    {
        //Debug.Log("今" + manager.GetCurrentWeather());
        //Debug.Log("未来" + manager.GetFutureWeather());

        //最初はカウントアップして問題ない
        //StartCoroutine(IncrementCountOverTime(1f));

        ////
        //if (manager.GetCurrentTimeOfDay() == DayWeatherManager.TimeOfDay.Night)
        //{
        //    StartCoroutine(DecrementCountOverTime(2f));
        //}

        ////雨でなければカウントリセット
        //if (manager.GetCurrentWeather() != DayWeatherManager.Weather.Rainy)
        //{
        //    IncrementCount = 0;
        //    DecrementCount = 50;
        //}
    }


    private void OnWillRenderObject()
    {
        if (Random.Range(0f, 100f) > aboutCallPercentOnFrame)
            return;

        // リセット可能な場合のみ画面リセットを開始
        if (canReset)
        {
            StartCoroutine(ResetScreen());
        }
        else
        {
            brush.Color = new Color(255, 255, 255, 0);

            // リセットされない場合は通常の描画処理を実行
            for (int i = Random.Range(1, 10); i > 0; --i)
            {
                brush.Scale = Random.Range(minSize, maxSize);

                // ランダムな位置を生成
                Vector2 randomPosition = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

                // 中心から一定の半径内に位置しないようにする
                if (Vector2.Distance(randomPosition, new Vector2(0.5f, 0.5f)) > radius)
                {
                    canvas.PaintUVDirect(brush, randomPosition);
                }
                else
                {
                    i++; // 再度位置を生成する
                }
            }
        }
    }



    private IEnumerator ResetScreen()
    {
        // 画面のリセット処理を実行
        brush.Scale = 1;
        canvas.PaintUVDirect(brush, new Vector2(0, 0));
        canvas.PaintUVDirect(brush, new Vector2(0, 1));
        canvas.PaintUVDirect(brush, new Vector2(0.5f, 0.5f));
        canvas.PaintUVDirect(brush, new Vector2(1, 0));
        canvas.PaintUVDirect(brush, new Vector2(1, 1));

        canReset = true; // 一定間隔が経過するまでリセットを無効にする
        yield return new WaitForSeconds(resetInterval); // リセット間隔待機
       
        canReset = false; // 再びリセット可能にする
    }


    public void RainMood(bool b) //falseは晴れ、trueは雨
    {
        if(b == false) parentCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(hiddenLayerName));
        else parentCamera.cullingMask = originalCullingMask;
    }


    private IEnumerator IncrementCountOverTime(float timeInterval)
    {
        //今日雨でなければ返す
        if (manager.GetCurrentWeather() != DayWeatherManager.Weather.Rainy)
            yield break;

        //countが50までやる
        if(IncrementCount >= 50f)
            yield break;

        while (true)
        {
            // 指定した時間だけ待機
            yield return new WaitForSeconds(timeInterval);

            Debug.Log("インクリメント");
            Debug.Log(IncrementCount);

            // countを1進める
            IncrementCount++;
            mat.SetFloat("_BumpAmt", IncrementCount);
            //Debug.Log("IncrementCount: " + IncrementCount);
        }
    }


    private IEnumerator DecrementCountOverTime(float timeInterval)
    {
        //今日雨でなければ返す
        if(manager.GetCurrentWeather() != DayWeatherManager.Weather.Rainy)
            yield break;

        //今日雨、明日も雨であれば返す
        if (manager.GetFutureWeather() == DayWeatherManager.Weather.Rainy)
            yield break;

        //countが0までやる
        if (DecrementCount <= 0f)
            yield break;

        //今日雨の時だけ実行
        while (true)
        {
            // 指定した時間だけ待機
            yield return new WaitForSeconds(timeInterval);

            Debug.Log("デクリメント");

            // countを1戻す
            DecrementCount--;
            mat.SetFloat("_BumpAmt", DecrementCount);
            //Debug.Log("DecrementCount: " + DecrementCount);
        }
    }
}
