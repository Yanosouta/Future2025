using UnityEngine;
using Es.WaveformProvider;
public class FloatingSphere : MonoBehaviour
{
    public Transform waterSurface; // Plane‚ÌTransform‚ğw’è
    public float buoyancyForce = 10f; // •‚—Í‚Ì‹­‚³
    public float dampingFactor = 0.5f; // ’ïR—Í
    private Rigidbody rb;
    private float sphereRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereRadius = sphereCollider.radius * transform.localScale.y; // ƒXƒP[ƒ‹‚ğl—¶‚µ‚½”¼Œa
    }

    void FixedUpdate()
    {
        // …–Ê‚Ì‚‚³‚ğæ“¾
        float waterHeight = waterSurface.position.y;

        // Sphere‚Ì’ê•”‚ÌˆÊ’u (’†S - ”¼Œa)
        float sphereBottom = transform.position.y - sphereRadius;

        // Sphere‚ª…–Ê‰º‚É‚ ‚éê‡‚É•‚—Í‚ğ‰Á‚¦‚é
        if (sphereBottom < waterHeight)
        {
            float depth = waterHeight - sphereBottom;

            // •‚—ÍŒvZ ([‚³‚É”ä—á)
            Vector3 buoyancy = Vector3.up * buoyancyForce * depth;

            // ’ïR—Í
            Vector3 drag = -rb.velocity * dampingFactor;

            // Rigidbody‚É—Í‚ğ‰Á‚¦‚é
            rb.AddForce(buoyancy + drag, ForceMode.Force);
        }
    }
}
