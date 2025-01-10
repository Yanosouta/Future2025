using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Es.WaveformProvider.Sample
{
    public class CollisionInput : MonoBehaviour
    {
        [SerializeField]
        private Texture2D waveform;

        [SerializeField, Range(0f, 1f)]
        private float inputScaleFitter = 0.01f;

        [SerializeField, Range(0f, 1f)]
        private float strength = 1f;

        [SerializeField, Range(0f, 5f)]
        private float mass = 3f;

        private Vector3 previousPosition;
        private float velocityMagnitude;

        private void Awake()
        {
            previousPosition = transform.position;
        }

        private void Update()
        {
            // ‘¬“x‚ðˆÊ’u‚Ì•Ï‰»‚©‚çŒvŽZ‚·‚é
            Vector3 displacement = transform.position - previousPosition;
            velocityMagnitude = displacement.magnitude / Time.deltaTime;
            previousPosition = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            WaveInput(collision);
        }

        public void OnCollisionStay(Collision collision)
        {
            WaveInput(collision);
        }

        private void WaveInput(Collision collision)
        {
            foreach (var p in collision.contacts)
            {
                var canvas = p.otherCollider.GetComponent<WaveConductor>();
                if (canvas != null)
                    canvas.Input(waveform, p.point, velocityMagnitude * mass * inputScaleFitter, strength);
            }
        }
    }
}
