using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnGauge : MonoBehaviour
{
    public Image spawnGauge; // UIゲージ（Image）
    private KapibaraSpawner spawner; // AIオブジェクトスポーナー

    void Start()
    {
        spawner = FindObjectOfType<KapibaraSpawner>(); // スポーナーを取得
        if (spawnGauge != null && spawner != null)
        {
            spawnGauge.fillAmount = 0; // 初期値を0に設定
        }
    }

    void Update()
    {
        if (spawner != null && spawnGauge != null)
        {
            // スポーンまでの時間を計算
            float remainingTime = spawner.spawnInterval - (Time.time - (spawner.nextSpawnTime - spawner.spawnInterval));
            // ゲージのFillAmountを更新（0から1の範囲に変換）
            spawnGauge.fillAmount = Mathf.Clamp01(1 - (remainingTime / spawner.spawnInterval));
        }
    }
}
