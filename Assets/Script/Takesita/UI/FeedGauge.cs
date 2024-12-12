using UnityEngine;
using UnityEngine.UI;

public class FeedGauge : MonoBehaviour
{
    public Image Gauge;  // UIゲージ（Image）
    public static bool fFeedFlg; // エサが投げられるかどうかのフラグ
    public GameObject FeedShine;
    public float gaugeSpeed = 0.2f; // ゲージのたまる速度

    void Start()
    {
        Gauge.fillAmount = 0; // 初期値を0に設定
        fFeedFlg = false;
    }

    void Update()
    {
        // ゲージがたまっていない場合、fillAmountを増加させる
        if (!fFeedFlg)
        {
            Gauge.fillAmount += Time.deltaTime * gaugeSpeed;
            
            // ゲージが最大値（1）になったらエサを投げられるようにする
            if (Gauge.fillAmount >= 1)
            {
                Gauge.fillAmount = 1; // 上限を1に固定
                fFeedFlg = true;
                FeedShine.SetActive(true);
            }
            else
            {
                FeedShine.SetActive(false);
            }
        }
    }
}