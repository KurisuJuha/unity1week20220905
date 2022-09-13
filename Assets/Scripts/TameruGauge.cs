using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TameruGauge : MonoBehaviour
{
    public UINumber uINumber1;
    public UINumber uINumber2;
    public RectTransform fill;

    void Start()
    {
        
    }

    void Update()
    {
        int maxpoint = PointManager.Instance.MaxPoint;
        int point = PointManager.Instance.Point;
        int totalpoint = PointManager.Instance.TotalPoint;

        uINumber1.number = point;
        uINumber2.number = maxpoint;

        fill.localScale = new Vector3(point / (float)(maxpoint == 0 ? 1 : maxpoint), 1, 1);
    }
}
