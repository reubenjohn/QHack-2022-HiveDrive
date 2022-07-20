using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Members : MonoBehaviour
{
    [SerializeField] Text population = null;

    void Update() => population.text = transform.childCount.ToString();
}
