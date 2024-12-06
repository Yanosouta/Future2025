using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFeed : MonoBehaviour
{
    public GameObject FeedPrefab; // 投げるエサのプレハブを設定
    public Transform spawnPoint; // エサの生成位置を設定（例えば、カメラの前方など）

    // Start is called before the first frame update
    void Start()
    {
        // 必要に応じて初期化処理
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ThrowFeedPrefab();
        }
    }

    // エサを投げる処理
    void ThrowFeedPrefab()
    {
        if (FeedPrefab != null && spawnPoint != null)
        {
            // プレハブを生成
            GameObject feedInstance = Instantiate(FeedPrefab, spawnPoint.position, spawnPoint.rotation);

            // 必要に応じて生成後の処理（例: Rigidbodyで力を加える）
            Rigidbody rb = feedInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(spawnPoint.forward * 500f); // 前方に力を加える
            }
        }
        else
        {
            Debug.LogWarning("FeedPrefab または spawnPoint が設定されていません。");
        }
    }
}
