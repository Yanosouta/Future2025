using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKapiSpawn : MonoBehaviour
{
    public List<GameObject> aiPrefabs; // 生成するAIオブジェクトのPrefabリスト
    private List<GameObject> availablePrefabs; // 残りの生成可能なPrefabリスト
    public Transform parentObject; // 親オブジェクトをインスペクターから指定
    // Start is called before the first frame update
    void Start()
    {
        availablePrefabs = new List<GameObject>(aiPrefabs); // プレハブリストを複製して使用
    }

    // Update is called once per frame
    void Update()
    {
        if(DebugFeed.DebugFlg)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SpawnAIObject();
                //nextSpawnTime = Time.time + spawnInterval; // 次の生成時間を更新
            }
        }      
    }

    void SpawnAIObject()
    {
        // 生成可能なPrefabがない場合は何もしない
        if (availablePrefabs.Count == 0)
        {
            Debug.Log("No more unique prefabs available to spawn.");
            return;
        }

        // ランダムな位置を生成するための範囲を設定
        Vector3 randomPosition = new Vector3(
            Random.Range(-28f, -13f), // X軸の範囲
            -12.64f,  // Y軸は0に固定
            Random.Range(-5f, 4f)  // Z軸の範囲
        );

        // 生成する位置を地面にスナップ
        RaycastHit hit;
        if (Physics.Raycast(randomPosition + Vector3.up * 10, Vector3.down, out hit, 100f))
        {
            randomPosition.y = hit.point.y; // 地面の高さにスナップ
        }

        // ランダムにPrefabを選択して生成し、使用済みとしてリストから削除
        int randomIndex = Random.Range(0, availablePrefabs.Count);
        GameObject selectedPrefab = availablePrefabs[randomIndex];
        GameObject spawnedObject = Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
        //Instantiate(selectedPrefab, randomPosition, Quaternion.identity);
        Debug.Log("AI object spawned at: " + randomPosition + " using prefab: " + selectedPrefab.name);

        // 生成したオブジェクトの親を設定
        spawnedObject.transform.SetParent(parentObject);

        // 使用したPrefabをリストから削除
        availablePrefabs.RemoveAt(randomIndex);
    }
}
