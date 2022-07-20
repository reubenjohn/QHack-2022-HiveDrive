using System.Collections;
using System.Collections.Generic;
using HiveDrive;
using UnityEngine;

public class DriverSpawner : MonoBehaviour
{
    public GameObject driver;
    public int targetPopulation = 10;

    private void Start()
    {
        for (var i = 0; i < targetPopulation; i++) Instantiate(driver, transform);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        for (var i = 0; i < 4; i++)
            Gizmos.DrawWireSphere(transform.position, Driver.MinRadius + Driver.LaneWidth * i);
    }

    private void Update()
    {
        var drivers = GetComponentsInChildren<Driver>();
        foreach (var driverA in drivers)
        {
            foreach (var driverB in drivers)
            {
                if ((driverA.transform.position - driverB.transform.position).magnitude < 2f)
                {
                    Debug.DrawLine(driverA.transform.position, driverB.transform.position, Color.cyan);
                }
            }
        }
    }
}