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

        [SerializeField, Range(0f, 2f)]
        private float strength = 1f;

        [SerializeField, Range(0f, 10f)]
        private float mass = 3f;

        private Vector3 previousPosition;
        private float velocityMagnitude;

        private void Awake()
        {
            previousPosition = transform.position;
        }

        private void Update()
        {
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
            // 現在の位置から速度を計算
            Vector3 currentPosition = transform.position;
            Vector3 displacement = currentPosition - previousPosition;
            float velocityMagnitude = displacement.magnitude / Time.deltaTime;

            foreach (var p in collision.contacts)
            {
                var canvas = p.otherCollider.GetComponent<WaveConductor>();
                if (canvas != null)
                    canvas.Input(waveform, p.point, velocityMagnitude * mass * inputScaleFitter, strength);
            }

            // 衝突後の位置を再設定
            previousPosition = currentPosition;
        }
    }
}
