using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Gauge : MonoBehaviour
{
    [Range(0,1)]
    public float Range;
    public GameObject Pivot;

    void Update()
    {
        Pivot.transform.localScale = new Vector3(Mathf.Clamp01(Range), 1, 1);
    }
}
