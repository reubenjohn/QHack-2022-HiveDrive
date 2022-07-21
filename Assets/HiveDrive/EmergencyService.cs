using System.Collections;
using System.Collections.Generic;
using HiveDrive;
using UnityEngine;

public class EmergencyService : MonoBehaviour
{
    private Driver driver;
    private SpriteRenderer spriteRenderer;

    private bool isEmergency = false;

    private void Start()
    {
        driver = GetComponent<Driver>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        StartCoroutine(SpontaneousEmergency());
    }

    private IEnumerator EmergencyLights()
    {
        while (isEmergency)
        {
            yield return new WaitForSeconds(.25f);
            spriteRenderer.color = Color.blue;
            yield return new WaitForSeconds(.25f);
            spriteRenderer.color = Color.red;
        }
    }

    private IEnumerator SpontaneousEmergency()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 6f));
            isEmergency = true;
            StartCoroutine(EmergencyLights());
            for(var second = Random.Range(8, 24); second >= 0; second--) {
                foreach (var nearbyDriver in driver.DriversInRange)
                    nearbyDriver.BroadcastEmergencyRoute(second, driver.angularPosition, Random.Range(0f, 360f), driver.lane);
                yield return new WaitForSeconds(0.25f);
            }
            isEmergency = false;
        }
    }

    private void Update()
    {
        
    }
}
