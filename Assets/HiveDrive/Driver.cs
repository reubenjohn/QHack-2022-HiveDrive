using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HiveDrive
{
    public class Driver : MonoBehaviour
    {
        public const float MinRadius = 3f;
        public const float LaneChangeSpeed = 0.05f;
        public const float LaneWidth = .25f;
        public const int NLanes = 4;

        public float lane = 0;
        public float targetLane = 0;
        public float angularPosition = 0;
        public float speed = .1f;
        public float range = 2f;

        private VehicleSpawner spawner;
        private SpriteRenderer spriteRenderer;
        private float health;
        private int lastRecievedEta = -1;

        public IEnumerable<Driver> DriversInRange =>
            spawner.Drivers
                .Where(driver => driver != this && (transform.position - driver.transform.position).magnitude < range);

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spawner = GetComponentInParent<VehicleSpawner>();
            lane = targetLane = Mathf.RoundToInt(Random.Range(0, NLanes - 1));
            speed = Random.Range(0.1f, .2f) + (TryGetComponent<EmergencyService>(out _) ? 0.3f : 0f);
            angularPosition = Random.Range(0f, 360f);
        }

        private float Radius => MinRadius + lane * LaneWidth;
        private float AngularVelocity => speed / Radius;

        private void Update()
        {
            angularPosition = (angularPosition + AngularVelocity) % 360f;
            transform.rotation = Quaternion.AngleAxis(angularPosition + 90f, Vector3.back);
            transform.position = Quaternion.AngleAxis(angularPosition, Vector3.back) *
                                 (Vector2.up * Radius);
            lane += (targetLane - lane) * LaneChangeSpeed;

            foreach (var driverB in DriversInRange)
            {
                Debug.DrawLine(transform.position, driverB.transform.position, Color.cyan);
            }
        }

        public void BroadcastEmergencyRoute(int eta, float origin, float destination, float emergencyLane)
        {
            if (IsRelevant(eta, origin, destination, emergencyLane)) return;
            lastRecievedEta = eta;
            StartCoroutine(ComplyWithEmergencyBroadcast(eta, origin, destination, emergencyLane));
        }

        private IEnumerator ComplyWithEmergencyBroadcast(int eta, float origin, float destination, float emergencyLane)
        {
            LeaveLane(emergencyLane);
            spriteRenderer.color = Color.yellow;
            // foreach (var driver in DriversInRange)
            // {
            //     driver.BroadcastEmergencyRoute(eta, origin, destination, emergencyLane);
            // }

            while (Mathf.Abs(lane - targetLane) > 0.5f)
            {
                yield return new WaitForSeconds(Random.Range(2f, 6f));
            }

            lastRecievedEta = -1;
            spriteRenderer.color = Color.blue;
        }

        private void LeaveLane(float laneToLeave)
        {
            if (Mathf.Abs(targetLane - laneToLeave) > 0.9f) return;
            if (laneToLeave < 1f) targetLane++;
            else if (targetLane > NLanes - 2f) targetLane--;
            else targetLane += 2 * Random.Range(0, 1) - 1;
        }

        private bool IsRelevant(int eta, float origin, float destination, float emergencyLane)
        {
            if (eta == lastRecievedEta) return false;
            if (Math.Abs(emergencyLane - targetLane) < 0.9f * LaneWidth)
                return false;
            if (origin <= destination)
                return origin <= angularPosition && angularPosition <= destination;
            return angularPosition >= origin || angularPosition <= destination;
        }
    }
}