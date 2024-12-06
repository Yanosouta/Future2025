using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayVisualizer : MonoBehaviour
{
    public float rayLength = 10f; // Rayの長さ
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // LineRendererに2つの点を設定
        lineRenderer.enabled = true; // 最初に有効化しておく
    }

    void FixedUpdate()
    {
        Vector3 rayOrigin = transform.position; // Rayの始点（オブジェクトの位置）
        Vector3 rayDirection = transform.forward; // Rayの方向（オブジェクトの前方）

        // 始点とデフォルトの終点を設定
        lineRenderer.SetPosition(0, rayOrigin);
        lineRenderer.SetPosition(1, rayOrigin + rayDirection * rayLength);

        Ray ray = new Ray(rayOrigin, rayDirection);
        RaycastHit hit;

        // Rayがヒットした場合
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // ヒットした位置までに調整
            lineRenderer.SetPosition(1, hit.point);
        }
    }
}
