using UnityEngine;

public class Floating : MonoBehaviour
{
    public float waterLevel = -12.25f;  // 水面の高さ
    public float floatStrength = 3.0f;  // 浮力の強さ
    public float damping = 0.5f;  // 水中での抵抗
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // オブジェクトが水面より下にある場合に浮力を加える
        if (transform.position.y < waterLevel)
        {
            float force = (waterLevel - transform.position.y) * floatStrength;
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);

            // 水中での減衰処理（滑りを防止）
            rb.velocity = new Vector3(rb.velocity.x * (1 - damping), rb.velocity.y, rb.velocity.z * (1 - damping));
        }
    }
}
