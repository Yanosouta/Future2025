using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapibaraSpawner : MonoBehaviour
{
    public GameObject aiPrefab;      // 生成するAIオブジェクトのPrefab
    public float spawnInterval = 5f; // 生成する間隔（秒）
    public int maxObjects = 10;      // 生成するAIオブジェクトの最大数
    public float nextSpawnTime;     // 次に生成する時間

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval; // 最初の生成時間を設定
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("KAPIBARA").Length < maxObjects) // "AI"タグがついているオブジェクトを数える
        {
            SpawnAIObject();
            nextSpawnTime = Time.time + spawnInterval; // 次の生成時間を更新
        }
    }

    void SpawnAIObject()
    {
        // ランダムな位置を生成するための範囲を設定
        Vector3 randomPosition = new Vector3(
            Random.Range(-5f, 5f), // X軸の範囲
            0,  // 一旦y軸は0に固定
            Random.Range(-5f, 5f)  // Z軸の範囲
        );

        // 生成する位置をシーン上の適切な位置（地面）にスナップ
        RaycastHit hit;
        if (Physics.Raycast(randomPosition + Vector3.up * 10, Vector3.down, out hit, 100f))
        {
            randomPosition.y = hit.point.y; // 地面の高さにスナップ
        }

        // AIオブジェクトを生成
        Instantiate(aiPrefab, randomPosition, Quaternion.identity);
        Debug.Log("AI object spawned at: " + randomPosition);
    }
}
