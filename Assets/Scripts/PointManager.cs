using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : SingletonMonoBehaviour<PointManager>
{
    public int TotalPoint;
    public int Point;
    public int MaxPoint;
    public int wave;

    public void addPoint(int id)
    {
        if (GameManager.Instance.settings.items[id].Can_AddPoint)
        {
            Point += GameManager.Instance.settings.items[id].point;

            if (Point >= MaxPoint) 
            {
                SkillManager.Instance.Show();
                wave++;
                TotalPoint += Point;
                Point = 0;
                MaxPoint = Mathf.FloorToInt(GameManager.Instance.settings.MaxPointCurve.Evaluate(wave));
            }
        }
    }

    public static void AddPoint(int id) => Instance.addPoint(id);
}
