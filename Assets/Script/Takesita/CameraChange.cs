using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public List<GameObject> cameras = new List<GameObject>(); // カメラを格納するリスト
    private int camNum = 0;

    void Start()
    {
        // 最初にすべてのカメラを非アクティブ化
        foreach (GameObject cam in cameras)
        {
            cam.SetActive(false);
        }

        // 最初のカメラをアクティブに設定
        if (cameras.Count > 0)
        {
            cameras[camNum].SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 現在のカメラを非アクティブ化
            cameras[camNum].SetActive(false);

            // 次のカメラへ
            camNum = (camNum + 1) % cameras.Count;

            // 新しいカメラをアクティブ化
            cameras[camNum].SetActive(true);
        }
    }

    // 外部からカメラを追加するためのメソッド
    public void AddCamera(GameObject newCamera)
    {
        if (!cameras.Contains(newCamera))
        {
            cameras.Add(newCamera); // 新しいカメラをリストに追加
            newCamera.SetActive(false); // 追加されたカメラを非表示にする
        }

        // 最初のカメラをアクティブ化
        if (cameras.Count == 1)
        {
            cameras[0].SetActive(true);
        }
    }
}
