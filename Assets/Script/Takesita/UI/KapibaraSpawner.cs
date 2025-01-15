using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KapibaraSpawner : MonoBehaviour
{
    public List<GameObject> aiPrefabs; // 生成するAIオブジェクトのPrefabリスト
    public float spawnInterval = 5f;   // 生成する間隔（秒）
    public int maxObjects = 10;       // 生成するAIオブジェクトの最大数
    public float nextSpawnTime;       // 次に生成する時間
    private List<GameObject> availablePrefabs; // 残りの生成可能なPrefabリスト
    public Transform parentObject;    // 親オブジェクトをインスペクターから指定

    public float minScale = 0.5f; // 最小スケール（インスペクターで設定可能）
    public float maxScale = 2f;  // 最大スケール（インスペクターで設定可能）

    public CameraChange camChange;
    KapiDicData KapiData;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval; // 最初の生成時間を設定
        availablePrefabs = new List<GameObject>(aiPrefabs); // プレハブリストを複製して使用
        if (camChange == null)
        {
            camChange = FindObjectOfType<CameraChange>(); // CameraChangeスクリプトを自動で見つける
        }
    }

    void Update()
    {
        if (KapiData == null)
            KapiData = FindObjectOfType<KapiDicData>();

        if (camChange == null)
        {
            camChange = FindObjectOfType<CameraChange>(); // CameraChangeスクリプトを自動で見つける
        }
        // "KAPIBARA"タグのついたオブジェクトがmaxObjects以下の場合に生成を行う
        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("KAPIBARA").Length < maxObjects)
        {
            SpawnAIObject();
            nextSpawnTime = Time.time + spawnInterval; // 次の生成時間を更新
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

        Vector3 randomPosition = new Vector3(
                   Random.Range(-32.3f, -35.0f), // X軸の範囲
                   -12.64f,  // Y軸は0に固定
                   Random.Range(10.3f, 12f)  // Z軸の範囲
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

        // ランダムなスケールを計算し、全軸に適用
        float randomScale = Random.Range(minScale, maxScale);
        spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        Debug.Log("AI object spawned at: " + randomPosition + " with uniform scale: " + randomScale + " using prefab: " + selectedPrefab.name);

        // 生成したオブジェクトの親を設定
        spawnedObject.transform.SetParent(parentObject);

        // 使用したPrefabをリストから削除
        availablePrefabs.RemoveAt(randomIndex);

        // 生成されたオブジェクト内にカメラが存在する場合、CameraChangeに登録
        Camera spawnedCamera = spawnedObject.GetComponentInChildren<Camera>();
        if (spawnedCamera != null)
        {
            camChange.AddCamera(spawnedCamera.gameObject);
        }

        // 生成したオブジェクトのIdを参照
        KapiID kapiID = spawnedObject.GetComponent<KapiID>();
        if (kapiID != null)
        {
            Debug.Log("すぽおおおおおおおおおおおおおんID:" + kapiID.KapiId);
        }

        KapiData.MarkAsKapi(kapiID.KapiId);
    }
}
