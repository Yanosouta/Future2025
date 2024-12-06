using UnityEngine;
using Es.WaveformProvider;

public class FloatingWithWaves : MonoBehaviour
{
    public WaveConductor waveConductor; // 波のデータを提供するスクリプト
    public Transform waterPlane;       // 水面の基準（平面オブジェクト）
    public Rigidbody rb;               // オブジェクトの Rigidbody
    public float floatHeight = 1f;     // 浮く高さ
    public float bounceDamp = 0.05f;   // 揺れの減衰

    private bool isAboveWater = true;  // 初期状態では水面の上にいると仮定

    void FixedUpdate()
    {
        if (waveConductor == null || waterPlane == null || rb == null)
            return;

        // 現在のオブジェクト位置
        Vector3 objectPos = transform.position;

        // 水面の高さを取得
        Vector3 waterPos = GetWaveHeightAtPosition(objectPos);

        if (isAboveWater)
        {
            // 自由落下中（Sphereが水面より上にある場合）
            if (objectPos.y <= waterPos.y)
            {
                // 水面に到達したら浮力フェーズに移行
                isAboveWater = false;
            }
        }
        else
        {
            // 水面以下にいる場合は浮力を適用
            float displacementMultiplier = Mathf.Clamp01((waterPos.y + floatHeight - objectPos.y) / floatHeight);
            Vector3 buoyancyForce = Vector3.up * displacementMultiplier * rb.mass * Physics.gravity.magnitude;
            rb.AddForce(buoyancyForce, ForceMode.Acceleration);

            // 揺れの減衰
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * (1f - bounceDamp), rb.velocity.z);
        }
    }

    Vector3 GetWaveHeightAtPosition(Vector3 position)
    {
        // ローカル座標をテクスチャUVに変換
        Vector3 localPos = waterPlane.InverseTransformPoint(position);
        float u = localPos.x / waterPlane.localScale.x + 0.5f;
        float v = localPos.z / waterPlane.localScale.z + 0.5f;

        // 波データから高さを取得
        RenderTexture.active = waveConductor.Output;
        Texture2D waveTexture = new Texture2D(waveConductor.Output.width, waveConductor.Output.height, TextureFormat.RGBA32, false);
        waveTexture.ReadPixels(new Rect(0, 0, waveTexture.width, waveTexture.height), 0, 0);
        waveTexture.Apply();

        int x = Mathf.Clamp((int)(u * waveTexture.width), 0, waveTexture.width - 1);
        int y = Mathf.Clamp((int)(v * waveTexture.height), 0, waveTexture.height - 1);

        float waveHeight = waveTexture.GetPixel(x, y).r; // 波の高さは赤チャンネルで表現されると仮定
        Destroy(waveTexture);

        // 波の高さをワールド座標に変換
        return new Vector3(position.x, waveHeight * waterPlane.localScale.y, position.z);
    }
}
