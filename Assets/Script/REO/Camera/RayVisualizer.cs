using UnityEngine;

public class RayVisualizer : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer コンポーネント
    public Camera mainCamera; // レイを飛ばすカメラ
    public float rayLength = 100f; // レイの長さ
    private Vector2 cursorPosition;

    void Start()
    {
        // LineRenderer の設定
        lineRenderer.positionCount = 2; // レイの開始点と終了点の2つ
    }

    void Update()
    {
        // カーソル位置からレイを飛ばす
        Ray ray = mainCamera.ScreenPointToRay(cursorPosition);

        // LineRenderer にレイの開始点と終了点を設定
        lineRenderer.SetPosition(0, ray.origin); // レイの始点
        lineRenderer.SetPosition(1, ray.origin + ray.direction * rayLength); // レイの終点
    }

    public void SetCursorPosition(Vector2 position)
    {
        cursorPosition = position;
    }
}
