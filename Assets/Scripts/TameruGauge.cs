using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TameruGauge : MonoBehaviour
{
    public RectTransform fill;

    void Start()
    {
        
    }

    void Update()
    {
        float per = PointManager.Instance.Point / (float)PointManager.Instance.MaxPoint;
        per = Mathf.Clamp01(per);

        fill.localScale = new Vector3(per, 1, 1);
    }
}
