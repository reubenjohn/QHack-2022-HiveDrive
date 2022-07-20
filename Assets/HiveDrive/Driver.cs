using UnityEngine;
using static Utils;

namespace HiveDrive
{
    public class Driver : MonoBehaviour
    {
        public static readonly float MinRadius = 3f;
        public static readonly float LaneWidth = .25f;

        [SerializeField] private float alignmentCoefficient = .1f;
        [SerializeField] private float healthSpeed = 1e-8f;
        [SerializeField] private float explosionRadius = 1f;
        [SerializeField] private float explosionPower = 10f;
        public Transform front;
        public Transform spawnPoint;
        
        public float lane = 0;
        public float angle = 0;
        public float speed = .1f;

        private SpriteRenderer spriteRenderer;
        private float health;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            lane = Mathf.RoundToInt(Random.Range(0, 3f));
            speed = Random.Range(0.1f, .2f);
            angle = Random.Range(0f, 360f);
        }

        private float Radius => MinRadius + lane * LaneWidth;
        private float AngularVelocity => speed / Radius;

        private void Update()
        {
            angle = (angle + AngularVelocity) % 360f;
            transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.back);
            transform.position = Quaternion.AngleAxis(angle, Vector3.back) *
                                 (Vector2.up * Radius);
            // spriteRenderer.color = Color.Lerp(Color.red, Color.blue, .5f + health * 500f);
        }

        private void OnCollisionStay2D(Collision2D other) => LerpHealth(-10);

        private void OnTriggerStay2D(Collider2D other) // obstacle in range
        {
            AvoidCollision(other);
            if (other.TryGetComponent(out Driver bird))
                TryAlignWithDriver(bird);
        }

        private void AvoidCollision(Collider2D other)
        {
            Vector2 vectorToTarget = other.ClosestPoint(front.position) - (Vector2)front.position;
        }

        private void TryAlignWithDriver(Driver bird) =>
            transform.rotation = Quaternion.Slerp(transform.rotation, bird.transform.rotation, alignmentCoefficient);

        private void LerpHealth(float target) => health = Lerp(health, target, healthSpeed);
    }
}