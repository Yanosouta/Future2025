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

    public float resetInterval = 4f; // リセット間隔（秒数）
    private bool canReset = true; // リセット可能かどうかのフラグ
    private int originalCullingMask; // 元のカリングマスクを保持
    public string hiddenLayerName = "Rain"; // 非表示にするレイヤー名
    private Camera parentCamera; // 親カメラへの参照

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
                canvas.PaintUVDirect(brush, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
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
}
