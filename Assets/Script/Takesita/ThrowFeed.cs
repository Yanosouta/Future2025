using UnityEngine;

public class ThrowFeed : MonoBehaviour
{
    public GameObject FeedPrefab; // 投げるエサのプレハブを設定
    public Transform spawnPoint; // エサの生成位置を設定

    private FeedGauge feedGauge;

    void Start()
    {
        // FeedGauge コンポーネントを取得
        feedGauge = FindObjectOfType<FeedGauge>();
    }

    void Update()
    {
        // キー入力でエサを投げる
        if (Input.GetKeyDown(KeyCode.P))
        {
            ThrowFeedPrefab();
        }
    }

    // エサを投げる処理
    void ThrowFeedPrefab()
    {
        // エサが投げられる状態か確認
        if (FeedGauge.fFeedFlg)
        {
            if (FeedPrefab != null && spawnPoint != null)
            {
                // プレハブを生成
                GameObject feedInstance = Instantiate(FeedPrefab, spawnPoint.position, spawnPoint.rotation);

                // Rigidbody に力を加えて前方に飛ばす
                Rigidbody rb = feedInstance.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(spawnPoint.forward * 500f); // 前方に力を加える
                }

                // フラグをリセットし、ゲージを0に戻す
                FeedGauge.fFeedFlg = false;
                feedGauge.Gauge.fillAmount = 0;
            }
            else
            {
                Debug.LogWarning("FeedPrefab または spawnPoint が設定されていません。");
            }
        }
    }
}