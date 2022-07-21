using UnityEngine;
using static Utils;

namespace HiveDrive
{
    public class Driver : MonoBehaviour
    {
        public const float MinRadius = 3f;
        public const float LaneWidth = .25f;

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
    }
}