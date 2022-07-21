using System.Collections.Generic;
using UnityEngine;

namespace HiveDrive
{
    public class VehicleSpawner : MonoBehaviour
    {
        public GameObject driver;
        public GameObject emergencyVehicle;
        public int targetPopulation = 10;

        public IEnumerable<Driver> Drivers => GetComponentsInChildren<Driver>();

        private void Start()
        {
            for (var i = 0; i < Driver.NLanes; i++)
            {
                var driver = Instantiate(emergencyVehicle, transform).GetComponent<Driver>();
                driver.targetLane = driver.lane = i;
                driver.range = 3f;
            }

            for (var i = 0; i < targetPopulation; i++) Instantiate(driver, transform);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (var i = 0; i < Driver.NLanes; i++)
                Gizmos.DrawWireSphere(transform.position, Driver.MinRadius + Driver.LaneWidth * i);
        }

        private void Update()
        {
            var drivers = Drivers;
            foreach (var driverA in drivers) { }
        }
    }
}