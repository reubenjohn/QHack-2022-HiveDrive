using System;
using System.Collections;
using System.Collections.Generic;
using HiveDrive;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public GameObject members;
    public float range = 2f;
    
    private void Start()
    {
        members = GameObject.Find("Environment").transform.Find("Members").gameObject;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        var position = transform.position;
        var drivers = members.GetComponentsInChildren<Driver>();
        foreach (var driverA in drivers)
        {
            if ((position - driverA.transform.position).magnitude < range)
            {
                Debug.DrawLine(position, driverA.transform.position, Color.green);
            }
        }
    }
}
