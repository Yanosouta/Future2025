using UnityEngine;

public class IgnoreMeshCollider : MonoBehaviour
{
    public string waterTag = "Water";  // 水面オブジェクトに設定するタグ

    void Start()
    {
        // タグで水面オブジェクトを探す
        GameObject waterSurface = GameObject.FindGameObjectWithTag(waterTag);

        if (waterSurface != null)
        {
            MeshCollider waterMeshCollider = waterSurface.GetComponent<MeshCollider>();
            Collider objectCollider = GetComponent<Collider>();

            if (waterMeshCollider != null && objectCollider != null)
            {
                // 衝突を無視
                Physics.IgnoreCollision(objectCollider, waterMeshCollider);
                Debug.Log($"{gameObject.name} と {waterSurface.name} の Mesh Collider の衝突を無視しました");
            }
            else
            {
                Debug.LogWarning("MeshCollider または Collider が見つかりません");
            }
        }
        else
        {
            Debug.LogError("タグ 'Water' のオブジェクトが見つかりません");
        }
    }
}
