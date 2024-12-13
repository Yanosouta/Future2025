using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFeed : MonoBehaviour
{
    public GameObject FeedPrefab; // 投げるエサのプレハブを設定
    public Transform spawnPoint; // エサの生成位置を設定
    public GameObject DebugMode;
    public GameObject Target;
    static public bool DebugFlg = false;
    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(DebugFlg)
            {
                DebugFlg = false;
                DebugMode.SetActive(false);
                Target.SetActive(false);
            }
            else
            {
                DebugFlg = true;
                DebugMode.SetActive(true);
                Target.SetActive(true);
            }
        }
        // キー入力でエサを投げる
        if(DebugFlg)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Throw();
            }
        }    
    }

    public void Throw()
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
        }
        else
        {
            Debug.LogWarning("FeedPrefab または spawnPoint が設定されていません。");
        }
    }

}
